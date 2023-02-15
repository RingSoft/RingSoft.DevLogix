using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Utilities.Collections;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        [MaxLength(255)]
        public string? Password { get; set; }

        [MaxLength(255)]
        public string? Rights { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? PhoneNumber { get; set; }

        public string? Notes { get; set; }

        public int? DefaultChartId { get; set; }

        public virtual DevLogixChart DefaultChart { get; set; }

        public virtual ICollection<UsersGroup>  UserGroups { get; set; }
        public virtual ICollection<Error> FoundByUserErrors { get; set; }
        public virtual ICollection<Error> AssignedDeveloperErrors { get; set; }
        public virtual  ICollection<Error> AssignedTesterErrors { get; set; }
        public virtual ICollection<ErrorDeveloper> ErrorDevelopers { get; set; }
        public virtual ICollection<ErrorQa> ErrorTesters { get; set; }
        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public User()
        {
            UserGroups = new HashSet<UsersGroup>();
            AssignedDeveloperErrors = new HashSet<Error>();
            AssignedTesterErrors = new HashSet<Error>();
            ErrorDevelopers = new HashSet<ErrorDeveloper>();
            ErrorTesters = new HashSet<ErrorQa>();
            TimeClocks = new HashSet<TimeClock>();
        }
    }
}
