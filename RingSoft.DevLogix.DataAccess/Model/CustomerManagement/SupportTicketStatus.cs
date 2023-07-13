using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class SupportTicketStatus
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<SupportTicket> Tickets { get; set; }

        public SupportTicketStatus()
        {
            Tickets = new HashSet<SupportTicket>();
        }
    }
}
