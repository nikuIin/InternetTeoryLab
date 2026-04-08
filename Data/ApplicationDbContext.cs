using Microsoft.EntityFrameworkCore;
using WineShop.Models;

namespace WineShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Wine> Wines { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed-данные хранятся в миграции Migrations/20240101000000_InitialCreate.cs
        // и в SQL-скрипте migration.sql
    }
}
