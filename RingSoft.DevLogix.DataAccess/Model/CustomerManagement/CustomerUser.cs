using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class CustomerUser
    {
        [Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public double MinutesSpent { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}
