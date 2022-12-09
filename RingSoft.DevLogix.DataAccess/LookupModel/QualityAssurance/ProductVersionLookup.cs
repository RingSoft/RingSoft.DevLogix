using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel
{
    public class ProductVersionLookup
    {
        public string Product { get; set; }

        public string Description { get; set; }

        public DateTime VersionDate { get; set; }

        public string MaxDepartment { get; set; }
    }
}
