using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class TestingTemplateItem
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int TestingTemplateId { get; set; }

        public virtual TestingTemplate TestingTemplate { get; set; }
    }
}
