using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectTaskDependenciesConfiguration : IEntityTypeConfiguration<ProjectTaskDependency>
    {
        public void Configure(EntityTypeBuilder<ProjectTaskDependency> builder)
        {
            builder.Property(p => p.ProjectTaskId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DependsOnProjectTaskId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.ProjectTaskId, p.DependsOnProjectTaskId });

            builder.HasOne(p => p.ProjectTask)
                .WithMany(p => p.SourceDependencies)
                .HasForeignKey(p => p.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.DependsOnProjectTask)
                .WithMany(p => p.Dependencies)
                .HasForeignKey(p => p.DependsOnProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
