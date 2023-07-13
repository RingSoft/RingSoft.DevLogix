using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class CustomerStatus
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public CustomerStatus()
        {
            Customers = new HashSet<Customer>();
        }
    }
}
