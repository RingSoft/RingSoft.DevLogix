using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement
{
    public class CustomerLookup
    {
        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class TimeZoneLookup
    {
        public string Name { get; set; }
    }

    public class TerritoryLookup
    {
        public string Name { get; set; }

        public string Salesperson { get; set; }
    }

    public class CustomerProductLookup
    {
        public string Customer { get; set; }

        public string Product { get; set; }

        public DateTime ExpirationDate { get; set; }
    }

    public class CustomerComputerLookup
    {
        public string Name { get; set; }
    }

    public class CustomerUserLookup
    {
        public string CustomerName { get; set; }

        public string UserName { get; set; }
    }

}
