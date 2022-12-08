using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class ProductVersionDepartment
    {
        [Required]
        public int VersionId { get; set; }

        public virtual ProductVersion ProductVersion { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        [Required]
        public DateTime ReleaseDateTime { get; set; }
    }
}
