using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.QualityAssurance
{
    public class ErrorUser
    {
        [Required]
        public int ErrorId { get; set; }

        public virtual Error Error { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [Required]
        [DefaultValue(0)]
        public double Cost { get; set; }
    }
}
