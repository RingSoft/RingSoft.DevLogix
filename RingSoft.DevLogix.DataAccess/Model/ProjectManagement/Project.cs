using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class Project
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public DateTime OriginalDeadline { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal? SundayHours { get; set; }

        public decimal? MondayHours { get; set; }
        
        public decimal? TuesdayHours { get; set; }

        public decimal? WednesdayHours { get; set; }

        public decimal? ThursdayHours { get; set; }

        public decimal? FridayHours { get; set; }

        public decimal? SaturdayHours { get; set; }

        public string? Notes { get; set; }

        [DefaultValue(true)]
        public bool IsBillable { get; set; }

        [DefaultValue(0)]
        public decimal EstimatedMinutes { get; set; }

        [DefaultValue(0)]
        public decimal EstimatedCost { get; set; }

        [DefaultValue(0)]
        public decimal MinutesSpent { get; set; }

        [DefaultValue(0)]
        public decimal Cost { get; set; }

        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public Project()
        {
            TimeClocks = new HashSet<TimeClock>();
            ProjectUsers = new HashSet<ProjectUser>();
            ProjectTasks = new HashSet<ProjectTask>();
        }
    }
}
