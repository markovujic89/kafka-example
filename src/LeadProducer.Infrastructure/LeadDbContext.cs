using System.Reflection;
using LeadProducer.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeadProducer.Infrastructure;

public class LeadDbContext: DbContext
{
    public LeadDbContext(DbContextOptions<LeadDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public DbSet<RealEstateLead> RealEstateLeads => Set<RealEstateLead>();
}