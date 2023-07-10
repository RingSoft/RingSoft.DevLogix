using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement
{
    public class SupportTicketLookup
    {
        public string TicketId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Customer { get; set; }

        public double MinutesSpent { get; set; }
    }

    public class SupportTicketUserLookup
    {
        public string TicketId { get; set; }

        public string UserName { get; set; }
    }

    public class SupportTicketErrorLookup
    {
        public string TicketId { get; set; }

        public string ErrorId { get; set; }
    }
}
