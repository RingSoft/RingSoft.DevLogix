using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectMaterialHistoryConfiguration : IEntityTypeConfiguration<ProjectMaterialHistory>
    {
        public void Configure(EntityTypeBuilder<ProjectMaterialHistory> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProjectMaterialId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Date).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Quantity).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasOne(p => p.ProjectMaterial)
                .WithMany(p => p.History)
                .HasForeignKey(p => p.ProjectMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.ProjectMaterialHistory)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
