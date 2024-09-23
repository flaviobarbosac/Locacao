using Locacao.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Locacao.Infraestructure
{
    public class LocacaoDbContext : DbContext
    {
        public LocacaoDbContext(DbContextOptions<LocacaoDbContext> options) : base(options) { }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Deliveryman> Deliverymen { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<MotorcycleRegistrationEvent> MotorcycleRegistrationEvents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorcycle>()
                .HasIndex(m => m.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Deliveryman>()
                .HasIndex(d => d.Cnpj)
                .IsUnique();

            modelBuilder.Entity<Deliveryman>()
                .HasIndex(d => d.DriversLicenseNumber)
                .IsUnique();

            modelBuilder.Entity<User>()
                 .HasIndex(u => u.Username)
                 .IsUnique();
        }
    }
}