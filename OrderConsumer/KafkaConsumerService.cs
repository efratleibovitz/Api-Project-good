using Confluent.Kafka;
using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OrderConsumer;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly string _groupId;

    public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
    {
        _logger = logger;
        _bootstrapServers = configuration["KafkaSettings:BootstrapServers"] ?? "localhost:9092";
        _topic = configuration["KafkaSettings:Topic"] ?? "orders";
        _groupId = configuration["KafkaSettings:GroupId"] ?? "order-consumer";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order consumer starting. Topic: '{Topic}', Group: '{GroupId}'", _topic, _groupId);

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            AllowAutoCreateTopics = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<string, string>? result = null;
                try
                {
                    result = await Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogWarning("Consume error: {Reason} — retrying in 3s", ex.Error.Reason);
                    await Task.Delay(3000, stoppingToken);
                    continue;
                }

                if (result is null) continue;

                try
                {
                    var order = JsonSerializer.Deserialize<Order>(result.Message.Value);
                    if (order is null)
                    {
                        _logger.LogWarning("Could not deserialize order message — skipping.");
                        continue;
                    }

                    _logger.LogInformation(
                        "New Order received — Id: {OrderId}, User: {UserId}, Sum: {Sum}, Date: {Date}, Items: {ItemCount}",
                        order.OrderId, order.UserId, order.OrederSum, order.OrderDate, order.OrderItems.Count);

                    consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing order message — skipping.");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // normal shutdown
        }
        finally
        {
            consumer.Close();
            _logger.LogInformation("Order consumer stopped.");
        }
    }
}
