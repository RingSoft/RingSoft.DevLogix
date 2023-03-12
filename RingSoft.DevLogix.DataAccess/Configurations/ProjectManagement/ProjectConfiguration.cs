﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ManagerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ContractCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Deadline).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.OriginalDeadline).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SundayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MondayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TuesdayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.WednesdayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ThursdayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.FridayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SaturdayHours).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.IsBillable).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.EstimatedMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.EstimatedCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.Manager)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}