using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.DataAccess.Configurations.QualityAssurance
{
    public class TestingOutlineConfiguration : IEntityTypeConfiguration<TestingOutline>
    {
        public void Configure(EntityTypeBuilder<TestingOutline> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.ProductId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.CreatedByUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.AssignedToUserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DueDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PercentComplete).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.TestingOutlines)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CreatedByUser)
                .WithMany(p => p.CreatedTestingOutlines)
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.AssignedToUser)
                .WithMany(p => p.AssignedTestingOutlines)
                .HasForeignKey(p => p.AssignedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
