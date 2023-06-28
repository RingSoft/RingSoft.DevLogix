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

        public int ManagerId { get; set; }

        public virtual User Manager { get; set; }

        public double? ContractCost { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public DateTime OriginalDeadline { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public double? SundayMinutes { get; set; }

        public double? MondayMinutes { get; set; }
        
        public double? TuesdayMinutes { get; set; }

        public double? WednesdayMinutes { get; set; }

        public double? ThursdayMinutes { get; set; }

        public double? FridayMinutes { get; set; }

        public double? SaturdayMinutes { get; set; }

        public string? Notes { get; set; }

        [DefaultValue(true)]
        public bool IsBillable { get; set; }

        [DefaultValue(0)]
        public double EstimatedMinutes { get; set; }

        [DefaultValue(0)]
        public double EstimatedCost { get; set; }

        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [DefaultValue(0)]
        public double Cost { get; set; }

        public DateTime? StartDateTime { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        public virtual ICollection<ProjectMaterial> ProjectMaterialParts { get; set; }

        public Project()
        {
            ProjectUsers = new HashSet<ProjectUser>();
            ProjectTasks = new HashSet<ProjectTask>();
            ProjectMaterialParts = new HashSet<ProjectMaterial>();
        }
    }
}
