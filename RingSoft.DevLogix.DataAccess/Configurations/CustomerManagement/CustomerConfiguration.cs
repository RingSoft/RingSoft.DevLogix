using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.CompanyName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ContactName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ContactTitle).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.AssignedUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Address).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.City).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Region).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PostalCode).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Country).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Phone).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TimeZoneId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SupportMinutesPurchased).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SupportMinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SupportCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SalesMinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SalesCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.EmailAddress).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.WebAddress).HasColumnType(DbConstants.StringColumnType);

            builder.HasOne(p => p.AssignedUser)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder .HasOne(p => p.TimeZone)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.TimeZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
