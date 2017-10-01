﻿// <auto-generated />
using HealthcareNetCoreSample.Data;
using HealthcareNetCoreSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HealthcareNetCoreSample.Migrations
{
    [DbContext(typeof(PatientContext))]
    [Migration("20170929080359_AmountOwedInClaims")]
    partial class AmountOwedInClaims
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HealthcareNetCoreSample.Models.Claim", b =>
                {
                    b.Property<int>("ClaimID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AmountOwed")
                        .HasColumnType("money");

                    b.Property<int?>("ClaimStatus");

                    b.Property<int>("InsProviderID");

                    b.Property<int>("PatientID");

                    b.HasKey("ClaimID");

                    b.HasIndex("InsProviderID");

                    b.HasIndex("PatientID");

                    b.ToTable("Claim");
                });

            modelBuilder.Entity("HealthcareNetCoreSample.Models.InsProvider", b =>
                {
                    b.Property<int>("InsProviderID");

                    b.Property<string>("InsProviderName")
                        .HasMaxLength(50);

                    b.HasKey("InsProviderID");

                    b.ToTable("InsProvider");
                });

            modelBuilder.Entity("HealthcareNetCoreSample.Models.Patient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FirstMidName")
                        .IsRequired()
                        .HasColumnName("FirstName")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("HealthcareNetCoreSample.Models.Claim", b =>
                {
                    b.HasOne("HealthcareNetCoreSample.Models.InsProvider", "InsProvider")
                        .WithMany("Claims")
                        .HasForeignKey("InsProviderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HealthcareNetCoreSample.Models.Patient", "Patient")
                        .WithMany("Claim")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
