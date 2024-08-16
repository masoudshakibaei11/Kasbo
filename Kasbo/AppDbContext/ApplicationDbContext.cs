using Kasbo.Models;
using Microsoft.EntityFrameworkCore;

namespace Kasbo.AppDbContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Chart> Charts { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Stock>().HasData(
            new Stock { Id = 1, Name = "کفرا" }
        );
    }
}