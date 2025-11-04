using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.MasterData;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.SqlServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using DbPlatforms = RingSoft.App.Library.DbPlatforms;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

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

        public static User LoggedInUser { get; set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "DevLogix";
            RingSoftAppGlobals.AppCopyright = $"©{DateTime.Today.Year} by Peter Ringering";
            RingSoftAppGlobals.PathToDownloadUpgrade = MasterDbContext.ProgramDataFolder;
            RingSoftAppGlobals.AppGuid = "96ac9aa5-65af-43ca-8cb8-6e35a2f12570";
            RingSoftAppGlobals.AppVersion = 396;
            SystemGlobals.ProgramDataFolder = MasterDbContext.ProgramDataFolder;
        }

        public static void Initialize()
        {
            DataRepository ??= new DataRepository();
            SystemGlobals.ConvertAllDatesToUniversalTime = true;

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            LookupContext = new DevLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

            SystemGlobals.ItemRightsFactory = new DevLogixRightsFactory();

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
            IDevLogixDbContext? context = GetNewDbContext();
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
                        systemMaster = new SystemMaster
                        {
                            OrganizationName = organization.Name,
                            AppGuid = RingSoftAppGlobals.AppGuid,
                        };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");

                    }

                    break;
                case DbPlatforms.SqlServer:

                    var databases = RingSoftAppGlobals.GetSqlServerDatabaseList(organization.Server);
                    {
                        var migrate = true;
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
                            var context1 = SystemGlobals.DataRepository.GetDataContext();
                            var sysMasterTable = context1.GetTable<SystemMaster>();
                            try
                            {
                                var sysMasterRecord = sysMasterTable.FirstOrDefault();
                                var appGuid = sysMasterRecord.AppGuid;
                            }
                            catch (Exception e)
                            {
                                MigrateContext(migrateContext);
                            }
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
                        systemMaster = new SystemMaster
                        {
                            OrganizationName = organization.Name,
                            AppGuid = RingSoftAppGlobals.AppGuid,
                        };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");
                    }
                    LookupContext.Initialize(context, DbPlatform);

                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SystemGlobals.Rights = new AppRights(new DevLogixRights());

            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Connecting to the {organization.Name} Database."));
            //var selectQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            //LookupContext.DataProcessor.GetData(selectQuery, false);
            DataAccessGlobals.SetupSysPrefs();
            return string.Empty;
        }

        public static bool AllowMigrate(DbDataProcessor processor = null)
        {
            //if (processor == null)
            //{
            //    processor = LookupContext.DataProcessor;
            //}

            var migrate = true;
            var context = DataRepository.GetDataContext();
            var table = context.GetTable<SystemMaster>();
            var sysMaster = new SystemMaster()
            {
                OrganizationName = Guid.NewGuid().ToString(),
                AppGuid = RingSoftAppGlobals.AppGuid,
            };

            var sysMasters = new List<SystemMaster>();
            sysMasters.Add(sysMaster);

            context.AddRange(sysMasters);
            migrate = context.Commit("Checking System Master");
            if (migrate)
            {
                context.RemoveRange(sysMasters);
                migrate = context.Commit("Checking Migrate");
            }

            //var sqlValue = Guid.NewGuid().ToString();
            //var field = LookupContext.SystemMaster.GetFieldDefinition(p => p.OrganizationName);
            //var insertStatement = new InsertDataStatement(LookupContext.SystemMaster);
            //var sqlData = new SqlData(field.FieldName, sqlValue, field.ValueType);
            //insertStatement.AddSqlData(sqlData);
            //var sqls = new List<string>();
            //sqls.Add(processor.SqlGenerator.GenerateInsertSqlStatement(
            //    insertStatement));
            //var orgQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            //orgQuery.AddWhereItem(field.FieldName, Conditions.Equals, sqlValue);

            //sqls.Add(processor.SqlGenerator.GenerateDeleteStatement(orgQuery));

            //var orgResult = processor.ExecuteSqls(sqls, true, false, false);
            //migrate = orgResult.ResultCode == GetDataResultCodes.Success;
            return migrate;
        }

        public static string MigrateContext(DbContext migrateContext)
        {
            try
            {
                migrateContext.Database.Migrate();
                ConvertData();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }

        private static void ConvertData()
        {
            var context = DataRepository.GetDataContext();
            var table = context.GetTable<TimeClock>();

            if (table.Any(p => p.Name == null))
            {
                var timeClocks = table.Where(p => p.Name == null);
                foreach (var timeClock in timeClocks)
                {
                    timeClock.Name = $"T-{timeClock.Id}";
                    context.SaveNoCommitEntity(timeClock, "Saving TimeClock");
                }

                context.Commit("Committing TimeClocks");
            }

            var prodVersionsTable = context.GetTable<ProductVersion>();
            var prodVersionQuery = prodVersionsTable
                .Include(p => p.ProductVersionDepartments)
                .Where(p => p.DepartmentId == null
                && p.ProductVersionDepartments.Any());
            if (prodVersionQuery.Any())
            {
                foreach (var productVersion in prodVersionQuery)
                {
                    var lastDepartment = productVersion.ProductVersionDepartments
                        .OrderByDescending(p => p.ReleaseDateTime).FirstOrDefault();
                    if (lastDepartment != null)
                    {
                        productVersion.DepartmentId = lastDepartment.DepartmentId;
                        productVersion.VersionDate = lastDepartment.ReleaseDateTime;
                        context.SaveNoCommitEntity(productVersion, "Updating Product Version");
                    }
                }

                context.Commit("Finalizing Product Versions");
            }
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
        public static string MakeTimeSpent(double minutesSpent)
        {
            var negative = false;
            if (minutesSpent < 0)
            {
                minutesSpent = Math.Abs(minutesSpent);
                negative = true;
            }

            var timeSpent = "0 Minutes";
            var hoursSpent = (double)0;
            var daysSpent = (double)0;
            var numFormatString = GblMethods.GetNumFormat(2, false);

            if (minutesSpent >= 0)
            {
                if (minutesSpent >= 60)
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

            if (hoursSpent >= 24)
            {
                daysSpent = Math.Round(hoursSpent / 24, 2);
                timeSpent = $"{FormatValue(daysSpent, DecimalFieldTypes.Decimal)} Days";
            }

            if (negative)
                timeSpent = $"-{timeSpent}";

            return timeSpent;
        }

        public static string MakeSpeed(double megaHertz)
        {
            var negative = false;
            if (megaHertz < 0)
            {
                megaHertz = Math.Abs(megaHertz);
                negative = true;
            }

            var speed = "0 MHz";
            var gigaHertz = (double)0;
            var teraHertz = (double)0;
            var petaHertz = (double)0;
            var numFormatString = GblMethods.GetNumFormat(2, false);

            if (megaHertz >= 0)
            {
                if (megaHertz >= 1000)
                {
                    gigaHertz = Math.Round(megaHertz / 1000, 2);
                    speed = $"{FormatValue(gigaHertz, DecimalFieldTypes.Decimal)} GHz";
                }
                else
                {
                    megaHertz = Math.Round(megaHertz, 2);
                    speed = $"{FormatValue(megaHertz, DecimalFieldTypes.Decimal)} MHz";
                }
            }

            if (gigaHertz >= 1000)
            {
                teraHertz = Math.Round(gigaHertz / 1000, 2);
                speed = $"{FormatValue(teraHertz, DecimalFieldTypes.Decimal)} THz";
            }

            if (teraHertz >= 1000)
            {
                petaHertz = Math.Round(teraHertz / 1000, 2);
                speed = $"{FormatValue(petaHertz, DecimalFieldTypes.Decimal)} PHz";
            }


            if (negative)
                speed = $"-{speed}";

            return speed;
        }

        public static string MakeSpace(double megaBytes)
        {
            var negative = false;
            if (megaBytes < 0)
            {
                megaBytes = Math.Abs(megaBytes);
                negative = true;
            }

            var space = "0 Megabytes";
            var gigaBytes = (double)0;
            var teraBytes = (double)0;
            var petaBytes = (double)0;
            var numFormatString = GblMethods.GetNumFormat(2, false);

            if (megaBytes >= 0)
            {
                if (megaBytes > 1000)
                {
                    gigaBytes = Math.Round(megaBytes / 1000, 2);
                    space = $"{FormatValue(gigaBytes, DecimalFieldTypes.Decimal)} GB";
                }
                else
                {
                    megaBytes = Math.Round(megaBytes, 2);
                    space = $"{FormatValue(megaBytes, DecimalFieldTypes.Decimal)} MB";
                }
            }

            if (gigaBytes >= 1000)
            {
                teraBytes = Math.Round(gigaBytes / 1000, 2);
                space = $"{FormatValue(teraBytes, DecimalFieldTypes.Decimal)} TB";
            }

            if (teraBytes >= 1000)
            {
                petaBytes = Math.Round(teraBytes / 1000, 2);
                space = $"{FormatValue(petaBytes, DecimalFieldTypes.Decimal)} PB";
            }

            if (negative)
                space = $"-{space}";

            return space;
        }


        private static string FormatValue(double value, DecimalFieldTypes decimalFieldType)
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

        public static void CalculateCustomer(Customer customer, List<CustomerUser> costs)
        {
            customer.MinutesSpent = costs.Sum(p => p.MinutesSpent);
            customer.MinutesCost = costs.Sum(p => p.Cost);
        }

        public static void CalculateTicket(SupportTicket ticket, List<SupportTicketUser> costs)
        {
            ticket.MinutesSpent = costs.Sum(p => p.MinutesSpent);
            ticket.Cost = costs.Sum(p => p.Cost);
        }

        public static void ClockInUser(RingSoft.DevLogix.DataAccess.IDbContext context, User user)
        {
            if (user != null && user.ClockOutReason != (byte)ClockOutReasons.ClockedIn)
            {
                user.ClockOutReason = (byte)ClockOutReasons.ClockedIn;
                user.ClockDate = GblMethods.NowDate().ToUniversalTime();
                context.SaveEntity(user, "Clocking In");
            }
        }

        public static double CalcPercentComplete(IEnumerable<TestingOutlineDetails> details)
        {
            var result = (double)0;
            var completeDetails = details
                .Where(p => p.IsComplete);

            var completedCount = completeDetails.Count();
            var detailsCount = details.Count();
            if (detailsCount > 0)
            {
                result = (double)completedCount / detailsCount;
            }

            return result;
        }

        public static AutoFillValue GetVersionForUser(Product product)
        {
            AutoFillValue result = null;
            var context = AppGlobals.DataRepository.GetDataContext();
            var productTable = context.GetTable<ProductVersionDepartment>();

            if (SystemGlobals.UnitTestMode)
            {
                foreach (var productVersionDepartment in productTable)
                {
                    AppGlobals.LookupContext.ProductVersionDepartments
                        .FillOutEntity(productVersionDepartment);
                }
            }

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

                    if (productVersion != null) result = productVersion.ProductVersion.GetAutoFillValue();
                }
            }
            return result;
        }

        public static string GetSupportTimeLeftTextFromDate(
            DateTime startDate
            , double? supportMinutesPurchased
            , out double? supportMinutesLeft
            , DateTime? punchOutDate = null)
        {
            var result = string.Empty;
            var nowDate = GblMethods.NowDate();
            if (punchOutDate != null)
            {
                nowDate = punchOutDate.GetValueOrDefault();
            }
            if (supportMinutesPurchased != null)
            {
                var endDate = startDate.AddMinutes(supportMinutesPurchased.Value);
                var duration = endDate.Subtract(nowDate);
                supportMinutesLeft = Math.Round(duration.TotalMinutes, 2);
                result = $"{duration.Days.ToString("00")} {duration.ToString("hh\\:mm\\:ss")}";
                if (supportMinutesLeft < 0)
                {
                    result = $"- {result}";
                }

                return result;
            }

            supportMinutesLeft = null;
            return null;
        }

        public static void FixAllDateTimes(ITwoTierProcedure procedure)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var tables = LookupContext
                .TableDefinitions
                .Where(p => p.FieldDefinitions
                    .Any(p => p.FieldDataType == FieldDataTypes.DateTime));

            var tableIndex = 1;
            var fields = new List<DateFieldDefinition>();
            foreach (var table in tables)
            {
                procedure.UpdateTopTier($"Processing Table {table.Description}", tables.Count(), tableIndex);
                var query = table.GetQueryableForTable(context);
                var recCount = query.Count();
                var recIndex = 1;
                foreach (var o in query)
                {
                    procedure.UpdateBottomTier($"Processing Record {recIndex}/{recCount}", recCount, recIndex);
                    var saveEntity = false;
                    var existFields = table.FieldDefinitions
                        .Where(p => p.FieldDataType == FieldDataTypes.DateTime);
                    foreach (var fieldDefinition in existFields)
                    {
                        if (fieldDefinition is DateFieldDefinition dateField)
                        {
                            switch (dateField.DateType)
                            {
                                case DbDateTypes.DateOnly:
                                    break;
                                case DbDateTypes.DateTime:
                                    var dateString = GblMethods.GetPropertyValue(o, dateField.PropertyName);
                                    if (!dateString.IsNullOrEmpty())
                                    {
                                        var dateVal = GblMethods.ScrubDateTime(dateString.ToDate().Value);
                                        GblMethods.SetPropertyValue(o, dateField.PropertyName, dateVal.FormatDateValue(DbDateTypes.DateTime));
                                        saveEntity = true;
                                    }
                                    break;
                                case DbDateTypes.Millisecond:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    if (saveEntity)
                    {
                        table.SaveObject(o, context, "Repairing Date");
                    }

                    recIndex++;
                }
                tableIndex++;
            }
        }

        public static string GetCurrentTimezoneTime(TimeZone timeZone)
        {
            var result = string.Empty;
            var now = DateTime.UtcNow;
            now = now.AddHours(timeZone.HourToGMT);
            result = now.ToLongTimeString();
            return result;
        }

    }
}
