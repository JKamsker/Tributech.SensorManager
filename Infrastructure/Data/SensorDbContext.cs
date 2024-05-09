using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tributech.SensorManager.Domain.Entities;

namespace Tributech.SensorManager.Infrastructure.Data;
public class SensorDbContext : DbContext
{
    public SensorDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Sensor> Sensors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Configure SensorMetadata as an owned type
            entity.OwnsMany<SensorMetadata>("Metadata", md =>
            {
                md.WithOwner().HasForeignKey("SensorId");
                md.Property<Guid>("Id");
                md.HasKey("Id");
                md.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(64);
                md.Property(m => m.Value)
                    .IsRequired()
                    .HasMaxLength(128);

                // Configure table name if different from the default
                // md.ToTable("SensorMetadata"); // Uncomment if a separate table is desired
            });
        });
    }
}


public class SensorDbContextFactory : IDesignTimeDbContextFactory<SensorDbContext>
{
    public SensorDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SensorDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TributechSensorManager;Trusted_Connection=True;Encrypt=false");

        return new SensorDbContext(optionsBuilder.Options);
    }
}