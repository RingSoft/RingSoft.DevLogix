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
}
