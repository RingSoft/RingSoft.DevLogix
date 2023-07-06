using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class SupportTicketUser
    {
        [Required]
        public int SupportTicketId { get; set; }

        public virtual SupportTicket SupportTicket { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public double MinutesSpent { get; set; }

        [Required]
        public double Cost { get; set; }
    }
}
