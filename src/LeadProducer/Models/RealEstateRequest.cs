using RealEstate.Shared.Enums;

namespace LeadProducer.Models;

public class RealEstateRequest
{
    public string Address { get; set; } = null!;
    public RealEstateType RealEstateType { get; set; }
    public LeadType LeadType { get; set; }
    public decimal Price { get; set; }
}