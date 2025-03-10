using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoWorksListAPI.Service;

namespace ToDoWorksListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//(Roles = "admin")
    public class JobsController(IHangfireBackgroundJobs hangfireBackgroundJobs, IBackgroundJobClient jobClient, IRecurringJobManager recurringJobManager) : ControllerBase
    {
        private readonly IBackgroundJobClient _jobClient = jobClient;
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;
        private readonly IHangfireBackgroundJobs _hangfireBackgroundJobs = hangfireBackgroundJobs;

        /// <summary>
        /// Запуск джоба FireAndForgetJob библиотеки Hangfire
        /// </summary>
        /// <returns>Результат запуска</returns>
        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _jobClient.Enqueue(() => _hangfireBackgroundJobs.FireAndForgetJob());
            return Ok();
        }
        /// <summary>
        /// Запуск джоба DelayedJob библиотеки Hangfire
        /// </summary>
        /// <returns>Результат запуска</returns>
        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _jobClient.Schedule(() => _hangfireBackgroundJobs.DelayedJob(), TimeSpan.FromSeconds(60));
            return Ok();
        }
        /// <summary>
        /// Запуск джоба ReccuringJob библиотеки Hangfire
        /// </summary>
        /// <returns>Результат запуска</returns>
        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _hangfireBackgroundJobs.ReccuringJob(), Cron.Minutely);
            return Ok();
        }
        /// <summary>
        /// Запуск джоба ContinuationJob библиотеки Hangfire
        /// </summary>
        /// <returns>Результат запуска</returns>
        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _jobClient.Enqueue(() => _hangfireBackgroundJobs.FireAndForgetJob());
            _jobClient.ContinueJobWith(parentJobId, () => _hangfireBackgroundJobs.ContinuationJob());

            return Ok();
        }
    }
}
