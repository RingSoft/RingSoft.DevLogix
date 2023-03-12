using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectMaterialPart
    {
        [Required]
        public int ProjectMaterialId { get; set; }

        public virtual ProjectMaterial ProjectMaterial { get; set; }

        [Required]
        public int DetailId { get; set; }

        [Required]
        public byte LineType { get; set; }

        [Required]
        [MaxLength(50)]
        public string RowId { get; set; }

        [MaxLength(50)]
        public string? ParentRowId { get; set; }

        public bool? CommentCrLf { get; set; }

        public int? MaterialPartId { get; set; }

        public virtual MaterialPart MaterialPart { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Cost { get; set; }
    }
}
