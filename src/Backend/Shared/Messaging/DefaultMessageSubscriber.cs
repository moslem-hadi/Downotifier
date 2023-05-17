namespace Shared.Messaging
{
    internal sealed class DefaultMessageSubscriber : IMessageSubscriber
    {
        Task IMessageSubscriber.SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler)
        => Task.CompletedTask;

    }
}