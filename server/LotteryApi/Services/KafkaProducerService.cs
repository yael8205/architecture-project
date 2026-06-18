using System.Text.Json;
using Confluent.Kafka;
using LotteryApi.Configuration;
using LotteryApi.Dtos;
using Microsoft.Extensions.Options;

namespace LotteryApi.Services
{
    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
        {
            var settings = kafkaSettings.Value;
            _topic = settings.Topic;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishOrderEventAsync(string eventType, OrderDto order)
        {
            var message = new
            {
                EventType = eventType,
                Timestamp = DateTime.UtcNow,
                Order = order
            };

            var json = JsonSerializer.Serialize(message);

            try
            {
                var result = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
                _logger.LogInformation(
                    "Published {EventType} for order {OrderId} to Kafka topic {Topic} (partition {Partition}, offset {Offset})",
                    eventType, order.Id, _topic, result.Partition.Value, result.Offset.Value);
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError(ex, "Failed to publish {EventType} for order {OrderId} to Kafka topic {Topic}",
                    eventType, order.Id, _topic);
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}
