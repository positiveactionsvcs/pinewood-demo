using Microsoft.EntityFrameworkCore;
using PinewoodDemo.Data.Models;

namespace PinewoodDemo.Data;

/// <summary>
/// Database context.
/// </summary>
public class PinewoodContext : DbContext
{
    public PinewoodContext()
    {
    }

    public PinewoodContext(DbContextOptions<PinewoodContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    // Tables
    public virtual DbSet<Customer> Customers { get; set; } = null!;
}