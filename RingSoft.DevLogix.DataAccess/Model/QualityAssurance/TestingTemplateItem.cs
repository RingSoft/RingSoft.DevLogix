using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class TestingTemplateItem
    {
        [Required]
        public int TestingTemplateId { get; set; }

        public virtual TestingTemplate TestingTemplate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
    }
}
