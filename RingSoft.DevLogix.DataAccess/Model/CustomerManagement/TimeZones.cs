using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class TimeZone
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int HourToGMT { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public TimeZone()
        {
            Customers = new HashSet<Customer>();
        }
    }
}
