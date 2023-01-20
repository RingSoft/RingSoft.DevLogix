using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class TimeClockLookup
    {
        public string UserName { get; set; }

        public DateTime PunchInDate { get; set; }

        public decimal MinutesSpent { get; set; }
    }
}
