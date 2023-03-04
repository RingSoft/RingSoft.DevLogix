﻿using System.Collections.Generic;
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
        public decimal MinutesCost { get; set; }

        [Required]
        public decimal PercentComplete { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<ProjectTaskLaborPart> LaborParts { get; set; }

        public ProjectTask()
        {
            LaborParts = new HashSet<ProjectTaskLaborPart>();
        }
    }
}
