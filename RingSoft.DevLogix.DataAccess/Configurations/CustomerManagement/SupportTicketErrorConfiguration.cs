using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class SupportTicketErrorConfiguration : IEntityTypeConfiguration<SupportTicketError>
    {
        public void Configure(EntityTypeBuilder<SupportTicketError> builder)
        {
            builder.Property(p => p.SupportTicketId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.SupportTicketId, p.ErrorId });

            builder.HasOne(p => p.SupportTicket)
                .WithMany(p => p.Errors)
                .HasForeignKey(p => p.SupportTicketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Error)
                .WithMany(p => p.SupportTickets)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
