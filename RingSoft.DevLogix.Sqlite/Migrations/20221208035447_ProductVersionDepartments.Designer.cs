﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.DevLogix.Sqlite;

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    [DbContext(typeof(DevLogixSqliteDbContext))]
    [Migration("20221208035447_ProductVersionDepartments")]
    partial class ProductVersionDepartments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("RingSoft.DbLookup.AdvancedFind.AdvancedFind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<bool?>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("FromFormula")
                        .HasColumnType("ntext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("RedAlert")
                        .HasColumnType("integer");

                    b.Property<byte?>("RefreshCondition")
                        .HasColumnType("smallint");

                    b.Property<byte?>("RefreshRate")
                        .HasColumnType("smallint");

                    b.Property<int?>("RefreshValue")
                        .HasColumnType("integer");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

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
                        .HasColumnType("nvarchar");

                    b.Property<byte>("DecimalFormatType")
                        .HasColumnType("smallint");

                    b.Property<byte>("FieldDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar");

                    b.Property<double>("PercentWidth")
                        .HasColumnType("numeric");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

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
                        .HasColumnType("smallint");

                    b.Property<string>("FieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Formula")
                        .HasColumnType("ntext");

                    b.Property<byte>("FormulaDataType")
                        .HasColumnType("smallint");

                    b.Property<string>("FormulaDisplayValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("LeftParentheses")
                        .HasColumnType("smallint");

                    b.Property<byte>("Operand")
                        .HasColumnType("smallint");

                    b.Property<string>("Path")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryFieldName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryTableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<byte>("RightParentheses")
                        .HasColumnType("smallint");

                    b.Property<int?>("SearchForAdvancedFindId")
                        .HasColumnType("integer");

                    b.Property<string>("SearchForValue")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("TableName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("AdvancedFindId", "FilterId");

                    b.HasIndex("SearchForAdvancedFindId");

                    b.ToTable("AdvancedFindFilters");
                });

            modelBuilder.Entity("RingSoft.DbLookup.RecordLocking.RecordLock", b =>
                {
                    b.Property<string>("Table")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PrimaryKey")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("LockDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("User")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("Table", "PrimaryKey");

                    b.ToTable("RecordLocks");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("ErrorFailStatusId")
                        .HasColumnType("integer");

                    b.Property<int?>("ErrorFixStatusId")
                        .HasColumnType("integer");

                    b.Property<int?>("ErrorPassStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("FailText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("FixText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<string>("PassText")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

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
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ErrorPriorities");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ErrorStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("ErrorStatuses");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Rights")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ProductVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVersions");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.QualityAssurance.ProductVersionDepartment", b =>
                {
                    b.Property<int>("VersionId")
                        .HasColumnType("integer");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ReleaseDateTime")
                        .HasColumnType("datetime");

                    b.HasKey("VersionId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("ProductVersionDepartments");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.SystemMaster", b =>
                {
                    b.Property<string>("OrganizationName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("OrganizationName");

                    b.ToTable("SystemMaster");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Notes")
                        .HasColumnType("ntext");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Rights")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar");

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

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.QualityAssurance.ProductVersionDepartment", b =>
                {
                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.Department", "Department")
                        .WithMany("ProductVersionDepartments")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RingSoft.DevLogix.DataAccess.Model.ProductVersion", "ProductVersion")
                        .WithMany("ProductVersionDepartments")
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("ProductVersion");
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
                    b.Navigation("ProductVersionDepartments");

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

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.ProductVersion", b =>
                {
                    b.Navigation("ProductVersionDepartments");
                });

            modelBuilder.Entity("RingSoft.DevLogix.DataAccess.Model.User", b =>
                {
                    b.Navigation("UserGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
