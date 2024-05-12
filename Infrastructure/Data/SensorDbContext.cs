using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Tributech.SensorManager.Application.Data;
using Tributech.SensorManager.Domain.Entities;
using Tributech.SensorManager.Infrastructure.Data.Extensions;

namespace Tributech.SensorManager.Infrastructure.Data;

/*
    Remove migration:
        dotnet ef migrations remove --project infrastructure
    Add migration
        dotnet ef migrations add --project Infrastructure mig_mandatoryMetadata
    Update database
        dotnet ef database update --project Infrastructure

 */

public class SensorDbContext(DbContextOptions options) : DbContext(options), ISensorContext
{
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<MandatoryMetadata> MandatoryMetadatas { get; set; }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await SaveChangesAsync(token);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Type is a value object
            entity
                .Property(s => s.Type)
                .IsSensorType()
                .HasMaxLength(64);

            // Configure SensorMetadata as an owned type
            entity.OwnsMany<SensorMetadata>("Metadata", md =>
            {
                md.ToTable("SensorMetadata");
                md.WithOwner().HasForeignKey("SensorId");
                md.Property<Guid>("Id");
                md.HasKey("Id");
                md.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(64);
                md.Property(m => m.Value)
                    .IsRequired()
                    .HasMaxLength(128);
            });
        });

        modelBuilder.Entity<MandatoryMetadata>(entity =>
        {
            entity.HasKey(mm => mm.Id);

            // unique SensorType
            entity.HasIndex(mm => mm.SensorType)
                .IsUnique();

            // Type is a value object
            entity
                .Property(mm => mm.SensorType)
                .IsSensorType()
                .HasMaxLength(64);

            // Configure SensorMetadata as an owned type
            entity.OwnsMany<MandatoryMetadataItem>("Metadata", md =>
            {
                md.ToTable("MandatoryMetadataItems");
                md.WithOwner().HasForeignKey("MandatoryMetadataId");
                md.Property<Guid>("Id");
                md.HasKey("Id");

                md.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(64);

                md.Property(m => m.Type)
                    .IsRequired()
                    .HasMaxLength(64);

                md.Property(m => m.DefaultValue)
                    .HasMaxLength(128);
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