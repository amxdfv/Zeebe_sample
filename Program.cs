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

        // TODO придумать бизнесу-логику и прикрутить БД
        // Сделать логи
        // Написать readme 
        // Убери соединение в конфиг
        // Создать модель для сущностей + вынести каждый воркер в отдельный поток
        public static async Task Main(string[] args)
        {
            var zeebeClient =
                    CamundaCloudClientBuilder.Builder()
                        .UseClientId("KbICvgh5X_O-7jfc8~jdnAIVF675aTy3")
                        .UseClientSecret("QI9hNIDKp3EjG-c-TRAMYsapcRWD-i.SO2.gcBdFysQ.QosCNNIpTjQBComOSdiI")
                        .UseContactPoint("d21832ea-056f-4c99-89e6-f4c93f6e9fc4.syd-1.zeebe.camunda.io")
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

/*            var signal = new EventWaitHandle(false, EventResetMode.AutoReset);

           var userWorker =  zeebeClient.NewWorker().
                JobType("Check_User").
                Handler((client, job) =>
                {
                    client.NewCompleteJobCommand(job.Key).Variables("{\"UserExists\":true}}").Send();
                }).
                MaxJobsActive(5).
                Timeout(TimeSpan.FromSeconds(10)).
                PollInterval(TimeSpan.FromSeconds(5))

                .Name("suslik").Open();


            var flightWorker = zeebeClient.NewWorker().
                JobType("check_schedule").
                Handler((client, job) =>
                {
                    client.NewCompleteJobCommand(job.Key).Send();
                }).
                MaxJobsActive(5).
                Timeout(TimeSpan.FromSeconds(10)).
                PollInterval(TimeSpan.FromSeconds(5))

                .Name("suslik").Open();

            signal.WaitOne();
*/
        }
    }
}