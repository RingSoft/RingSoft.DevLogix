using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class Department
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public int? ErrorFixStatusId { get; set; }

        public virtual ErrorStatus ErrorFixStatus { get; set; }

        public int? ErrorPassStatusId { get; set; }

        public virtual ErrorStatus ErrorPassStatus { get; set; }

        public int? ErrorFailStatusId { get; set; }

        public virtual ErrorStatus ErrorFailStatus { get; set; }


        [MaxLength(50)]
        public string? FixText { get; set; }

        [Required]
        [MaxLength(50)]
        public string PassText { get; set; }

        [Required]
        [MaxLength(50)]
        public string FailText { get; set; }
    }
}
