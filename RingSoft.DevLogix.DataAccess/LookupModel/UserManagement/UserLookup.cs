using System;
using System.Xml;

namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class UserLookup
    {
        public string UserName { get; set; }

        public string Department { get; set; }
    }

    public class UserMonthlySalesLookup
    {
        public string UserName { get; set; }

        public DateTime MonthEnding { get; set; }

        public double TotalSales { get; set; }
    }
}
