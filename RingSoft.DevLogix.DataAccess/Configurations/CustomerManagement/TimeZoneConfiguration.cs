using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class TimeZoneConfiguration : IEntityTypeConfiguration<TimeZone>
    {
        public void Configure(EntityTypeBuilder<TimeZone> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.HourToGMT).HasColumnType(DbConstants.IntegerColumnType);
        }
    }
}
