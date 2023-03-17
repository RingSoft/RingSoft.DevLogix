using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.UserManagement
{
    public class UserTimeOffConfiguration : IEntityTypeConfiguration<UserTimeOff>
    {
        public void Configure(EntityTypeBuilder<UserTimeOff> builder)
        {
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RowId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.StartDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.EndDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);

            builder.HasKey(p => new {p.UserId, p.RowId});

            builder.HasOne(p => p.User)
                .WithMany(p => p.UserTimeOff)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
