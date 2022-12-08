using System;
using System.Collections.Generic;
using System.Text;
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
        public TableDefinition<User> Users { get; set; }
        public TableDefinition<Group> Groups { get; set; }
        public TableDefinition<UsersGroup> UsersGroups { get; set; }
        public TableDefinition<Department> Departments { get; set; }

        public TableDefinition<ErrorStatus> ErrorStatuses { get; set; }
        public TableDefinition<ErrorPriority> ErrorPriorities { get; set; }
        public TableDefinition<Product> Products { get; set; }
        public TableDefinition<ProductVersion> ProductVersions { get; set; }
        public TableDefinition<ProductVersionDepartment> ProductVersionDepartments { get; set; }

        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        public LookupDefinition<UserLookup, User> UserLookup { get; set; }
        public LookupDefinition<GroupLookup, Group> GroupLookup { get; set; }
        public LookupDefinition<DepartmentLookup, Department> DepartmentLookup { get; set; }

        public LookupDefinition<ErrorStatusLookup, ErrorStatus> ErrorStatusLookup { get; set; }
        public LookupDefinition<ErrorPriorityLookup, ErrorPriority> ErrorPriorityLookup { get; set; }
        public LookupDefinition<ProductLookup, Product> ProductLookup { get; set; }
        public LookupDefinition<ProductVersionLookup, ProductVersion> ProductVersionLookup { get; set; }

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
            UserLookup = new LookupDefinition<UserLookup, User>(Users);
            UserLookup.AddVisibleColumnDefinition(p => p.UserName, "Name", p => p.Name, 70);
            UserLookup.Include(p => p.Department)
                .AddVisibleColumnDefinition(p => p.Department, "Department", 
                    p => p.Description, 30);
            Users.HasLookupDefinition(UserLookup);

            GroupLookup = new LookupDefinition<GroupLookup, Group>(Groups);
            GroupLookup.AddVisibleColumnDefinition(p => p.Group, "Name", p => p.Name, 100);
            Groups.HasLookupDefinition(GroupLookup);

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

            ProductVersionLookup = new LookupDefinition<ProductVersionLookup, ProductVersion>(ProductVersions);
            ProductVersionLookup.AddVisibleColumnDefinition(p => p.Description, "Version", p => p.Description, 100);
            ProductVersions.HasLookupDefinition(ProductVersionLookup);

        }

        protected override void SetupModel()
        {
            Groups.PriorityLevel = 100;

            Products.PriorityLevel = 100;
            Products.GetFieldDefinition(p => p.Notes).IsMemo();

            ErrorStatuses.PriorityLevel = 100;
            ErrorPriorities.PriorityLevel = 100;
            
            Departments.PriorityLevel = 200;

            Users.PriorityLevel = 300;

            UsersGroups.PriorityLevel = 400;

            ProductVersions.PriorityLevel = 400;
            ProductVersions.GetFieldDefinition(p => p.Notes).IsMemo();

            ProductVersionDepartments.PriorityLevel = 500;
            ProductVersionDepartments.GetFieldDefinition(p => p.ReleaseDateTime)
                .HasDateType(DbDateTypes.DateTime);
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
