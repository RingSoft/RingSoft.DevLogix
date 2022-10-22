using System.Linq;
using RingSoft.DataEntryControls.Engine.Annotations;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        User GetUser(int userId);

        bool SaveUser(User user);

        bool DeleteUser(int userId);
    }
    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public User GetUser(int userId)
        {
            var context = AppGlobals.GetNewDbContext();
            return  context.Users.FirstOrDefault(p => p.Id == userId);
        }

        public bool SaveUser(User user)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.Users, user,
                $"Saving User '{user.Name}.'");
        }

        public bool DeleteUser(int userId)
        {
            var context = AppGlobals.GetNewDbContext();
            var user = context.Users.FirstOrDefault(f => f.Id == userId);
            return user != null && context.DbContext.DeleteEntity(context.Users, user,
                $"Deleting User '{user.Name}'");
            ;
        }
    }
}
