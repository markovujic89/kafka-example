namespace LeadProducer.Helppers;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = null!;
    public string Topic { get; set; } = null!;
    
    public string? SchemaRegistryUrl { get; set; }
}