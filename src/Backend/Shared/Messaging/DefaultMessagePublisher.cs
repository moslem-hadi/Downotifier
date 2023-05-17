namespace Shared.Messaging
{
    public sealed class DefaultMessagePublisher : IMessagePublisher
    {
        public Task PublishAsync<T>(string queue, T message) where T : class, IMessage => Task.CompletedTask;

    }
}