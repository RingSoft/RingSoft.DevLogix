using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations.QualityAssurance
{
    public class ProductVersionDepartmentConfiguration : IEntityTypeConfiguration<ProductVersionDepartment>
    {
        public void Configure(EntityTypeBuilder<ProductVersionDepartment> builder)
        {
            builder.Property(p => p.VersionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DepartmentId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ReleaseDateTime).HasColumnType(DbConstants.DateColumnType);

            builder.HasKey(p => new { p.VersionId, p.DepartmentId });

            builder.HasOne(p => p.ProductVersion)
                .WithMany(p => p.ProductVersionDepartments)
                .HasForeignKey(p => p.VersionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Department)
                .WithMany(p => p.ProductVersionDepartments)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
