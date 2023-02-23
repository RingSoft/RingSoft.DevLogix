using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.DataAccess
{

    public class UserAutoFillArgs
    {
        public string UserName { get; set; }

        public UserAutoFill UserAutoFill { get; set; }
    }
    public class DevLogixLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public override DbDataProcessor DataProcessor => _dbDataProcessor;
        protected override DbContext DbContext => _dbContext;
        public LookupContextBase Context { get; }
        public TableDefinition<RecordLock> RecordLocks { get; set; }

        public TableDefinition<SystemMaster> SystemMaster { get; set; }
        public TableDefinition<DevLogixChart> DevLogixCharts { get; set; }
        public TableDefinition<DevLogixChartBar> DevLogixChartBars { get; set; }
        public TableDefinition<User> Users { get; set; }
        public TableDefinition<Group> Groups { get; set; }
        public TableDefinition<UsersGroup> UsersGroups { get; set; }
        public TableDefinition<Department> Departments { get; set; }
        public TableDefinition<TimeClock> TimeClocks { get; set; }

        public TableDefinition<ErrorStatus> ErrorStatuses { get; set; }
        public TableDefinition<ErrorPriority> ErrorPriorities { get; set; }
        public TableDefinition<Product> Products { get; set; }
        public TableDefinition<ProductVersion> ProductVersions { get; set; }
        public TableDefinition<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        public TableDefinition<Error> Errors { get; set; }
        public TableDefinition<ErrorDeveloper> ErrorDevelopers { get; set; }
        public TableDefinition<ErrorQa> ErrorTesters { get; set; }

        public TableDefinition<Project> Projects { get; set; }
        public TableDefinition<ProjectUser> ProjectUsers { get; set; }

        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        public LookupDefinition<DevLogixChartLookup, DevLogixChart> DevLogixChartLookup { get; set; }
        public LookupDefinition<UserLookup, User> UserLookup { get; set; }
        public LookupDefinition<GroupLookup, Group> GroupLookup { get; set; }
        public LookupDefinition<UsersGroupsLookup, UsersGroup> UsersGroupsLookup { get; set; }
        public LookupDefinition<DepartmentLookup, Department> DepartmentLookup { get; set; }
        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockLookup { get; set; }

        public LookupDefinition<ErrorStatusLookup, ErrorStatus> ErrorStatusLookup { get; set; }
        public LookupDefinition<ErrorPriorityLookup, ErrorPriority> ErrorPriorityLookup { get; set; }
        public LookupDefinition<ProductLookup, Product> ProductLookup { get; set; }
        public LookupDefinition<ProductVersionLookup, ProductVersion> ProductVersionLookup { get; set; }
        public LookupDefinition<ProductVersionDepartmentLookup, ProductVersionDepartment> ProductVersionDepartmentLookup { get; set; }
        public LookupDefinition<ErrorLookup, Error> ErrorLookup { get; set; }
        public LookupDefinition<ErrorDeveloperLookup, ErrorDeveloper> ErrorDeveloperLookup { get; set; }
        public LookupDefinition<ErrorTesterLookup, ErrorQa> ErrorTesterLookup { get; set; }

        public LookupDefinition<ProjectLookup, Project> ProjectLookup { get; set; }
        public LookupDefinition<ProjectUserLookup, ProjectUser> ProjectUserLookup { get; set; }

        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }
        public LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; }
        public SqlServerDataProcessor SqlServerDataProcessor { get; }

        public event EventHandler<UserAutoFillArgs> GetUserAutoFillEvent;


        private DbContext _dbContext;
        private DbDataProcessor _dbDataProcessor;
        private bool _initialized;

        public DevLogixLookupContext()
        {
            SqliteDataProcessor = new SqliteDataProcessor();
            SqlServerDataProcessor = new SqlServerDataProcessor();

        }

        public void SetProcessor(DbPlatforms platform)
        {
            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    _dbDataProcessor = SqliteDataProcessor;
                    break;
                case DbPlatforms.SqlServer:
                    _dbDataProcessor = SqlServerDataProcessor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }

        public void Initialize(IDevLogixDbContext dbContext, DbPlatforms dbPlatform)
        {
            dbContext.SetLookupContext(this);
            _dbContext = dbContext.DbContext;
            SystemGlobals.AdvancedFindLookupContext = this;

            SetProcessor(dbPlatform);
            if (_initialized)
            {
                return;
            }
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();
            Initialize();
            _initialized = true;
        }
        protected override void InitializeLookupDefinitions()
        {
            DevLogixChartLookup = new LookupDefinition<DevLogixChartLookup, DevLogixChart>(DevLogixCharts);
            DevLogixChartLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 50);
            DevLogixCharts.HasLookupDefinition(DevLogixChartLookup);
            
            UserLookup = new LookupDefinition<UserLookup, User>(Users);
            UserLookup.AddVisibleColumnDefinition(p => p.UserName, "Name", p => p.Name, 70);
            UserLookup.Include(p => p.Department)
                .AddVisibleColumnDefinition(p => p.Department, "Department", 
                    p => p.Description, 30);
            Users.HasLookupDefinition(UserLookup);

            GroupLookup = new LookupDefinition<GroupLookup, Group>(Groups);
            GroupLookup.AddVisibleColumnDefinition(p => p.Group, "Name", p => p.Name, 100);
            Groups.HasLookupDefinition(GroupLookup);

            UsersGroupsLookup = new LookupDefinition<UsersGroupsLookup, UsersGroup>(UsersGroups);
            UsersGroupsLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.User, "User", p => p.Name, 50);
            UsersGroupsLookup.Include(p => p.Group)
                .AddVisibleColumnDefinition(p => p.Group, "Group", p => p.Name, 50);
            UsersGroups.HasLookupDefinition(UsersGroupsLookup);

            TimeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(TimeClocks);
            TimeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, "User", p => p.Name, 50);
            TimeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, "Punch In Date", p => p.PunchInDate, 25);
            TimeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, "Minutes Spent", p => p.MinutesSpent, 25);
            TimeClocks.HasLookupDefinition(TimeClockLookup);

            ErrorStatusLookup = new LookupDefinition<ErrorStatusLookup, ErrorStatus>(ErrorStatuses);
            ErrorStatusLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 100);
            ErrorStatuses.HasLookupDefinition(ErrorStatusLookup);

            DepartmentLookup = new LookupDefinition<DepartmentLookup, Department>(Departments);
            DepartmentLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 100);
            Departments.HasLookupDefinition(DepartmentLookup);

            ErrorPriorityLookup = new LookupDefinition<ErrorPriorityLookup, ErrorPriority>(ErrorPriorities);
            ErrorPriorityLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 100);
            ErrorPriorities.HasLookupDefinition(ErrorPriorityLookup);

            ProductLookup = new LookupDefinition<ProductLookup, Product>(Products);
            ProductLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 100);
            Products.HasLookupDefinition(ProductLookup);

            ProductVersionLookup = MakeProductVersionLookupDefinition();
            //ProductVersionLookup.Include(p => p.Product)
            //    .AddVisibleColumnDefinition(p => p.Product, "Product", p => p.Description, 50);

            ProductVersions.HasLookupDefinition(ProductVersionLookup);

            ProductVersionDepartmentLookup =
                new LookupDefinition<ProductVersionDepartmentLookup, ProductVersionDepartment>(
                    ProductVersionDepartments);
            ProductVersionDepartmentLookup.Include(p => p.ProductVersion)
                .AddVisibleColumnDefinition(p => p.ProductVersion, "Product Version", p => p.Description, 33);
            ProductVersionDepartmentLookup.Include(p => p.Department)
                .AddVisibleColumnDefinition(p => p.Department, "Department", p => p.Description, 34);
            ProductVersionDepartmentLookup.AddVisibleColumnDefinition(p => p.ReleaseDate, "Date Released",
                p => p.ReleaseDateTime, 33);
            ProductVersionDepartments.HasLookupDefinition(ProductVersionDepartmentLookup);

            ErrorLookup = new LookupDefinition<ErrorLookup, Error>(Errors);
            ErrorLookup.AddVisibleColumnDefinition(p => p.ErrorId, "Error ID", p => p.ErrorId, 20);
            ErrorLookup.Include(p => p.ErrorStatus)
                .AddVisibleColumnDefinition(p => p.Status, "Status", p => p.Description, 30);
            ErrorLookup.Include(p => p.ErrorPriority)
                .AddVisibleColumnDefinition(p => p.Priority, "Priority", p => p.Description, 30);
            ErrorLookup.AddVisibleColumnDefinition(p => p.Date, "Date", p => p.ErrorDate, 20);
            Errors.HasLookupDefinition(ErrorLookup);

            ErrorDeveloperLookup = new LookupDefinition<ErrorDeveloperLookup, ErrorDeveloper>(ErrorDevelopers);
            ErrorDeveloperLookup.Include(p => p.Error)
                .AddVisibleColumnDefinition(p => p.ErrorId, "Error Id", p => p.ErrorId, 40);
            ErrorDeveloperLookup.Include(p => p.Developer)
                .AddVisibleColumnDefinition(p => p.Developer, "Developer", p => p.Name, 60);
            ErrorDevelopers.HasLookupDefinition(ErrorDeveloperLookup);

            ErrorTesterLookup = new LookupDefinition<ErrorTesterLookup, ErrorQa>(ErrorTesters);
            ErrorTesterLookup.Include(p => p.Error)
                .AddVisibleColumnDefinition(p => p.ErrorId, "Error Id", p => p.ErrorId, 40);
            ErrorTesterLookup.Include(p => p.Tester)
                .AddVisibleColumnDefinition(p => p.Tester, "Tester", p => p.Name, 60);
            ErrorTesters.HasLookupDefinition(ErrorTesterLookup);

            ProjectLookup = new LookupDefinition<ProjectLookup, Project>(Projects);
            ProjectLookup.AddVisibleColumnDefinition(p => p.Name, "Project Name", p => p.Name, 70);
            ProjectLookup.AddVisibleColumnDefinition(p => p.Deadline, "Deadline", p => p.Deadline, 30);
            Projects.HasLookupDefinition(ProjectLookup);

            ProjectUserLookup = new LookupDefinition<ProjectUserLookup, ProjectUser>(ProjectUsers);
            ProjectUserLookup.Include(p => p.Project)
                .AddVisibleColumnDefinition(p => p.Project, "Project", p => p.Name, 50);
            ProjectUserLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.User, "User", p => p.Name, 50);
            ProjectUsers.HasLookupDefinition(ProjectUserLookup);

        }

        public LookupDefinition<ProductVersionLookup, ProductVersion> MakeProductVersionLookupDefinition()
        {
            var result = new LookupDefinition<ProductVersionLookup, ProductVersion>(ProductVersions);

            result.AddVisibleColumnDefinition(p => p.Description, "Version", p => p.Description, 35);

            var tableDefinition = ProductVersions;
            var query = new SelectQuery(tableDefinition.TableName);
            foreach (var fieldDefinition in tableDefinition.FieldDefinitions)
            {
                query.AddSelectColumn(fieldDefinition.FieldName);
            }
            //query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Id).FieldName);
            //query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.ProductId).FieldName);
            //query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Description).FieldName);
            query.AddSelectFormulaColumn("VersionDate", MakeVersionDateFormula());
            query.AddSelectFormulaColumn("MaxDepartment", MakeMaxDepartmentFormula());

            var sql = DataProcessor.SqlGenerator.GenerateSelectStatement(query);
            result.HasFromFormula(sql);

            var column = result.AddVisibleColumnDefinition(p => p.VersionDate
                , "VersionDate", "");
            column.HasDateType(DbDateTypes.DateTime)
                .HasDateFormatString(string.Empty)
                .HasConvertToLocalTime();

            column = result.AddVisibleColumnDefinition(p => p.MaxDepartment
                , "MaxDepartment", "");

            result.VisibleColumns[0].UpdatePercentWidth(35);
            result.VisibleColumns[0].UpdateCaption("Version");
            result.VisibleColumns[1].UpdatePercentWidth(25);
            result.VisibleColumns[1].UpdateCaption("Release Date");
            result.VisibleColumns[2].UpdatePercentWidth(40);
            result.VisibleColumns[2].UpdateCaption("Released To Department");

            result.InitialSortColumnDefinition = result.VisibleColumns[0];
            result.InitialOrderByColumn = result.VisibleColumns[1];
            result.InitialOrderByType = OrderByTypes.Descending;

            return result;
        }

        private string MakeVersionDateFormula()
        {
            var result = string.Empty;

            var tableDefinition = ProductVersionDepartments;
            var field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
            field = DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";
            var query = new SelectQuery(tableDefinition.TableName);
            query.AddSelectFormulaColumn("VersionDate", $"MAX({field})");

            field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
            field = DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            var targetField = ProductVersions
                .GetFieldDefinition(p => p.Id).FieldName;
            targetField = DataProcessor.SqlGenerator.FormatSqlObject(targetField);
            targetField = $"{DataProcessor.SqlGenerator.FormatSqlObject(ProductVersions.TableName)}.{targetField}";

            query.AddWhereItemFormula($"{field} = {targetField}");

            result = DataProcessor.SqlGenerator.GenerateSelectStatement(query);

            return result;
        }

        private string MakeMaxDepartmentFormula()
        {
            var result = string.Empty;
            var tableDefinition = ProductVersionDepartments;
            var descriptionField = Departments.GetFieldDefinition(p => p.Description).FieldName;
            var query = new SelectQuery(tableDefinition.TableName);
            var departmentJoin =
                query.AddPrimaryJoinTable(JoinTypes.InnerJoin, Departments.TableName)
                    .AddJoinField(Departments.GetFieldDefinition(p => p.Id).FieldName
                    , tableDefinition.GetFieldDefinition(p => p.DepartmentId).FieldName);
            query.AddSelectColumn(descriptionField, departmentJoin);

            var field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
            field = DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            var targetField = ProductVersions
                .GetFieldDefinition(p => p.Id).FieldName;
            targetField = DataProcessor.SqlGenerator.FormatSqlObject(targetField);
            targetField = $"{DataProcessor.SqlGenerator.FormatSqlObject(ProductVersions.TableName)}.{targetField}";

            query.AddWhereItemFormula($"{field} = {targetField}");

            field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
            field = DataProcessor.SqlGenerator.FormatSqlObject(field);
            field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

            query.AddWhereItemFormula($"{field} = ({MakeVersionDateFormula()})");

            result = DataProcessor.SqlGenerator.GenerateSelectStatement(query);
            return result;
        }


        protected override void SetupModel()
        {
            var advancedField = AdvancedFinds.GetFieldDefinition(p => p.Id);
            var chartAdvFindField = DevLogixChartBars.GetFieldDefinition(p => p.AdvancedFindId);
            chartAdvFindField.SetParentField(advancedField, advancedField.PropertyName);

            Groups.PriorityLevel = 100;

            ErrorStatuses.PriorityLevel = 100;
            ErrorPriorities.PriorityLevel = 100;
            
            Departments.PriorityLevel = 200;
            DevLogixCharts.PriorityLevel = 200;

            AdvancedFinds.PriorityLevel = 250;
            DevLogixChartBars.PriorityLevel = 300;

            Products.PriorityLevel = 300;
            Products.GetFieldDefinition(p => p.Notes).IsMemo();

            Users.PriorityLevel = 300;
            Users.GetFieldDefinition(p => p.SupervisorId).DoesAllowRecursion(false);

            UsersGroups.PriorityLevel = 400;

            ProductVersions.PriorityLevel = 400;
            ProductVersions.GetFieldDefinition(p => p.Notes).IsMemo();
            ProductVersions.GetFieldDefinition(p => p.ArchiveDateTime).HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            ProductVersionDepartments.PriorityLevel = 500;
            ProductVersionDepartments.GetFieldDefinition(p => p.ReleaseDateTime)
                .HasDateType(DbDateTypes.DateTime);

            Errors.PriorityLevel = 400;
            Errors.GetFieldDefinition(p => p.ErrorDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            Errors.GetFieldDefinition(p => p.Description)
                .IsMemo();
            Errors.GetFieldDefinition(p => p.Resolution)
                .IsMemo();
            Errors.GetFieldDefinition(p => p.FoundByUserId).CanSetNull(false);

            ErrorDevelopers.PriorityLevel = 500;
            ErrorDevelopers.GetFieldDefinition(p => p.DateFixed)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            ErrorTesters.PriorityLevel = 500;
            ErrorTesters.GetFieldDefinition(p => p.DateChanged)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            TimeClocks.PriorityLevel = 600;
            TimeClocks.GetFieldDefinition(p => p.Notes).IsMemo();
            TimeClocks.GetFieldDefinition(p => p.PunchInDate).HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            TimeClocks.GetFieldDefinition(p => p.PunchOutDate).HasDateType(DbDateTypes.DateTime).DoConvertToLocalTime();
            TimeClocks.GetFieldDefinition(p => p.ErrorId).CanSetNull(false);

            Projects.GetFieldDefinition(p => p.Notes).IsMemo();
            Projects.GetFieldDefinition(p => p.Deadline).HasDateType(DbDateTypes.DateTime);
            Projects.GetFieldDefinition(p => p.OriginalDeadline).HasDateType(DbDateTypes.DateTime);
        }

        public override UserAutoFill GetUserAutoFill(string userName)
        {
            var userAutoFill = new UserAutoFillArgs();
            userAutoFill.UserAutoFill = new UserAutoFill();
            userAutoFill.UserName = userName;
            GetUserAutoFillEvent?.Invoke(this, userAutoFill);
            if (userAutoFill.UserAutoFill.AutoFillSetup != null)
            {
                return userAutoFill.UserAutoFill;
            }
            return base.GetUserAutoFill(userName);
        }
    }
}
