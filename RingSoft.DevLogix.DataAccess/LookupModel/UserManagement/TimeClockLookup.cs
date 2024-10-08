﻿using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class TimeClockLookup
    {
        public string Name { get; set; }
        public string UserName { get; set; }

        public DateTime PunchInDate { get; set; }

        public double MinutesSpent { get; set; }

        public string ProjectTask { get; set; }
    }
}
