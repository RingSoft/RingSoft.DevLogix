using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DevLogix.DataAccess
{
    public class DataAccessGlobals
    {
        public static void ConfigureModel(ModelBuilder modelBuilder)
        {
            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);
        }
    }
}
