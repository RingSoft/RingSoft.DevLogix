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
            builder.Property(p => p.Address).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.City).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Region).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PostalCode).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Country).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Phone).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TimeZoneId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TerritoryId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SupportMinutesPurchased).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SupportMinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SupportCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.EmailAddress).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.WebAddress).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TotalSales).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TotalCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MinutesCost).HasColumnType(DbConstants.DecimalColumnType);

            builder .HasOne(p => p.TimeZone)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.TimeZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Territory)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.TerritoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
