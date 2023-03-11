using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectMaterialConfiguration : IEntityTypeConfiguration<ProjectMaterial>
    {
        public void Configure(EntityTypeBuilder<ProjectMaterial> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ProjectId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.IsCostEdited).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.ActualCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);

            builder.HasOne(p => p.Project)
                .WithMany(p => p.ProjectMaterialParts)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
