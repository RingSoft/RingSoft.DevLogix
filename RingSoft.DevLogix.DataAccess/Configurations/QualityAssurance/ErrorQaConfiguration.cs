using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess.Configurations
{
    public class ErrorQaConfiguration : IEntityTypeConfiguration<ErrorQa>
    {
        public void Configure(EntityTypeBuilder<ErrorQa> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TesterId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasOne(p => p.Error)
                .WithMany(p => p.Testers)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Tester)
                .WithMany(p => p.ErrorTesters)
                .HasForeignKey(p => p.TesterId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.NewErrorStatus)
                .WithMany(p => p.ErrorTesters)
                .HasForeignKey(p => p.NewStatusId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
