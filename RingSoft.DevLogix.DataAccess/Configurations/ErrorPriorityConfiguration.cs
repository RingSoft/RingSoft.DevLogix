using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ErrorPriorityConfiguration : IEntityTypeConfiguration<ErrorPriority>
    {
        public void Configure(EntityTypeBuilder<ErrorPriority> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Level).HasColumnType(DbConstants.IntegerColumnType);
        }
    }
}
