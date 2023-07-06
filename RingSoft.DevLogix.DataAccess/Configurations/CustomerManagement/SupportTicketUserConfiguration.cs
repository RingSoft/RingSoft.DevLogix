using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class SupportTicketUserConfiguration : IEntityTypeConfiguration<SupportTicketUser>
    {
        public void Configure(EntityTypeBuilder<SupportTicketUser> builder)
        {
            builder.Property(p => p.SupportTicketId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.SupportTicketId, p.UserId });

            builder.HasOne(p => p.SupportTicket)
                .WithMany(p => p.SupportTicketUsers)
                .HasForeignKey(p => p.SupportTicketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.SupportTicketUsers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
