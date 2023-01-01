using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.InstallerFileName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ArchivePath).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
