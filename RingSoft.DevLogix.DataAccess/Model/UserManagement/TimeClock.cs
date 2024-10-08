﻿using System;
using System.ComponentModel.DataAnnotations;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class TimeClock
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime PunchInDate { get; set; }

        public DateTime? PunchOutDate { get; set; }

        public double? MinutesSpent { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int? ErrorId { get; set; }

        public virtual Error Error { get; set; }

        public int? ProjectTaskId { get; set; }

        public virtual ProjectTask ProjectTask { get; set; }

        public int? TestingOutlineId { get; set; }

        public virtual TestingOutline TestingOutline { get; set; }

        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int? SupportTicketId { get; set; }

        public virtual SupportTicket SupportTicket { get; set; }

        public string? Notes { get; set; }

        public bool AreDatesEdited { get; set; }

        [MaxLength(50)]
        public string? ClockOutReason { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
