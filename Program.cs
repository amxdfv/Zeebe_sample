using System;
using System.Threading.Tasks;
//using NLog.Extensions.Logging;
using Zeebe.Client.Impl.Builder;

namespace Client.Cloud.Example
{
    public class Program
    {
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

           var oslicWorker =  zeebeClient.NewWorker().
                JobType("call-shit").
                Handler((client, job) =>
                {
                    client.NewCompleteJobCommand(job.Key).Send();
                    Console.WriteLine("sampel_text");
                }).
                MaxJobsActive(5).
                Timeout(TimeSpan.FromSeconds(10)).
                PollInterval(TimeSpan.FromSeconds(5))

                .Name("suslik").Open();

            signal.WaitOne();

           // zeebeClient.JobHandler();

       //     var topology = await zeebeClient.TopologyRequest().Send();

         //  Console.WriteLine("Hello: " + topology);
        }
    }
}