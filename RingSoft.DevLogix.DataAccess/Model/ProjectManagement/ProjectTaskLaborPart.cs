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

        [Required]
        public byte LineType { get; set; }

        [Required]
        [MaxLength(50)]
        public string  RowId { get; set; }

        [MaxLength(50)]
        public string? ParentRowId { get; set; }

        public bool? CommentCrLf { get; set; }

        public int? LaborPartId { get; set; }

        public virtual LaborPart LaborPart { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        public decimal? Quantity { get; set; }

        [Required]
        public decimal MinutesCost { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Complete { get; set; }
    }
}
