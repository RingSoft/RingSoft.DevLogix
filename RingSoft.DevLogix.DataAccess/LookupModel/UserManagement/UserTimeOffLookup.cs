using System;

namespace RingSoft.DevLogix.DataAccess.LookupModel.UserManagement
{
    public class UserTimeOffLookup
    {
        public string UserName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
