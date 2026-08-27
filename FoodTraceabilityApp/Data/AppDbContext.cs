using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<RawMaterial> RawMaterials { get; set; }

    public DbSet<FinishedProduct> FinishedProducts { get; set; }

    public DbSet<TraceabilityLink> TraceabilityLinks { get; set; }
}