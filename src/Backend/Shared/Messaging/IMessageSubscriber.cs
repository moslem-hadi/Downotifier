namespace Shared.Messaging;

public interface IMessageSubscriber
{
    Task SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler) where T : class;//, IMessage;
}
