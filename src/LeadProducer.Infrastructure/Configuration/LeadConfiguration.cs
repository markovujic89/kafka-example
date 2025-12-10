using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Shared;
using RealEstateLead = LeadProducer.Infrastructure.Entities.RealEstateLead;

namespace LeadProducer.Infrastructure.Configuration;

public class LeadConfiguration: IEntityTypeConfiguration<RealEstateLead>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.General);
    
    public void Configure(EntityTypeBuilder<RealEstateLead> builder)
    {
        builder.ToTable("RealEstateLeads");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Address)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOptions),
                v => JsonSerializer.Deserialize<Address>(v, JsonOptions)!
            );
        
        builder.Property(x => x.Price);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.UpdatedAt);
        builder.Property(x => x.NumberOfRooms);
        builder.Property(x => x.PropertyType);
        builder.Property(x => x.DealType);
        
        builder.HasIndex(x => x.PublicId).IsUnique();
    }
}