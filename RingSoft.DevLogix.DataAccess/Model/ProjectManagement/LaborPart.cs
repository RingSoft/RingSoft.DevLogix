using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class LaborPart
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public double MinutesCost { get; set; }

        public string? Comment { get; set; }

        public virtual ICollection<ProjectTaskLaborPart> ProjectTasks { get; set; }

        public LaborPart()
        {
            ProjectTasks = new HashSet<ProjectTaskLaborPart>();
        }
    }
}
