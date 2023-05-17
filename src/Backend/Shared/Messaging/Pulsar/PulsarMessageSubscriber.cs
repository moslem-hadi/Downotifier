using System.Reflection;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Shared.Serialization;

namespace Shared.Messaging.Pulsar;

internal sealed class PulsarMessageSubscriber : IMessageSubscriber
{
    private readonly ISerializer _serializer;
    private readonly IPulsarClient _client;
    private readonly string _consumerName;

    public PulsarMessageSubscriber(ISerializer serializer )
    {
        _serializer = serializer;
        _client = PulsarClient.Builder().Build();
        _consumerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? string.Empty;
    }
    
    public async Task SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler) where T : class, IMessage
    {
        var subscription = $"{_consumerName}_{queue}";
        var consumer = _client.NewConsumer()
            .SubscriptionName(subscription)
            .Topic($"persistent://public/default/{queue}")
            .Create();

        await foreach (var message in consumer.Messages())
        {
            var producer = message.Properties["producer"];
            var customId = message.Properties["custom_id"];
            var correlationId = message.Properties["correlationId"];
            var payload = _serializer.DeserializeBytes<T>(message.Data.FirstSpan.ToArray());
            if (payload is not null)
            {
                var json = _serializer.Serialize(payload);
                handler(new MessageEnvelope<T>(payload, correlationId));
            }

            await consumer.Acknowledge(message);
        }
    }
}