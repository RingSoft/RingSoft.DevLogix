using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ManagerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ContractCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Deadline).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.OriginalDeadline).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SundayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MondayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TuesdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.WednesdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ThursdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.FridayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.SaturdayMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.IsBillable).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.EstimatedMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.EstimatedCost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.StartDateTime).HasColumnType(DbConstants.DateColumnType);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.Manager)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
