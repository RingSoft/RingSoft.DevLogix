using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.DataAccess.Configurations.QualityAssurance
{
    public class ErrorUserConfiguration : IEntityTypeConfiguration<ErrorUser>
    {
        public void Configure(EntityTypeBuilder<ErrorUser> builder)
        {
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.ErrorId, p.UserId });

            builder.HasOne(p => p.Error)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.ErrorUsers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
