using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangfireController : ControllerBase
    {
        private readonly ILogger<HangfireController> _logger;

        public HangfireController(ILogger<HangfireController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task CreateJob()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Sent similar product offer and suuggestions"), Cron.MinuteInterval(1));
        }
    }
}