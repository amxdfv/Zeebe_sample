using System;
using System.Threading;
using System.Threading.Tasks;
//using NLog.Extensions.Logging;
using Zeebe.Client.Impl.Builder;
using Zeebe_sample.Logic;

namespace Client.Cloud.Example
{
    public class Program
    {

        // TODO 
        // Сделать логи
        // Написать readme 
       
        public static async Task Main(string[] args)
        {
            ConfigLogic Cl = new ConfigLogic();
            Cl.ReadConfig();

            var zeebeClient =
                    CamundaCloudClientBuilder.Builder()
                        .UseClientId(Cl.client_id)
                        .UseClientSecret(Cl.client_secret)
                        .UseContactPoint(Cl.contact_point)
            //  .UseLoggerFactory(new NLogLoggerFactory()) // optional
                    .Build();

            Thread userTread = new Thread(delegate ()
            {
                UserLogic US = new UserLogic();
                WorkerLogic.BusinessWorker(zeebeClient, "Check_User", US);
            });

            Thread flightTread = new Thread(delegate ()
            {
                FlightLogic FL = new FlightLogic();
                WorkerLogic.BusinessWorker(zeebeClient, "check_schedule", FL);
            });

            Thread paymentTread = new Thread(delegate ()
            {
                PaymentLogic PM = new PaymentLogic();
                WorkerLogic.BusinessWorker(zeebeClient, "Check_Payment", PM);
            });

            userTread.Start();
            flightTread.Start();
            paymentTread.Start();

        }
    }
}