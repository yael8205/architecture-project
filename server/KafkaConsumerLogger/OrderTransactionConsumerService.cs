using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaConsumerLogger;

public class OrderTransactionConsumerService : BackgroundService
{
    private readonly ILogger<OrderTransactionConsumerService> _logger;
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly string _groupId;

    public OrderTransactionConsumerService(IConfiguration configuration, ILogger<OrderTransactionConsumerService> logger)
    {
        _logger = logger;
        _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
        _topic = configuration["Kafka:Topic"] ?? "order-transactions";
        _groupId = configuration["Kafka:GroupId"] ?? "order-transactions-logger";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order transaction consumer starting. Topic: '{Topic}', Group: '{GroupId}'", _topic, _groupId);

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, string>? result;
                try
                {
                    result = await Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogWarning("Consume error (will retry): {Reason}", ex.Error.Reason);
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    continue;
                }

                if (result is null) continue;

                try
                {
                    _logger.LogInformation(
                        "Order transaction received [partition {Partition}, offset {Offset}]: {Value}",
                        result.Partition.Value, result.Offset.Value, result.Message.Value);

                    consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing order transaction message — skipping.");
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
            _logger.LogInformation("Order transaction consumer stopped.");
        }
    }
}
