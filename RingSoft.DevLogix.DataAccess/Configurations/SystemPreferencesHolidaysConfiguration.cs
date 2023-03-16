using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class SystemPreferencesHolidaysConfiguration : IEntityTypeConfiguration<SystemPreferencesHolidays>
    {
        public void Configure(EntityTypeBuilder<SystemPreferencesHolidays> builder)
        {
            builder.Property(p => p.SystemPreferencesId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Date).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);

            builder.HasKey(p => new { p.SystemPreferencesId, p.Date });

            builder.HasOne(p => p.SystemPreferences)
                .WithMany(p => p.Holidays)
                .HasForeignKey(p => p.SystemPreferencesId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
