using Confluent.Kafka;

namespace LeadProducer.Helppers.Interface;

public interface ISerializer<T>
{
    byte[] Serialize(T data, SerializationContext context);
}