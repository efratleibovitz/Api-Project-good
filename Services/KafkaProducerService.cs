using Confluent.Kafka;
using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            _topic = configuration["KafkaSettings:Topic"] ?? "orders";

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["KafkaSettings:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishOrderCreatedAsync(Order order)
        {
            var message = JsonSerializer.Serialize(order, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });

            try
            {
                var result = await _producer.ProduceAsync(_topic, new Message<string, string>
                {
                    Key = order.OrderId.ToString(),
                    Value = message
                });

                _logger.LogInformation("Order {OrderId} sent to Kafka topic '{Topic}'", order.OrderId, _topic);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Failed to publish Order {OrderId} to Kafka topic '{Topic}'", order.OrderId, _topic);
                throw;
            }
        }

        public void Dispose()
        {
            try { _producer?.Flush(TimeSpan.FromSeconds(1)); } catch { }
            _producer?.Dispose();
        }
    }
}
