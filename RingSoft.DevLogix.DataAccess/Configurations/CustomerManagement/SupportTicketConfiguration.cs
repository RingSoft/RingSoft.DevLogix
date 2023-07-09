using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TicketId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CreateDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.CloseDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.PhoneNumber).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CreateUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.AssignedToUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ContactName).HasColumnType(DbConstants.StringColumnType);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CreateUser)
                .WithMany(p => p.CreateTickets)
                .HasForeignKey(p => p.CreateUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.AssignedToUser)
                .WithMany(p => p.AssignedTickets)
                .HasForeignKey(p => p.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

        }
    }
}
