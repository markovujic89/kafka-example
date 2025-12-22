using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using ConsumerX.Helppers;
using Microsoft.Extensions.Options;
using RealEstate.Shared;

namespace ConsumerX.BackgroundJobs;

public class KafkaConsumerService : BackgroundService
{
    private readonly KafkaSettings _settings;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(
        IOptions<KafkaSettings> settings,
        ILogger<KafkaConsumerService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _settings.SchemaRegistryUrl
            };

            using var schemaRegistry =
                new CachedSchemaRegistryClient(schemaRegistryConfig);

            using var consumer =
                new ConsumerBuilder<string, RealEstateLead>(consumerConfig)
                    .SetValueDeserializer(
                        new AvroDeserializer<RealEstateLead>(schemaRegistry)
                            .AsSyncOverAsync())
                    .Build();

            consumer.Subscribe(_settings.Topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    var lead = result.Message.Value; // ALREADY deserialized

                    _logger.LogInformation(
                        "Company X received lead: {LeadId}, {Address}, price {Price}",
                        lead.LeadId,
                        lead.Address,
                        lead.Price
                    );
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer is shutting down gracefully...");
            }
            finally
            {
                consumer.Close();
            }
        }, stoppingToken);
    }
}