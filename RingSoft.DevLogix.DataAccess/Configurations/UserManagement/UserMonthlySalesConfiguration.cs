using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.UserManagement
{
    public class UserMonthlySalesConfiguration : IEntityTypeConfiguration<UserMonthlySales>
    {
        public void Configure(EntityTypeBuilder<UserMonthlySales> builder)
        {
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MonthEnding).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Quota).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TotalSales).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Difference).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.UserId, p.MonthEnding });

            builder.HasOne(p => p.User)
                .WithMany(p => p.UserMonthlySales)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
