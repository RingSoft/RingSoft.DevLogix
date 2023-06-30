using RingSoft.DevLogix.DataAccess;
using System.Linq;
using RingSoft.App.Library;
using RingSoft.DbLookup.DataProcessor;
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

        public DbLookup.IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            var platform = DbPlatforms.Sqlite;

            if (dataProcessor is SqlServerDataProcessor)
            {
                platform = DbPlatforms.SqlServer;
            }
            return AppGlobals.GetNewDbContext(platform);
        }

        DbLookup.IDbContext DbLookup.IDataRepository.GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }
    }
}
