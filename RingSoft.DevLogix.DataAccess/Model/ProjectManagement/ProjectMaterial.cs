using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectMaterial
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsCostEdited { get; set; }

        [Required]
        public decimal ActualCost { get; set; }

        public string? Notes { get; set; }

        public ICollection<ProjectMaterialPart> Parts { get; set; }

        public ICollection<ProjectMaterialHistory> History { get; set; }

        public ProjectMaterial()
        {
            Parts = new HashSet<ProjectMaterialPart>();
            History = new HashSet<ProjectMaterialHistory>();
        }
    }
}
