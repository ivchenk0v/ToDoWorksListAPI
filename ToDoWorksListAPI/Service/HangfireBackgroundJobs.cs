using System.Threading.Tasks;
using Hangfire;

namespace ToDoWorksListAPI.Service
{
    public class HangfireBackgroundJobs(ILogger<HangfireBackgroundJobs> logger) : IHangfireBackgroundJobs
    {
        private readonly ILogger _logger = logger;

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 60, 60 })]
        public void ContinuationJob()
        {
            _logger.LogInformation($"Start Continuation job at [{DateTime.Now}]!");
        }
        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 60, 60 })]
        public void DelayedJob()
        {
            _logger.LogInformation($"Start Delayed job at [{DateTime.Now}]!");
        }
        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 60, 60 })]
        public void FireAndForgetJob()
        {
            _logger.LogInformation($"Start Fire and Forget job at [{DateTime.Now}]!");
        }
        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 60, 60 })]
        public void ReccuringJob()
        {
            _logger.LogInformation($"Start Scheduled job at [{DateTime.Now}]!");
        }
    }
}
