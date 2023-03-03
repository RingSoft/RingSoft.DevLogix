using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Utilities.Collections;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

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

        public DateTime? ClockDate { get; set; }

        public int? SupervisorId { get; set; }

        public virtual User Supervisor { get; set; }

        public virtual DevLogixChart DefaultChart { get; set; }

        [DefaultValue(0)]
        public decimal HourlyRate { get; set; }

        [DefaultValue(0)]
        public decimal BillableProjectsMinutesSpent { get; set; }

        [DefaultValue(0)]
        public decimal NonBillableProjectsMinutesSpent { get; set; }

        [DefaultValue(0)]
        public decimal ErrorsMinutesSpent { get; set; }

        public virtual ICollection<UsersGroup>  UserGroups { get; set; }
        public virtual ICollection<Error> FoundByUserErrors { get; set; }
        public virtual ICollection<Error> AssignedDeveloperErrors { get; set; }
        public virtual  ICollection<Error> AssignedTesterErrors { get; set; }
        public virtual ICollection<ErrorDeveloper> ErrorDevelopers { get; set; }
        public virtual ICollection<ErrorQa> ErrorTesters { get; set; }
        public virtual ICollection<ErrorUser> ErrorUsers { get; set; }
        public virtual ICollection<TimeClock> TimeClocks { get; set; }
        public virtual ICollection<User> Underlings { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public User()
        {
            UserGroups = new HashSet<UsersGroup>();
            AssignedDeveloperErrors = new HashSet<Error>();
            AssignedTesterErrors = new HashSet<Error>();
            ErrorDevelopers = new HashSet<ErrorDeveloper>();
            ErrorTesters = new HashSet<ErrorQa>();
            TimeClocks = new HashSet<TimeClock>();
            Underlings = new HashSet<User>();
            ProjectUsers = new HashSet<ProjectUser>();
            ErrorUsers = new HashSet<ErrorUser>();
            ProjectTasks = new HashSet<ProjectTask>();
        }
    }
}
