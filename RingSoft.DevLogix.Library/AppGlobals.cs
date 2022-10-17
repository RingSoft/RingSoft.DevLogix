using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.MasterData;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.SqlServer;
using DbPlatforms = RingSoft.App.Library.DbPlatforms;

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

        public static DbPlatforms DbPlatform { get; private set; }

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

            DbPlatform = DbPlatforms.SqlServer;
            //DbPlatform = DbPlatforms.Sqlite;
            var context = GetNewDbContext();
            context.SetLookupContext(LookupContext);

            
            LookupContext.SqliteDataProcessor.FilePath = "C:\\Temp\\";
            LookupContext.SqliteDataProcessor.FileName = "Temp.sqlite";

            LookupContext.SqlServerDataProcessor.Server = "localhost\\SQLEXPRESS";
            LookupContext.SqlServerDataProcessor.Database = "RingSoftDevLogixTemp";
            LookupContext.SqlServerDataProcessor.SecurityType = SecurityTypes.WindowsAuthentication;

            LookupContext.Initialize(context, DbPlatform);

            context.DbContext.Database.Migrate();

            var selectQuery = new SelectQuery(LookupContext.AdvancedFinds.TableName);
            LookupContext.SqliteDataProcessor.GetData(selectQuery, false);

        }

        public static IDevLogixDbContext GetNewDbContext(DbPlatforms? platform = null)
        {
            if (platform == null)
            {
                platform = DbPlatform;
            }
            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    var sqliteResult = new DevLogixSqliteDbContext();
                    sqliteResult.SetLookupContext(AppGlobals.LookupContext);
                    return sqliteResult;
                case DbPlatforms.SqlServer:
                    var result = new DevLogixSqlServerDbContext();
                    result.SetLookupContext(AppGlobals.LookupContext);
                    return result;
                //case DbPlatforms.MySql:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
