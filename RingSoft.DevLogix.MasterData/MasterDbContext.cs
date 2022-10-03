using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;

namespace RingSoft.DevLogix.MasterData
{
    public class MasterDbContext : DbContext
    {
        public static string ProgramDataFolder
        {
            get
            {
#if DEBUG
                return RingSoftAppGlobals.AssemblyDirectory;
#else
                return
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\HomeLogix\\";
#endif
            }
        }

        public static string MasterFilePath => $"{ProgramDataFolder}{MasterFileName}";

        public const string MasterFileName = "MasterDb.sqlite";
        public const string DemoDataFileName = "DemoData.sqlite";

    }
}
