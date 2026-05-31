using System.Data.Common;
using EasyShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Travel> Travels => Set<Travel>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Income> Incomes => Set<Income>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Travel>(entity =>
        {
            entity.ToTable("Travels");
            entity.HasKey(e => e.Id);

            entity.ComplexProperty(e => e.ChangeValue, b =>
            {
                b.Property(e => e.Amount).HasColumnName("ChangeValue_Amount");
                b.Property(e => e.Currency).HasColumnName("ChangeValue_Currency").HasConversion<int>();
            });

            entity.ComplexProperty(e => e.ChangeValueToBuy, b =>
            {
                b.Property(e => e.Amount).HasColumnName("ChangeValueToBuy_Amount");
                b.Property(e => e.Currency).HasColumnName("ChangeValueToBuy_Currency").HasConversion<int>();
            });

            entity.ComplexProperty(e => e.MoneyToTravel, b =>
            {
                b.Property(e => e.Amount).HasColumnName("MoneyToTravel_Amount");
                b.Property(e => e.Currency).HasColumnName("MoneyToTravel_Currency").HasConversion<int>();
            });

            entity.HasMany(e => e.Expenses)
                .WithOne()
                .HasForeignKey(e => e.TravelId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Products)
                .WithOne()
                .HasForeignKey(e => e.TravelId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Incomes)
                .WithOne()
                .HasForeignKey(e => e.TravelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);

            entity.ComplexProperty(e => e.TotalCost, b =>
            {
                b.Property(e => e.Amount).HasColumnName("TotalCost_Amount");
                b.Property(e => e.Currency).HasColumnName("TotalCost_Currency").HasConversion<int>();
            });

            entity.ComplexProperty(e => e.UnitPrice, b =>
            {
                b.Property(e => e.Amount).HasColumnName("UnitPrice_Amount");
                b.Property(e => e.Currency).HasColumnName("UnitPrice_Currency").HasConversion<int>();
            });

            entity.ComplexProperty(e => e.UnitChangePrice, b =>
            {
                b.Property(e => e.Amount).HasColumnName("UnitChangePrice_Amount");
                b.Property(e => e.Currency).HasColumnName("UnitChangePrice_Currency").HasConversion<int>();
            });

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TravelId).IsRequired();
            entity.Property(e => e.ImageDataUri);
            entity.Property(e => e.Description).IsRequired(false).HasMaxLength(100);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses");
            entity.HasKey(e => e.Id);

            entity.ComplexProperty(e => e.Value, b =>
            {
                b.Property(e => e.Amount).HasColumnName("Value_Amount");
                b.Property(e => e.Currency).HasColumnName("Value_Currency").HasConversion<int>();
            });

            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.TravelId).IsRequired();
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.ToTable("Incomes");
            entity.HasKey(e => e.Id);

            entity.ComplexProperty(e => e.Value, b =>
            {
                b.Property(e => e.Amount).HasColumnName("Value_Amount");
                b.Property(e => e.Currency).HasColumnName("Value_Currency").HasConversion<int>();
            });

            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.TravelId).IsRequired();
        });
    }

    public void CreateDatabase()
    {
        try
        {
            Database.EnsureCreated();
        }
        catch
        {
        }

        ApplyMigrations();
    }

    private void ApplyMigrations()
    {
        try
        {
            var conn = Database.GetDbConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Travels') WHERE name='ChangeValueToBuy_Amount'";
            var exists = (long)(cmd.ExecuteScalar() ?? 0);
            if (exists == 0)
            {
                cmd.CommandText = "ALTER TABLE Travels ADD COLUMN ChangeValueToBuy_Amount REAL NOT NULL DEFAULT 0";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "ALTER TABLE Travels ADD COLUMN ChangeValueToBuy_Currency INTEGER NOT NULL DEFAULT 0";
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
        }
    }

    public void DeleteDatabase()
    {
        try
        {
            Database.EnsureDeleted();
        }
        catch (Exception ex)
        {
        }
    }
}
