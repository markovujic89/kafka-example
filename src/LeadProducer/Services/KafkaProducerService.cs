using System.Text.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using LeadProducer.Helppers;
using LeadProducer.Services.Interfaces;
using Microsoft.Extensions.Options;
using RealEstate.Shared;

namespace LeadProducer.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly string _topic;
    private readonly IProducer<string, RealEstateLead> _producer;
    private readonly ISchemaRegistryClient _schemaRegistry;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        _topic = settings.Value.Topic;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers
        };
        
        _schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
        {
            Url = settings.Value.SchemaRegistryUrl
        });

        _producer = new ProducerBuilder<string, RealEstateLead>(config)
            .SetValueSerializer(
                new AvroSerializer<RealEstateLead>(_schemaRegistry))
            .Build();
    }

    public async Task ProduceAsync(RealEstateLead lead)
    {
        var output = await _producer.ProduceAsync(
            _topic,
            new Message<string, RealEstateLead>
            {
                Key = lead.LeadId.ToString(),
                Value = new RealEstateLead
                {
                    LeadId = lead.LeadId,
                    Address = lead.Address,
                    RealEstateType = lead.RealEstateType,
                    LeadType = lead.LeadType,
                    Price = lead.Price
                }
            });
    }
}