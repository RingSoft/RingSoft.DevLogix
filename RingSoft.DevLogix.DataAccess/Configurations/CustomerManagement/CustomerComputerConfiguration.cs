using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.DataAccess.Configurations.CustomerManagement
{
    public class CustomerComputerConfiguration :IEntityTypeConfiguration<CustomerComputer>
    {
        public void Configure(EntityTypeBuilder<CustomerComputer> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.OperatingSystem).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Speed).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ScreenResolution).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.RamSize).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.HardDriveSize).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.HardDriveFree).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.InternetSpeed).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DatabasePlatform).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.Printer).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.CustomerComputers)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
