using RingSoft.DevLogix.DataAccess;
using System.Linq;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository : DbLookup.IDataRepository
    {
        IDbContext GetDataContext();
    }

    public class DataRepository : IDataRepository
    {
        public IDbContext GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }

        DbLookup.IDbContext DbLookup.IDataRepository.GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }
    }
}
