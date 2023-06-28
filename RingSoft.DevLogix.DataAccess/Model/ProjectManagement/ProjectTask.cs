using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.ProjectManagement
{
    public class ProjectTask
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public double MinutesCost { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool MinutesEdited { get; set; }

        [DefaultValue(0)]
        public double EstimatedCost { get; set; }

        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [DefaultValue(0)]
        public double Cost { get; set; }

        [Required]
        public double PercentComplete { get; set; }

        [Required]
        [DefaultValue(0)]
        public double HourlyRate { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<TimeClock> TimeClocks { get; set; }
        public virtual ICollection<ProjectTaskLaborPart> LaborParts { get; set; }
        public virtual ICollection<ProjectTaskDependency> SourceDependencies { get; set; }
        public virtual ICollection<ProjectTaskDependency> Dependencies { get; set; }

        public ProjectTask()
        {
            TimeClocks = new HashSet<TimeClock>();
            LaborParts = new HashSet<ProjectTaskLaborPart>();
            SourceDependencies = new HashSet<ProjectTaskDependency>();
            Dependencies = new HashSet<ProjectTaskDependency>();
        }
    }
}
