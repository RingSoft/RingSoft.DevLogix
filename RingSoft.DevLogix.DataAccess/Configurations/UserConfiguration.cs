using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Type).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.Password).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Rights).HasColumnType(DbConstants.StringColumnType);
            builder.HasKey(p => p.Id);
        }
    }
}
