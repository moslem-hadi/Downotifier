using Hangfire;
using Scheduler.Models.Events;
using Shared.Helper;
using Shared.Messaging;
using static Shared.Constants;

namespace Scheduler.Jobs
{
    public class HttpCallJob
    {
        private readonly IMessagePublisher _messagePublisher;

        public HttpCallJob(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public async Task Run(ApiCallJob msg, IJobCancellationToken cancellationToken)
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
