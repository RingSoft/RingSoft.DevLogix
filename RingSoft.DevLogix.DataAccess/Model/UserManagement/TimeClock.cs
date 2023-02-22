﻿using System;
using System.ComponentModel.DataAnnotations;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class TimeClock
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PunchInDate { get; set; }

        public DateTime? PunchOutDate { get; set; }

        public decimal? MinutesSpent { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int? ErrorId { get; set; }

        public virtual Error Error { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string? Notes { get; set; }

        public bool AreDatesEdited { get; set; }
    }
}
