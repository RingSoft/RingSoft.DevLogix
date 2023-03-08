using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ProjectId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.EstimatedCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.PercentComplete).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.HourlyRate).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);

            builder.HasOne(p => p.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
