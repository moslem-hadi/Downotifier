namespace Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string queue, T message) where T : class, IMessage;
}
