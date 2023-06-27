using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

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

        public DateTime? ArchiveDateTime { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public DateTime? VersionDate { get; set; }

        public virtual ICollection<ProductVersionDepartment> ProductVersionDepartments { get; set; }

        public virtual ICollection<Error> FoundErrors { get; set; }

        public virtual ICollection<Error> FixedErrors { get; set; }

        public virtual ICollection<TestingOutlineDetails> TestingOutlineDetails { get; set; }

        public ProductVersion()
        {
            ProductVersionDepartments = new HashSet<ProductVersionDepartment>();
            FoundErrors = new HashSet<Error>();
            FixedErrors = new HashSet<Error>();
            TestingOutlineDetails = new HashSet<TestingOutlineDetails>();
        }
    }
}
