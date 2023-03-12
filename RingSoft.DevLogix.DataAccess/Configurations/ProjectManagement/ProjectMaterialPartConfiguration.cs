using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectMaterialPartConfiguration : IEntityTypeConfiguration<ProjectMaterialPart>
    {
        public void Configure(EntityTypeBuilder<ProjectMaterialPart> builder)
        {
            builder.Property(p => p.ProjectMaterialId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DetailId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.LineType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RowId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ParentRowId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CommentCrLf).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.MaterialPartId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Quantity).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.ProjectMaterialId, p.DetailId });

            builder.HasOne(p => p.ProjectMaterial)
                .WithMany(p => p.Parts)
                .HasForeignKey(p => p.ProjectMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.MaterialPart)
                .WithMany(p => p.ProjectMaterialParts)
                .HasForeignKey(p => p.MaterialPartId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
