using System;
using System.ComponentModel.DataAnnotations;

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

        public DateTime? CloseDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required]
        public int CreateUserId { get; set; }

        public virtual User CreateUser { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int? AssignedToUserId { get; set; }

        public virtual User AssignedToUser { get; set; }

        public string? Notes { get; set; }

        public double? MinutesSpent { get; set; }

        public double? Cost { get; set; }
    }
}
