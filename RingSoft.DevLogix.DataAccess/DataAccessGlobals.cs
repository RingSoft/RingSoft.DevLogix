using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Configurations;

namespace RingSoft.DevLogix.DataAccess
{
    public class DataAccessGlobals
    {
        public static void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorStatusConfiguration());

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);
        }
    }
}
