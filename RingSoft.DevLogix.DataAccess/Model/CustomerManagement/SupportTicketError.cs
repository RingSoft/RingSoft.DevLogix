using System.ComponentModel.DataAnnotations;

namespace RingSoft.DevLogix.DataAccess.Model.CustomerManagement
{
    public class SupportTicketError
    {
        [Required]
        public int SupportTicketId { get; set; }

        public virtual SupportTicket SupportTicket { get; set; }

        [Required]
        public int ErrorId { get; set; }

        public virtual Error Error { get; set; }
    }
}
