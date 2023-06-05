﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.MasterData;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.SqlServer;
using DbPlatforms = RingSoft.App.Library.DbPlatforms;

namespace RingSoft.DevLogix.Library
{
    public class DialogInput
    {
        public bool DialogResult { get; set; }
    }

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

        public static bool UnitTesting { get; set; }

        public static Organization LoggedInOrganization { get; set; }

        public static DbPlatforms DbPlatform { get; set; }

        public static MainViewModel MainViewModel { get; set; }

        public static AppRights Rights { get; set; }

        public static User LoggedInUser { get; set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "DevLogix";
            RingSoftAppGlobals.AppCopyright = "©2023 by Peter Ringering";
            RingSoftAppGlobals.PathToDownloadUpgrade = MasterDbContext.ProgramDataFolder;
            RingSoftAppGlobals.AppGuid = "96ac9aa5-65af-43ca-8cb8-6e35a2f12570";
            RingSoftAppGlobals.AppVersion = 139;
            SystemGlobals.ProgramDataFolder = MasterDbContext.ProgramDataFolder;
        }

        public static void Initialize()
        {
            DataRepository ??= new DataRepository();
            SystemGlobals.DataRepository = DataRepository;
            SystemGlobals.ConvertAllDatesToUniversalTime = true;

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            LookupContext = new DevLogixLookupContext();
            SystemGlobals.LookupContext = LookupContext;
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;
            

            if (!UnitTesting)
            {
                AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to the Master Database."));

                MasterDbContext.ConnectToMaster();

                var defaultOrganization = MasterDbContext.GetDefaultOrganization();
                if (defaultOrganization != null)
                {
                    if (LoginToOrganization(defaultOrganization).IsNullOrEmpty())
                        LoggedInOrganization = defaultOrganization;
                }
            }
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

        public static string LoginToOrganization(Organization organization)
        {
            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Migrating the {organization.Name} Database."));
            DbPlatform = (DbPlatforms)organization.Platform;
            LookupContext.SetProcessor(DbPlatform);
            var context = GetNewDbContext();
            context.SetLookupContext(LookupContext);
            LoadDataProcessor(organization);
            SystemMaster systemMaster = null;
            DbContext migrateContext = context.DbContext;
            var migrateResult = string.Empty;

            switch ((DbPlatforms)organization.Platform)
            {
                case DbPlatforms.Sqlite:
                    if (!organization.FilePath.EndsWith('\\'))
                        organization.FilePath += "\\";

                    LookupContext.Initialize(context, DbPlatform);

                    var newFile = !File.Exists($"{organization.FilePath}{organization.FileName}");

                    if (newFile == false)
                    {
                        try
                        {
                            var file = new FileInfo($"{organization.FilePath}{organization.FileName}");
                            file.IsReadOnly = false;
                        }
                        catch (Exception e)
                        {
                            var message = $"Can't access Organization file path: {organization.FilePath}.  You must run this program as administrator.";
                            return message;
                        }
                        migrateResult = MigrateContext(migrateContext);
                        if (!migrateResult.IsNullOrEmpty())
                        {
                            return migrateResult;
                        }

                        systemMaster = context.SystemMaster.FirstOrDefault();
                        if (systemMaster != null) organization.Name = systemMaster.OrganizationName;
                    }
                    else
                    {
                        migrateResult = MigrateContext(migrateContext);
                        if (!migrateResult.IsNullOrEmpty())
                        {
                            return migrateResult;
                        }

                        context.DbContext.Database.Migrate();
                        systemMaster = new SystemMaster { OrganizationName = organization.Name };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");

                    }

                    break;
                case DbPlatforms.SqlServer:

                    var databases = RingSoftAppGlobals.GetSqlServerDatabaseList(organization.Server);
                    {
                        if (databases.IndexOf(organization.Database) < 0)
                        {
                            migrateResult = MigrateContext(migrateContext);
                            if (!migrateResult.IsNullOrEmpty())
                            {
                                return migrateResult;
                            }
                        }
                        else
                        {
                            var migrate = true;

                            migrate = AllowMigrate();

                            if (migrate)
                            {
                                migrateResult = MigrateContext(migrateContext);
                                if (!migrateResult.IsNullOrEmpty())
                                {
                                    return migrateResult;
                                }
                            }
                        }
                    }

                    if (databases.IndexOf(organization.Database) >= 0)
                    {
                        systemMaster = context.SystemMaster.FirstOrDefault();
                        if (systemMaster != null) organization.Name = systemMaster.OrganizationName;
                    }

                    if (systemMaster == null)
                    {
                        systemMaster = new SystemMaster { OrganizationName = organization.Name };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");
                    }
                    LookupContext.Initialize(context, DbPlatform);

                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Rights = new AppRights();

            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Connecting to the {organization.Name} Database."));
            var selectQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            LookupContext.DataProcessor.GetData(selectQuery, false);
            DataAccessGlobals.SetupSysPrefs();
            return string.Empty;
        }

        public static bool AllowMigrate(DbDataProcessor processor = null)
        {
            if (processor == null)
            {
                processor = LookupContext.DataProcessor;
            }
            bool migrate;
            var sqlValue = Guid.NewGuid().ToString();
            var field = LookupContext.SystemMaster.GetFieldDefinition(p => p.OrganizationName);
            var insertStatement = new InsertDataStatement(LookupContext.SystemMaster);
            var sqlData = new SqlData(field.FieldName, sqlValue, field.ValueType);
            insertStatement.AddSqlData(sqlData);
            var sqls = new List<string>();
            sqls.Add(processor.SqlGenerator.GenerateInsertSqlStatement(
                insertStatement));
            var orgQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            orgQuery.AddWhereItem(field.FieldName, Conditions.Equals, sqlValue);

            sqls.Add(processor.SqlGenerator.GenerateDeleteStatement(orgQuery));

            var orgResult = processor.ExecuteSqls(sqls, true, false, false);
            migrate = orgResult.ResultCode == GetDataResultCodes.Success;
            return migrate;
        }

        public static string MigrateContext(DbContext migrateContext)
        {
            try
            {
                migrateContext.Database.Migrate();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }

        public static void LoadDataProcessor(Organization organization, DbPlatforms? platform = null)
        {
            if (platform == null)
            {
                platform = (DbPlatforms)organization.Platform;
            }

            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    LookupContext.SqliteDataProcessor.FilePath = organization.FilePath;
                    LookupContext.SqliteDataProcessor.FileName = organization.FileName;
                    break;
                case DbPlatforms.SqlServer:
                    LookupContext.SqlServerDataProcessor.Server = organization.Server;
                    LookupContext.SqlServerDataProcessor.Database = organization.Database;
                    LookupContext.SqlServerDataProcessor.SecurityType = (SecurityTypes)organization.AuthenticationType;
                    LookupContext.SqlServerDataProcessor.UserName = organization.Username;
                    LookupContext.SqlServerDataProcessor.Password = organization.Password.DecryptDatabasePassword();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static string MakeTimeSpent(decimal minutesSpent)
        {
            var negative = false;
            if (minutesSpent < 0)
            {
                minutesSpent = Math.Abs(minutesSpent);
                negative = true;
            }

            var timeSpent = "0 Minutes";
            var hoursSpent = (decimal)0;
            var daysSpent = (decimal)0;
            var numFormatString = GblMethods.GetNumFormat(2, false);

            if (minutesSpent >= 0)
            {
                if (minutesSpent > 60)
                {
                    hoursSpent = Math.Round(minutesSpent / 60, 2);
                    timeSpent = $"{FormatValue(hoursSpent, DecimalFieldTypes.Decimal)} Hours";
                }
                else
                {
                    minutesSpent = Math.Round(minutesSpent, 2);
                    timeSpent = $"{FormatValue(minutesSpent, DecimalFieldTypes.Decimal)} Minutes";
                }
            }

            if (hoursSpent > 24)
            {
                daysSpent = Math.Round(hoursSpent / 24, 2);
                timeSpent = $"{FormatValue(daysSpent, DecimalFieldTypes.Decimal)} Days";
            }

            if (negative)
                timeSpent = $"-{timeSpent}";

            return timeSpent;
        }
        private static string FormatValue(decimal value, DecimalFieldTypes decimalFieldType)
        {
            var result = string.Empty;
            var numFormat = GblMethods.GetNumFormat(2, false);
            result = GblMethods.FormatValue(FieldDataTypes.Decimal, value.ToString()
                , numFormat);

            return result;
        }

        public static void CalculateProject(Project project, List<ProjectUser> users)
        {
            project.MinutesSpent = users.Sum(p => p.MinutesSpent);
            project.Cost = users.Sum(p => p.Cost);
        }

        public static void CalculateError(Error error, List<ErrorUser> users)
        {
            error.MinutesSpent = users.Sum(p => p.MinutesSpent);
            error.Cost = users.Sum(p => p.Cost);

        }

        public static void CalculateTestingOutline(TestingOutline testingOutline, List<TestingOutlineCost> costs)
        {
            testingOutline.MinutesSpent = costs.Sum(p => p.TimeSpent);
            testingOutline.TotalCost = costs.Sum(p => p.Cost);
        }

        public static void ClockInUser(RingSoft.DevLogix.DataAccess.IDbContext context, User user)
        {
            if (user != null && user.ClockOutReason != (byte)ClockOutReasons.ClockedIn)
            {
                user.ClockOutReason = (byte)ClockOutReasons.ClockedIn;
                user.ClockDate = DateTime.Now.ToUniversalTime();
                context.SaveEntity(user, "Clocking In");
            }
        }

        public static decimal CalcPercentComplete(IEnumerable<TestingOutlineDetails> details)
        {
            var result = (decimal)0;
            var completeDetails = details
                .Where(p => p.IsComplete);

            var completedCount = completeDetails.Count();
            var detailsCount = details.Count();
            if (detailsCount > 0)
            {
                result = (decimal)completedCount / detailsCount;
            }

            return result;
        }

        public static AutoFillValue GetVersionForUser(Product product)
        {
            AutoFillValue result = null;
            var context = AppGlobals.DataRepository.GetDataContext();
            var productTable = context.GetTable<ProductVersionDepartment>();
            var productVersions = productTable.Include(p => p.ProductVersion)
                .OrderByDescending(p => p.ReleaseDateTime)
                .Where(p => p.ProductVersion.ProductId == product.Id);

            if (productVersions != null)
            {
                var productVersion = productVersions.FirstOrDefault();

                if (productVersion != null)
                {
                    if (AppGlobals.LoggedInUser != null)
                    {
                        var departmentId = AppGlobals.LoggedInUser.DepartmentId;
                        productVersion = productVersions.FirstOrDefault(p => p.DepartmentId == departmentId);
                    }

                    if (productVersion == null)
                    {
                        productVersion = productVersions.FirstOrDefault();
                    }

                    result = productVersion.ProductVersion.GetAutoFillValue();
                }
            }
            return result;
        }

    }
}
