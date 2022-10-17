using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDevLogixDbContext : IAdvancedFindDbContextEfCore
    {
        DbContext DbContext { get; }

        void SetLookupContext(DevLogixLookupContext lookupContext);
    }
}
