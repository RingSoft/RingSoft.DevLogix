using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class TestingOutline
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; }

        public virtual User AssignedToUser { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public decimal PercentComplete { get; set; }

        public string? Notes { get; set; }
    }
}
