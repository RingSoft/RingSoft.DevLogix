using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class TestingTemplate
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? BaseTemplateId { get; set; }

        public virtual TestingTemplate BaseTemplate { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<TestingTemplate> ChildTemplates { get; set; }

        public virtual ICollection<TestingTemplateItem> Items { get; set; }

        public virtual ICollection<TestingOutlineDetails> TestingOutlineDetails { get; set; }

        public TestingTemplate()
        {
            ChildTemplates = new HashSet<TestingTemplate>();
            Items = new HashSet<TestingTemplateItem>();
            TestingOutlineDetails = new HashSet<TestingOutlineDetails>();
        }
    }
}
