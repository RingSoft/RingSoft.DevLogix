using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectTaskLaborPartConfiguration : IEntityTypeConfiguration<ProjectTaskLaborPart>
    {
        public void Configure(EntityTypeBuilder<ProjectTaskLaborPart> builder)
        {
            builder.Property(p => p.ProjectTaskId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DetailId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.LineType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RowId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ParentRowId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CommentCrLf).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.LaborPartId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Quantity).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesCost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.ProjectTaskId, p.DetailId });

            builder.HasOne(p => p.ProjectTask)
                .WithMany(p => p.LaborParts)
                .HasForeignKey(p => p.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.LaborPart)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(p => p.LaborPartId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
