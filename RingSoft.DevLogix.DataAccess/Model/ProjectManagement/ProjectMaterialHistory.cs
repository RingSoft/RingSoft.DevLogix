using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectMaterialHistory
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectMaterialId { get; set; }

        public virtual ProjectMaterial ProjectMaterial { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Cost { get; set;  }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
