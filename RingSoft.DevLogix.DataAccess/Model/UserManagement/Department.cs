using System.Collections.Generic;
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

        [MaxLength(50)]
        public string? PassText { get; set; }

        [MaxLength(50)]
        public string? FailText { get; set; }

        [MaxLength(50)]
        public string? FtpAddress { get; set; }

        [MaxLength(50)]
        public string? FtpUsername { get; set; }

        [MaxLength(200)]
        public string? FtpPassword { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        public virtual ICollection<Product> CreateVersionProducts { get; set; }
        public virtual ICollection<Product> ArchiveVersionProducts { get; set; }

        public Department()
        {
            Users = new HashSet<User>();
            ProductVersionDepartments = new HashSet<ProductVersionDepartment>();
            CreateVersionProducts = new HashSet<Product>();
            ArchiveVersionProducts = new HashSet<Product>();
        }
    }
}
