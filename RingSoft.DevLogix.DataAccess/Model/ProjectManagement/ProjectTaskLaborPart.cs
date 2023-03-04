using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectTaskLaborPart
    {
        [Required]
        public int ProjectTaskId { get; set; }

        public virtual ProjectTask ProjectTask { get; set; }

        [Required]
        public int DetailId { get; set; }

        public int? LaborPartId { get; set; }

        public virtual LaborPart LaborPart { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        public int? Quantity { get; set; }

        [Required]
        public decimal MinutesCost { get; set; }
    }
}
