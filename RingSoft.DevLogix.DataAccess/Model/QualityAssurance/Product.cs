using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public string? Notes { get; set; }

        public ICollection<ProductVersion> Versions { get; set; }

        public ICollection<Error> Errors { get; set; }

        public Product()
        {
            Versions = new HashSet<ProductVersion>();
            Errors = new HashSet<Error>();
        }
    }
}
