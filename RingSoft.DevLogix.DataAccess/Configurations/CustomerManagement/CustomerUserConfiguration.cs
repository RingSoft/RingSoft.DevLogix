using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class CustomerUserConfiguration : IEntityTypeConfiguration<CustomerUser>
    {
        public void Configure(EntityTypeBuilder<CustomerUser> builder)
        {
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.CustomerId, p.UserId });

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.CustomerUsers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
