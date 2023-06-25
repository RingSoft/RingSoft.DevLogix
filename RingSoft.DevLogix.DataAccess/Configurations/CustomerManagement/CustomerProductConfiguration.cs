using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class CustomerProductConfiguration : IEntityTypeConfiguration<CustomerProduct>
    {
        public void Configure(EntityTypeBuilder<CustomerProduct> builder)
        {
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ExpirationDate).HasColumnType(DbConstants.DateColumnType);

            builder.HasKey(p => new { p.CustomerId, p.ProductId });

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.CustomerProducts)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.CustomerProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
