using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DevLogix.DataAccess.Model
{
    [Table("ErrorPriorities")]
    public class ErrorPriority
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
