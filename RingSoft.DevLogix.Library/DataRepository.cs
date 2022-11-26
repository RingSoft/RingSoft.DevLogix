using RingSoft.DevLogix.DataAccess;
using System.Linq;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class;
    }

    public class DataRepository : IDataRepository
    {
        public IDbContext GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            var context = AppGlobals.GetNewDbContext();
            var dbSet = context.DbContext.Set<TEntity>();
            return dbSet;
        }

    }
}
