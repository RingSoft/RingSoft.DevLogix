using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class MaterialPart
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public string? Comment { get; set; }

        public ICollection<ProjectMaterialPart> ProjectMaterialParts { get; set; }

        public MaterialPart()
        {
            ProjectMaterialParts = new HashSet<ProjectMaterialPart>();
        }
    }
}
