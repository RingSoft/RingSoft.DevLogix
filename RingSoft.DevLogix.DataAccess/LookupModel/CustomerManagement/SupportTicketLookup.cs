using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement
{
    public class SupportTicketLookup
    {
        public string TicketId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Customer { get; set; }

        public string Product { get; set; }
    }

    public class SupportTicketUserLookup
    {
        public string TicketId { get; set; }

        public string UserName { get; set; }
    }
}
