﻿using Zeebe.Client;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;

namespace Zeebe_sample.Logic
{
    public abstract class ModelLogic
    {
        public abstract  void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job);
    }

    public class UserLogic : ModelLogic
    {
        public override void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job)
        {
            JObject inputVariables = JObject.Parse(job.Variables);
            string id =  inputVariables.GetValue("user_id").ToString().Replace("\"", "");
            bool UserExists = DatabaseLogic.CheckUser(id);

            if (UserExists == true)
            {
                client.NewCompleteJobCommand(job.Key).Variables(inputVariables.ToString()).Send();
            }
            else
            {
                client.NewThrowErrorCommand(job.Key).ErrorCode("user_not_found").ErrorMessage("Пользователь не найден в базе").Send();
            }
        }
    }

    public class FlightLogic : ModelLogic
    {
        public string? City;
        public int? Day;
        public string? Price;

        public override void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job)
        {
            JObject inputVariables = JObject.Parse(job.Variables);
            string data =  inputVariables.GetValue("date").ToString();
            DateTime parsedDate = DateTime.Parse(data);
            if (parsedDate < DateTime.Now.AddDays(1))
            {
                client.NewThrowErrorCommand(job.Key).ErrorCode("date_too_early").ErrorMessage("Слишком ранняя дата").Send();
                return;
            }
            Day =  (int) parsedDate.DayOfWeek;
            City = inputVariables.GetValue("FlightCity").ToString();
            Price = DatabaseLogic.CheckFlight(City, Day);
            inputVariables.Add("Price", Price);

            if (Price is null)
            {
                client.NewThrowErrorCommand(job.Key).ErrorCode("flight_not_found").ErrorMessage("Рейс не найден").Send();
            } else
            {
                client.NewCompleteJobCommand(job.Key).Variables(inputVariables.ToString()).Send();

            }
        }

    }

    public class PaymentLogic : ModelLogic
    {
        public override void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job)
        {
            JObject inputVariables = JObject.Parse(job.Variables);
            if (DatabaseLogic.CheckAccount(inputVariables.GetValue("user_id").ToString(), inputVariables.GetValue("Price").ToString()) == true)
            {
                client.NewCompleteJobCommand(job.Key).Variables(inputVariables.ToString()).Send();
            } else
            {
                client.NewThrowErrorCommand(job.Key).ErrorCode("not_enough_money").ErrorMessage("Недостаточно денег на счете").Send();
            }
        }
    }
}
