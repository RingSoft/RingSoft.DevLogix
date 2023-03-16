using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class SystemPreferencesConfiguration : IEntityTypeConfiguration<SystemPreferences>
    {
        public void Configure(EntityTypeBuilder<SystemPreferences> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
        }
    }
}
