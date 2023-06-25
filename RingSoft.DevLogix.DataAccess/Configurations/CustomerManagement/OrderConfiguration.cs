using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.OrderId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.OrderDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ShippedDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.CompanyName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ContactName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ContactTitle).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Address).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.City).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Region).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PostalCode).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Country).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.SubTotal).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TotalDiscount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Freight).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Total).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
