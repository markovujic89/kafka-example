using RealEstate.Shared;

namespace LeadProducer.Infrastructure.Entities;

public class RealEstateLead
{
    public int Id { get; set; }
    
    public Address Address { get; set; } = null!;
    
    public decimal Price { get; set; }
    
    public Guid PublicId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public int NumberOfRooms { get; set; }
    
    public short PropertyType { get; set; }
    
    public short DealType { get; set; }
}