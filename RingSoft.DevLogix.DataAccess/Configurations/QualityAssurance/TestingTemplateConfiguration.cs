using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.DataAccess.Configurations.QualityAssurance
{
    public class TestingTemplateConfiguration : IEntityTypeConfiguration<TestingTemplate>
    {
        public void Configure(EntityTypeBuilder<TestingTemplate> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.BaseTemplateId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);

            builder.HasOne(p => p.BaseTemplate)
                .WithMany(p => p.ChildTemplates)
                .HasForeignKey(p => p.BaseTemplateId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }

    public class TestingTemplateItemConfiguration : IEntityTypeConfiguration<TestingTemplateItem>
    {
        public void Configure(EntityTypeBuilder<TestingTemplateItem> builder)
        {
            builder.Property(p => p.TestingTemplateId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);

            builder.HasKey(p => new { p.TestingTemplateId, p.Description });

            builder.HasOne(p => p.TestingTemplate)
                .WithMany(p => p.Items)
                .HasForeignKey(p => p.TestingTemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
