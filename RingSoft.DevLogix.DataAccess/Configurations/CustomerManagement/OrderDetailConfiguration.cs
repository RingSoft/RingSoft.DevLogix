using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(p => p.OrderId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DetailId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Quantity).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.UnitPrice).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ExtendedPrice).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Discount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.OrderId, p.DetailId });

            builder.HasOne(p => p.Order)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.OrderDetailProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
