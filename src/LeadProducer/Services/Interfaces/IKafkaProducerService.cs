using RealEstate.Shared;

namespace LeadProducer.Services.Interfaces;

public interface IKafkaProducerService
{
    Task ProduceAsync(RealEstateLead lead);
}