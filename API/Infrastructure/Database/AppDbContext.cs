using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
    : DbContext(options)
{
    
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Adiciona administrador default (hardcoded por motivos de praticidade)
        modelBuilder.Entity<Administrator>()
            .HasData(
                new Administrator 
                {
                    Id = 1,
                    Email = "admin@email.com",
                    Password = "123456",
                    Role = AdminRoleEnum.Admin
                }
            );
    }
}