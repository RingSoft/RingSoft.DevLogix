using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDevLogixDbContext : IAdvancedFindDbContextEfCore
    {
        DbContext DbContext { get; }

        DbSet<SystemMaster> SystemMaster { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<UsersGroup> UsersGroups { get; set; }

        void SetLookupContext(DevLogixLookupContext lookupContext);
    }
}
