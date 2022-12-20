using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DevLogix.DataAccess.Model
{
    [Table("ErrorStatuses")]
    public class ErrorStatus
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<Department> FixedDepartments { get; set; }

        public ICollection<Department> PassedDepartments { get; set; }

        public ICollection<Department> FailedDepartments { get; set; }

        public ICollection<Error> Errors { get; set; }

        public ErrorStatus()
        {
            FixedDepartments = new HashSet<Department>();
            PassedDepartments = new HashSet<Department>();
            FailedDepartments = new HashSet<Department>();
            Errors = new HashSet<Error>();
        }
    }
}
