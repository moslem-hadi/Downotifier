using Hangfire;
using Scheduler.Models.Events;
using Shared.Messaging;

namespace Scheduler.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
        readonly IMessageSubscriber _messageSubscriber;
        readonly ILogger<MessagingBackgroundService> _logger;
        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _messageSubscriber.SubscribeAsync<ApiCallJobCreatedEvent>("job", HandleApiCallJobCreated);
            return Task.CompletedTask;

        }
        public void HandleApiCallJobCreated(MessageEnvelope<ApiCallJobCreatedEvent> message)
        {
            var apiCallJobMessage = message.Message;
            var apiCallJob = new HttpCall();
       
            _logger.LogInformation("Job being added: "+ apiCallJobMessage.Url);
            RecurringJob.RemoveIfExists(apiCallJobMessage.Id.ToString());
            RecurringJob.AddOrUpdate<HttpCall>(
                apiCallJobMessage.Id.ToString(),
                job => apiCallJob.Run(apiCallJobMessage,JobCancellationToken.Null),
                Cron.MinuteInterval(apiCallJobMessage.MonitoringInterval!));

        }


        public class HttpCall  
        {
            public  Task Run(ApiCallJobCreatedEvent msg,IJobCancellationToken cancellationToken)
            {
                Console.WriteLine ("Job is running " + msg.Url);
                if (Random.Shared.Next(0,5) % 2 == 0)
                {

                }
                else{ 
                
                }

                return Task.CompletedTask;
            }
        }
    }
}
