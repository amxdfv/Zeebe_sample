using System;
using System.Threading.Tasks;
//using NLog.Extensions.Logging;
using Zeebe.Client.Impl.Builder;

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

            var signal = new EventWaitHandle(false, EventResetMode.AutoReset);

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

        }
    }
}