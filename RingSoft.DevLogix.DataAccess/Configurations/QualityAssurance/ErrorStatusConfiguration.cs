using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ErrorStatusConfiguration : IEntityTypeConfiguration<ErrorStatus>
    {
        public void Configure(EntityTypeBuilder<ErrorStatus> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
