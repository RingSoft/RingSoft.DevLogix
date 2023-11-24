using System.Linq;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();
    }

    public class DataRepository : SystemDataRepositoryEfCore, IDataRepository
    {
        public IDbContext GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }

        public override DbLookup.IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            var platform = DbPlatforms.Sqlite;

            if (dataProcessor is SqlServerDataProcessor)
            {
                platform = DbPlatforms.SqlServer;
            }
            return AppGlobals.GetNewDbContext(platform);
        }
    }
}
