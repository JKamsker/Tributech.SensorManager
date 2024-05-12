﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tributech.SensorManager.Infrastructure.Data;

#nullable disable

namespace Tributech.SensorManager.Infrastructure.Migrations
{
    [DbContext(typeof(SensorDbContext))]
    [Migration("20240510230301_mig_sensor_type_nullable")]
    partial class mig_sensor_type_nullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tributech.SensorManager.Domain.Entities.MandatoryMetadata", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SensorType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("SensorType")
                        .IsUnique();

                    b.ToTable("MandatoryMetadatas");
                });

            modelBuilder.Entity("Tributech.SensorManager.Domain.Entities.Sensor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Type")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Tributech.SensorManager.Domain.Entities.MandatoryMetadata", b =>
                {
                    b.OwnsMany("Tributech.SensorManager.Domain.Entities.MandatoryMetadataItem", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DefaultValue")
                                .HasMaxLength(128)
                                .HasColumnType("nvarchar(128)");

                            b1.Property<string>("Key")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<Guid>("MandatoryMetadataId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.HasKey("Id");

                            b1.HasIndex("MandatoryMetadataId");

                            b1.ToTable("MandatoryMetadataItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MandatoryMetadataId");
                        });

                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("Tributech.SensorManager.Domain.Entities.Sensor", b =>
                {
                    b.OwnsMany("Tributech.SensorManager.Domain.Entities.SensorMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Key")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)");

                            b1.Property<Guid>("SensorId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("nvarchar(128)");

                            b1.HasKey("Id");

                            b1.HasIndex("SensorId");

                            b1.ToTable("SensorMetadata", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("SensorId");
                        });

                    b.Navigation("Metadata");
                });
#pragma warning restore 612, 618
        }
    }
}
