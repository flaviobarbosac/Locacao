﻿// <auto-generated />
using System;
using Locacao.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Locacao.Infraestrutura.Migrations
{
    [DbContext(typeof(LocacaoDbContext))]
    [Migration("20240921140354_InitialCreate2")]
    partial class InitialCreate2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Locacao.Domain.Model.Deliveryman", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DriversLicenseImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DriversLicenseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DriversLicenseType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.HasIndex("DriversLicenseNumber")
                        .IsUnique();

                    b.ToTable("Deliverymen");
                });

            modelBuilder.Entity("Locacao.Domain.Model.Motorcycle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LicensePlate")
                        .IsUnique();

                    b.ToTable("Motorcycles");
                });

            modelBuilder.Entity("Locacao.Domain.Model.MotorcycleRegistrationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MotorcycleId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MotorcycleRegistrationEvents");
                });

            modelBuilder.Entity("Locacao.Domain.Model.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DeliverymanId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MotorcycleId")
                        .HasColumnType("uuid");

                    b.Property<int>("RentalPlan")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("DeliverymanId");

                    b.HasIndex("MotorcycleId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("Locacao.Domain.Model.Rental", b =>
                {
                    b.HasOne("Locacao.Domain.Model.Deliveryman", "Deliveryman")
                        .WithMany("Rentals")
                        .HasForeignKey("DeliverymanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Locacao.Domain.Model.Motorcycle", "Motorcycle")
                        .WithMany("Rentals")
                        .HasForeignKey("MotorcycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deliveryman");

                    b.Navigation("Motorcycle");
                });

            modelBuilder.Entity("Locacao.Domain.Model.Deliveryman", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("Locacao.Domain.Model.Motorcycle", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
