using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            builder.Property(p => p.ProjectId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.IsStandard).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.SundayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MondayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TuesdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.WednesdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ThursdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.FridayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SaturdayMinutes).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.ProjectId, p.UserId });

            builder.HasOne(p => p.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
