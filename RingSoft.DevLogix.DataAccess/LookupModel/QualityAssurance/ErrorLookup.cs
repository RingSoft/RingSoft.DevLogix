using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class ErrorLookup
    {
        public string ErrorId { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public DateTime Date { get; set; }

        public string User { get; set; }
    }
}
