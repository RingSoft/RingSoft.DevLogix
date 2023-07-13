using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class SupportTicket
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TicketId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public int? StatusId { get; set; }

        public virtual SupportTicketStatus Status { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? ContactName { get; set; }

        [Required]
        public int CreateUserId { get; set; }

        public virtual User CreateUser { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int? AssignedToUserId { get; set; }

        public virtual User AssignedToUser { get; set; }

        public string? Notes { get; set; }

        [DefaultValue(0)]
        public double MinutesSpent { get; set; }

        public double? Cost { get; set; }

        public ICollection<TimeClock> TimeClocks { get; set; }

        public ICollection<SupportTicketUser> SupportTicketUsers { get; set; }
        public ICollection<SupportTicketError> Errors { get; set; }

        public SupportTicket()
        {
            TimeClocks = new HashSet<TimeClock>();
            SupportTicketUsers = new HashSet<SupportTicketUser>();
            Errors = new HashSet<SupportTicketError>();
        }

        public override string ToString()
        {
            return TicketId;
        }
    }
}
