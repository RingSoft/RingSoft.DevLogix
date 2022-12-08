using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class ProductVersion
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<ProductVersionDepartment> ProductVersionDepartments { get; set; }

        public ProductVersion()
        {
            ProductVersionDepartments = new HashSet<ProductVersionDepartment>();
        }
    }
}
