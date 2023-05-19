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
            builder.Property(p => p.MinutesSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TotalCost).HasColumnType(DbConstants.DecimalColumnType);
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

    public class TestingOutlineDetailsConfiguration : IEntityTypeConfiguration<TestingOutlineDetails>
    {
        public void Configure(EntityTypeBuilder<TestingOutlineDetails> builder)
        {
            builder.Property(p => p.TestingOutlineId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.DetailId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Step).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.IsComplete).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.CompletedVersionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TestingTemplateId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.TestingOutlineId, p.DetailId });

            builder.HasOne(p => p.TestingOutline)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.TestingOutlineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CompletedVersion)
                .WithMany(p => p.TestingOutlineDetails)
                .HasForeignKey(p => p.CompletedVersionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TestingTemplate)
                .WithMany(p => p.TestingOutlineDetails)
                .HasForeignKey(p => p.TestingTemplateId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TestingOutlineTemplateConfiguration : IEntityTypeConfiguration<TestingOutlineTemplate>
    {
        public void Configure(EntityTypeBuilder<TestingOutlineTemplate> builder)
        {
            builder.Property(p => p.TestingOutlineId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TestingTemplateId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.TestingOutlineId, p.TestingTemplateId });

            builder.HasOne(p => p.TestingOutline)
                .WithMany(p => p.Templates)
                .HasForeignKey(p => p.TestingOutlineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TestingTemplate)
                .WithMany(p => p.TestingOutlineTemplates)
                .HasForeignKey(p => p.TestingTemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TestingOutlineCostConfiguration : IEntityTypeConfiguration<TestingOutlineCost>
    {
        public void Configure(EntityTypeBuilder<TestingOutlineCost> builder)
        {
            builder.Property(p => p.TestingOutlineId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.UserId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TimeSpent).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.TestingOutlineId, p.UserId });

            builder.HasOne(p => p.TestingOutline)
                .WithMany(p => p.Costs)
                .HasForeignKey(p => p.TestingOutlineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.User)
                .WithMany(p => p.TestingOutlineCosts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
