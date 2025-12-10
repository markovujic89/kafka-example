namespace RealEstate.Shared;

public class Address
{
    public int ZipCode { get; set; }
    
    public string Street { get; set; } = null!;
    
    public string City { get; set; } = null!;
    
    public string Canton { get; set; } = null!;
}