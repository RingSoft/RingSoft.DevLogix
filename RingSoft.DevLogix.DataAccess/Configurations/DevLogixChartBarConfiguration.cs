using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class DevLogixChartBarConfiguration : IEntityTypeConfiguration<DevLogixChartBar>
    {
        public void Configure(EntityTypeBuilder<DevLogixChartBar> builder)
        {
            builder.Property(p => p.ChartId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.BarId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.AdvancedFindId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.ChartId, p.BarId });

            builder.HasOne(p => p.Chart)
                .WithMany(p => p.ChartBars)
                .HasForeignKey(p => p.ChartId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
