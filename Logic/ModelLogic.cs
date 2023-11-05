using Zeebe.Client;

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
            client.NewCompleteJobCommand(job.Key).Variables("{\"UserExists\":true}}").Send();
        }
    }

    public class FlightLogic : ModelLogic
    {
        public override void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job)
        {
            client.NewCompleteJobCommand(job.Key).Variables("{\"FlightExists\":true}}").Send();
        }

    }

    public class PaymentLogic : ModelLogic
    {
        public override void BusinessLogic(Zeebe.Client.Api.Worker.IJobClient client, Zeebe.Client.Api.Responses.IJob job)
        {
            client.NewCompleteJobCommand(job.Key).Send();
        }
    }
}
