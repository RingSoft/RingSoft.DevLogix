using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ErrorDeveloperConfiguration : IEntityTypeConfiguration<ErrorDeveloper>
    {
        public void Configure(EntityTypeBuilder<ErrorDeveloper> builder)
        {
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DeveloperId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.ErrorId, p.DeveloperId });

            builder.HasOne(p => p.Error)
                .WithMany(p => p.Developers)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Developer)
                .WithMany(p => p.ErrorDevelopers)
                .HasForeignKey(p => p.DeveloperId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
