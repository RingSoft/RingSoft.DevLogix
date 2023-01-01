using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ProductVersionConfiguration : IEntityTypeConfiguration<ProductVersion>
    {
        public void Configure(EntityTypeBuilder<ProductVersion> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.ArchiveDateTime).HasColumnType(DbConstants.DateColumnType);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Versions)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
