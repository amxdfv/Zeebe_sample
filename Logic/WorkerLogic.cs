using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Impl.Builder;


namespace Zeebe_sample.Logic
{
    public class WorkerLogic
    {
        public static void BusinessWorker(Zeebe.Client.IZeebeClient zeebeClient, string JobType, ModelLogic ML)
        {
            var signal = new EventWaitHandle(false, EventResetMode.AutoReset);
            zeebeClient.NewWorker().
            JobType(JobType).
                Handler((client, job) =>
                {
                    ML.BusinessLogic(client, job);
                }).
                MaxJobsActive(5).
                Timeout(TimeSpan.FromSeconds(10)).
                PollInterval(TimeSpan.FromSeconds(5))
                .Name(JobType+"name").Open();
            signal.WaitOne();

            
        }


    }
}
