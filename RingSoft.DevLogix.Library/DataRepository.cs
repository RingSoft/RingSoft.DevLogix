using System.Linq;
using RingSoft.DataEntryControls.Engine.Annotations;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();
    }
    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

    }
}
