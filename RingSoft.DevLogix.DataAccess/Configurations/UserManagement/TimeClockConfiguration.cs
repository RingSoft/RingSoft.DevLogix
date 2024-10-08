﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class TimeClockConfiguration : IEntityTypeConfiguration<TimeClock>
    {
        public void Configure(EntityTypeBuilder<TimeClock> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PunchInDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PunchOutDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProjectTaskId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.AreDatesEdited).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ClockOutReason).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.SupportTicketId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasOne(p => p.Error)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.User)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.ProjectTask)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.ProjectTaskId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.TestingOutline)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.TestingOutlineId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.SupportTicket)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.SupportTicketId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
