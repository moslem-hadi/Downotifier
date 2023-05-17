using Hangfire;
using Scheduler.Models.Events;
using Shared.Helper;
using Shared.Messaging;
using static Shared.Constants;

namespace Scheduler.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
            private readonly IMessagePublisher _messagePublisher;
        readonly IMessageSubscriber _messageSubscriber;
        readonly ILogger<MessagingBackgroundService> _logger;
        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger, IMessagePublisher messagePublisher)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _messageSubscriber.SubscribeAsync<ApiCallJobCreated>(QueueConstants.Job, HandleApiCallJobCreated);
            return Task.CompletedTask;

        }
        public void HandleApiCallJobCreated(MessageEnvelope<ApiCallJobCreated> message)
        {
            var apiCallJobMessage = message.Message;
            var apiCallJob = new HttpCall(_messagePublisher);
       
            _logger.LogInformation("Job being added: "+ apiCallJobMessage.Url);

            //RecurringJob.RemoveIfExists(apiCallJobMessage.Id.ToString());

            RecurringJob.AddOrUpdate<HttpCall>(
                apiCallJobMessage.Id.ToString(),
                job => apiCallJob.Run(apiCallJobMessage,JobCancellationToken.Null),
                Cron.MinuteInterval(apiCallJobMessage.MonitoringInterval!));

        }


        public class HttpCall  
        {
            private readonly IMessagePublisher _messagePublisher;

            public HttpCall(IMessagePublisher messagePublisher)
            {
                _messagePublisher = messagePublisher;
            }

            public  async Task Run(ApiCallJobCreated msg,IJobCancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    _ = await HttpHelper.Request(msg.Method, msg.Url, msg.JsonBody, msg.Headers);
                }
                catch (Exception ex)
                {
                    if (msg.Notifications != null)
                        foreach (var notify in msg.Notifications)
                            await _messagePublisher.PublishAsync(QueueConstants.Notify, notify);
                }
            }
        }
    }
}
