using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement
{
    public class OrderLookup
    {
        public string OrderId { get; set; }

        public string Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public double Total { get; set; }
    }

    public class OrderDetailLookup
    {
        public string OrderId { get; set; }

        public string Product { get; set; }
    }
}
