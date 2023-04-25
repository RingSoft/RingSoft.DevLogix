using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.UserManagement
{
    public class UserTrackerConfiguration : IEntityTypeConfiguration<UserTracker>
    {
        public void Configure(EntityTypeBuilder<UserTracker> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.RefreshInterval).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RefreshType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RedMinutes).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.YellowMinutes).HasColumnType(DbConstants.DecimalColumnType);
        }
    }
}
