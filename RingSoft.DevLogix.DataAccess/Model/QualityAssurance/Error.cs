﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.DataAccess.Model
{
    public class Error
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErrorId { get; set; }

        [Required]
        public DateTime ErrorDate { get; set; }

        [Required]
        public int ErrorStatusId { get; set; }

        public virtual ErrorStatus ErrorStatus { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int ErrorPriorityId { get; set; }

        public virtual ErrorPriority ErrorPriority { get; set; }

        [Required]
        public int FoundVersionId { get; set; }

        public int? FoundByUserId { get; set; }

        public virtual User FoundByUser { get; set; }

        public virtual ProductVersion FoundVersion { get; set; }

        public int? FixedVersionId { get; set; }

        public virtual ProductVersion FixedVersion { get; set; }

        public int? AssignedDeveloperId { get; set; }

        public virtual User AssignedDeveloper { get; set; }

        public int? AssignedTesterId { get; set;  }

        public virtual User AssignedTester { get; set; }

        public string Description { get; set; }

        public string? Resolution { get; set; }

        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        [DefaultValue(0)]
        public double Cost { get; set; }

        public int? TestingOutlineId { get; set; }

        public virtual TestingOutline TestingOutline { get; set; }

        public virtual ICollection<ErrorDeveloper> Developers { get; set; }

        public virtual ICollection<ErrorQa> Testers { get; set; }

        public virtual ICollection<TimeClock> TimeClocks { get; set; }

        public virtual ICollection<ErrorUser> Users { get; set; }

        public virtual ICollection<SupportTicketError> SupportTickets { get; set; }

        public Error()
        {
            Developers = new HashSet<ErrorDeveloper>();
            Testers = new HashSet<ErrorQa>();
            TimeClocks = new HashSet<TimeClock>();
            Users = new HashSet<ErrorUser>();
            SupportTickets = new HashSet<SupportTicketError>();
        }

        public override string ToString()
        {
            return ErrorId;
        }
    }
}
