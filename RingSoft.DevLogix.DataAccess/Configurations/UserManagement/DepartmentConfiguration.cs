using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ErrorFixStatusId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorPassStatusId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorFailStatusId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FixText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PassText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FailText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.FtpAddress).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FtpUsername).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FtpPassword).HasColumnType(DbConstants.StringColumnType);

            builder.HasOne(p => p.ErrorFixStatus)
                .WithMany(p => p.FixedDepartments)
                .HasForeignKey(p => p.ErrorFixStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(p => p.ErrorPassStatus)
                .WithMany(p => p.PassedDepartments)
                .HasForeignKey(p => p.ErrorPassStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(p => p.ErrorFailStatus)
                .WithMany(p => p.FailedDepartments)
                .HasForeignKey(p => p.ErrorFailStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
