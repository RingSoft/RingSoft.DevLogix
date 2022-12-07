﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.DevLogix.SqlServer;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    [DbContext(typeof(DevLogixSqlServerDbContext))]
    partial class DevLogixSqlServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("FromFormula")
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RedAlert")
                        .HasColumnType("integer");

                    b.Property<byte?>("RefreshCondition")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("RefreshRate")
                        .HasColumnType("tinyint");

                    b.Property<int?>("RefreshValue")
                        .HasColumnType("integer");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("YellowAlert")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AdvancedFinds");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("ColumnId")
                        .HasColumnType("integer");

                    b.Property<string>("Caption")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<byte>("DecimalFormatType")
                        .HasColumnType("tinyint");

                    b.Property<byte>("FieldDataType")
                        .HasColumnType("tinyint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("PercentWidth")
                        .HasColumnType("numeric(38,17)");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AdvancedFindId", "ColumnId");

                    b.ToTable("AdvancedFindColumns");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.Property<int>("AdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<int>("FilterId")
                        .HasColumnType("integer");

                    b.Property<bool>("CustomDate")
                        .HasColumnType("bit");

                    b.Property<byte>("EndLogic")
                        .HasColumnType("tinyint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<byte>("FormulaDataType")
                        .HasColumnType("tinyint");

                    b.Property<string>("FormulaDisplayValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("LeftParentheses")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Operand")
                        .HasColumnType("tinyint");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("RightParentheses")
                        .HasColumnType("tinyint");

                    b.Property<int?>("SearchForAdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<string>("SearchForValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AdvancedFindId", "FilterId");

                    b.HasIndex("SearchForAdvancedFindId");

                    b.ToTable("AdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.DbLookup.RecordLocking.RecordLock", b =>
                {
                    b.Property<string>("Table")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PrimaryKey")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("LockDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("User")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Table", "PrimaryKey");

                    b.ToTable("RecordLocks");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ErrorFailStatusId")
                        .HasColumnType("integer");

                    b.Property<int?>("ErrorFixStatusId")
                        .HasColumnType("integer");

                    b.Property<int?>("ErrorPassStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("FailText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FixText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<string>("PassText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ErrorFailStatusId");

                    b.HasIndex("ErrorFixStatusId");

                    b.HasIndex("ErrorPassStatusId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ErrorPriority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ErrorPriorities");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ErrorStatuses");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Rights")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ProductVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVersions");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.SystemMaster", b =>
                {
                    b.Property<string>("OrganizationName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("OrganizationName");

                    b.ToTable("SystemMaster");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Rights")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.UsersGroup", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UsersGroups");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Columns")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AdvancedFind");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter", b =>
                {
                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "AdvancedFind")
                        .WithMany("Filters")
                        .HasForeignKey("AdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RingSoft.DbLookup.AdvancedFind.AdvancedFind", "SearchForAdvancedFind")
                        .WithMany("SearchForAdvancedFindFilters")
                        .HasForeignKey("SearchForAdvancedFindId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AdvancedFind");

                    b.Navigation("SearchForAdvancedFind");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Department", b =>
                {
                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", "ErrorFailStatus")
                        .WithMany("FailedDepartments")
                        .HasForeignKey("ErrorFailStatusId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", "ErrorFixStatus")
                        .WithMany("FixedDepartments")
                        .HasForeignKey("ErrorFixStatusId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", "ErrorPassStatus")
                        .WithMany("PassedDepartments")
                        .HasForeignKey("ErrorPassStatusId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ErrorFailStatus");

                    b.Navigation("ErrorFixStatus");

                    b.Navigation("ErrorPassStatus");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ProductVersion", b =>
                {
                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.Product", "Product")
                        .WithMany("Versions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.User", b =>
                {
                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.UsersGroup", b =>
                {
                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Navigation("Columns");

                    b.Navigation("Filters");

                    b.Navigation("SearchForAdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Department", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", b =>
                {
                    b.Navigation("FailedDepartments");

                    b.Navigation("FixedDepartments");

                    b.Navigation("PassedDepartments");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Group", b =>
                {
                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Product", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.User", b =>
                {
                    b.Navigation("UserGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
