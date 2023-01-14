using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ErrorDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ErrorStatusId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorPriorityId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FoundVersionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FixedVersionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.AssignedDeveloperId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.AssignedTesterId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FixedDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PassedDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.Resolution).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.FoundByUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FixedByByUserId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasOne(p => p.ErrorStatus)
                .WithMany(p => p.Errors)
                .HasForeignKey(p => p.ErrorStatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Errors)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.ErrorPriority)
                .WithMany(p => p.Errors)
                .HasForeignKey(p => p.ErrorPriorityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.FoundVersion)
                .WithMany(p => p.FoundErrors)
                .HasForeignKey(p => p.FoundVersionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.FixedVersion)
                .WithMany(p => p.FixedErrors)
                .HasForeignKey(p => p.FixedVersionId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.AssignedDeveloper)
                .WithMany(p => p.AssignedDeveloperErrors)
                .HasForeignKey(p => p.AssignedDeveloperId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.AssignedTester)
                .WithMany(p => p.AssignedTesterErrors)
                .HasForeignKey(p => p.AssignedTesterId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(p => p.FoundByUser)
                .WithMany(p => p.FoundByUserErrors)
                .HasForeignKey(p => p.FoundByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.FixedByUser)
                .WithMany(p => p.FixedByUserErrors)
                .HasForeignKey(p => p.FixedByByUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}
