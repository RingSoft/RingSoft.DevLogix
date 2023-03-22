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

        public decimal? ContractCost { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public DateTime OriginalDeadline { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal? SundayMinutes { get; set; }

        public decimal? MondayMinutes { get; set; }
        
        public decimal? TuesdayMinutes { get; set; }

        public decimal? WednesdayMinutes { get; set; }

        public decimal? ThursdayMinutes { get; set; }

        public decimal? FridayMinutes { get; set; }

        public decimal? SaturdayMinutes { get; set; }

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
