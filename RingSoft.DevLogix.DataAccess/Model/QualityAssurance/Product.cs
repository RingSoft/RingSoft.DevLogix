using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

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

        [MaxLength(50)]
        public string? InstallerFileName { get; set; }

        [MaxLength(50)]
        public string? ArchivePath { get; set; }

        [MaxLength(50)]
        public string? AppGuid { get; set; }

        public int CreateDepartmentId { get; set; }

        public virtual Department CreateDepartment { get; set; }

        public int? ArchiveDepartmentId { get; set; }

        public virtual Department ArchiveDepartment { get; set; }

        public string? Notes { get; set; }

        public double? Price { get; set; }

        public double? Revenue { get; set; }

        public double? Cost { get; set; }

        public ICollection<ProductVersion> Versions { get; set; }

        public ICollection<Error> Errors { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<TestingOutline> TestingOutlines { get; set; }

        public ICollection<CustomerProduct> CustomerProducts { get; set; }

        public ICollection<OrderDetail> OrderDetailProducts { get; set; }

        public Product()
        {
            Versions = new HashSet<ProductVersion>();
            Errors = new HashSet<Error>();
            Projects = new HashSet<Project>();
            TestingOutlines = new HashSet<TestingOutline>();
            CustomerProducts = new HashSet<CustomerProduct>();
            OrderDetailProducts = new HashSet<OrderDetail>();
        }
    }
}
