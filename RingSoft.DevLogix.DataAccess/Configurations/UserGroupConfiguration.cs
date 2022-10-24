using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UsersGroup>
    {
        public void Configure(EntityTypeBuilder<UsersGroup> builder)
        {
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.GroupId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new {p.UserId, p.GroupId});

            builder.HasOne(p => p.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Group)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
