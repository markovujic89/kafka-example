using System.Text.Json;
using Confluent.Kafka;
using LeadProducer.Helppers;
using LeadProducer.Services.Interfaces;
using Microsoft.Extensions.Options;
using RealEstate.Shared;

namespace LeadProducer.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly string _topic;
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        _topic = settings.Value.Topic;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(RealEstateLead lead)
    {
        var messageValue = JsonSerializer.Serialize(lead);

        var output = await _producer.ProduceAsync(
            _topic,
            new Message<string, string>
            {
                Key = lead.LeadId.ToString(),
                Value = messageValue
            });
    }
}