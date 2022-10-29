﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.DevLogix.MasterData;

namespace RingSoft.DevLogix.MasterData.Migrations
{
    [DbContext(typeof(MasterDbContext))]
    [Migration("20221027193423_MigrateDb")]
    partial class MigrateDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("RingSoft.DevLogix.MasterData.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<byte?>("AuthenticationType")
                        .HasColumnType("smallint");

                    b.Property<string>("Database")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("DefaultUser")
                        .HasColumnType("integer");

                    b.Property<string>("FileName")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar");

                    b.Property<string>("FilePath")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("MigrateDb")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("Platform")
                        .HasColumnType("smallint");

                    b.Property<string>("Server")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });
#pragma warning restore 612, 618
        }
    }
}
