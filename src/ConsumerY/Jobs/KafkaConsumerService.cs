using System.Text.Json;
using Confluent.Kafka;
using ConsumerY.Helppers;
using Microsoft.Extensions.Options;
using RealEstate.Shared;

namespace ConsumerY.Jobs;

public class KafkaConsumerService : BackgroundService
{
    private readonly KafkaSettings _settings;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(IOptions<KafkaSettings> settings, 
        ILogger<KafkaConsumerService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(_settings.Topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    var lead = JsonSerializer.Deserialize<RealEstateLead>(result.Message.Value);

                    _logger.LogInformation(
                        $"Company Y received lead: {lead!.LeadId}, {lead.Address}, price {lead.Price}"
                    );
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when stoppingToken is triggered
                _logger.LogInformation("Kafka consumer is shutting down gracefully...");
            }
            finally
            {
                consumer.Close();
            }
        }, stoppingToken);
    }
}