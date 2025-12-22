using Confluent.Kafka;

namespace LeadProducer.Helppers;

public class JsonSerializer<T> : Interface.ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
    }
}