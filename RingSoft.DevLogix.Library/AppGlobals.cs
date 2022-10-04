using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DbPlatform;
using RingSoft.DevLogix.MasterData;

namespace RingSoft.DevLogix.Library
{
    public class AppProgressArgs
    {
        public string ProgressText { get; }

        public static IDataRepository DataRepository { get; set; }

        public AppProgressArgs(string progressText)
        {
            ProgressText = progressText;
        }
    }
    public class AppGlobals
    {
        public static DevLogixLookupContext LookupContext { get; private set; }

        public static IDataRepository DataRepository { get; set; }

        public static DbPlatform.DbPlatform DbPlatform { get; private set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "DevLogix";
            RingSoftAppGlobals.AppCopyright = "©2022 by Peter Ringering";
            RingSoftAppGlobals.AppVersion = "0.80.00";
        }

        public static void Initialize()
        {
            DataRepository ??= new DataRepository();

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            LookupContext = new DevLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

            DbPlatform = new DbPlatform.DbPlatform();

            LookupContext.Initialize(DbPlatform.GetNewDbContext(LookupContext), DevLogix.DbPlatform.DbPlatform.DevLogixPlatform);

            LookupContext.SqliteDataProcessor.FilePath = "C:\\Temp\\";
            LookupContext.SqliteDataProcessor.FileName = "Temp.sqlite";

            var context = DbPlatform.GetNewDbContext();
            context.DbContext.Database.Migrate();

            var selectQuery = new SelectQuery(LookupContext.AdvancedFinds.TableName);
            LookupContext.SqliteDataProcessor.GetData(selectQuery, false);

        }

    }
}
