using Hangfire;
using Scheduler.Jobs;
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
            _messageSubscriber.SubscribeAsync<ApiCallJob>(QueueConstants.JobCreateQueue, HandleApiCallJobCreated);
            _messageSubscriber.SubscribeAsync<ApiCallJob>(QueueConstants.JobUpdateQueue, HandleApiCallJobUpdated);
            _messageSubscriber.SubscribeAsync<ApiCallJob>(QueueConstants.JobDeleteQueue, HandleApiCallJobDelete);

            return Task.CompletedTask;

        }
        //TODO: move to separate file
        public void HandleApiCallJobDelete(MessageEnvelope<ApiCallJob> message)
        {
            RecurringJob.RemoveIfExists(message.Message.Id.ToString());
        }
        public void HandleApiCallJobCreated(MessageEnvelope<ApiCallJob> message)
        {
            var apiCallJobMessage = message.Message;
            UpdateHangfire(apiCallJobMessage);
        }
        public void HandleApiCallJobUpdated(MessageEnvelope<ApiCallJob> message)
        {
            var apiCallJobMessage = message.Message;
            RecurringJob.RemoveIfExists(apiCallJobMessage.Id.ToString());
            UpdateHangfire(apiCallJobMessage);
        }

        private void UpdateHangfire(ApiCallJob message)
        {
            var apiCallJob = new HttpCallJob(_messagePublisher);

            _logger.LogInformation("Job being added: " + message.Url);
            
            RecurringJob.AddOrUpdate<HttpCallJob>(
                message.Id.ToString(),
                job => apiCallJob.Run(message, JobCancellationToken.Null),
                Cron.MinuteInterval(message.MonitoringInterval!));
        }



    }
}
