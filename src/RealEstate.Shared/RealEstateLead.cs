using RealEstate.Shared.Enums;

namespace RealEstate.Shared;

public class RealEstateLead
{
    public Guid LeadId { get; set; }
    public string Address { get; set; } = null!;
    public RealEstateType RealEstateType { get; set; }
    public LeadType LeadType { get; set; }
    public decimal Price { get; set; }
}