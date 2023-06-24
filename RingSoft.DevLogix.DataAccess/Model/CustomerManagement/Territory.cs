using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class Territory
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int SalespersonId { get; set; }

        public virtual User Salesperson { get; set; }

        public ICollection<Customer> Customers  { get; set; }

        public Territory()
        {
            Customers = new HashSet<Customer>();
        }
    }
}
