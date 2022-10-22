using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public enum UserTypes
    {
        [Description("Developer")]
        Developer = 0,
        [Description("Quality Assurance")]
        QualityAssurance = 1,
        [Description("Technical Support")]
        TechSupport = 2,
        [Description("Sales")]
        Sales = 3,
        [Description("Miscellaneous")]
        Miscellaneous = 4,
    }
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public byte Type { get; set; }

        [MaxLength(255)]
        public string? Password { get; set; }

        [MaxLength(255)]
        public string? Rights { get; set; }
    }
}
