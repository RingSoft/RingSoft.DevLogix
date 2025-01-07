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
using System.Linq;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.LookupModel.QualityAssurance;
using RingSoft.DevLogix.DataAccess.LookupModel.UserManagement;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

namespace RingSoft.DevLogix.DataAccess
{

    public class UserAutoFillArgs
    {
        public string UserName { get; set; }

        public UserAutoFill UserAutoFill { get; set; }
    }
    public class DevLogixLookupContext : LookupContext
    {
        public const int TimeSpentHostId = 100;
        public const int SpeedHostId = 200;
        public const int MemoryHostId = 300;
        public override DbDataProcessor DataProcessor => _dbDataProcessor;
        //protected override DbContext DbContext => _dbContext;

        public TableDefinition<SystemMaster> SystemMaster { get; set; }
        public TableDefinition<DevLogixChart> DevLogixCharts { get; set; }
        public TableDefinition<DevLogixChartBar> DevLogixChartBars { get; set; }
        public TableDefinition<SystemPreferences> SystemPreferences { get; set; }
        public TableDefinition<SystemPreferencesHolidays> SystemPreferencesHolidays { get; set; }

        public TableDefinition<User> Users { get; set; }
        public TableDefinition<UserTimeOff> UsersTimeOff { get; set; }
        public TableDefinition<Group> Groups { get; set; }
        public TableDefinition<UsersGroup> UsersGroups { get; set; }
        public TableDefinition<Department> Departments { get; set; }
        public TableDefinition<TimeClock> TimeClocks { get; set; }
        public TableDefinition<UserTracker> UserTracker { get; set; }
        public TableDefinition<UserTrackerUser> UserTrackerUsers { get; set; }
        public TableDefinition<UserMonthlySales> UseerMonthlySales { get; set; }

        public TableDefinition<ErrorStatus> ErrorStatuses { get; set; }
        public TableDefinition<ErrorPriority> ErrorPriorities { get; set; }
        public TableDefinition<Product> Products { get; set; }
        public TableDefinition<ProductVersion> ProductVersions { get; set; }
        public TableDefinition<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        public TableDefinition<Error> Errors { get; set; }
        public TableDefinition<ErrorDeveloper> ErrorDevelopers { get; set; }
        public TableDefinition<ErrorQa> ErrorTesters { get; set; }
        public TableDefinition<ErrorUser> ErrorUsers { get; set; }
        public TableDefinition<TestingTemplate> TestingTemplates { get; set; }
        public TableDefinition<TestingTemplateItem> TestingTemplatesItems { get; set; }
        public TableDefinition<TestingOutline> TestingOutlines { get; set; }
        public TableDefinition<TestingOutlineDetails> TestingOutlineDetails { get; set; }
        public TableDefinition<TestingOutlineTemplate> TestingOutlineTemplates { get; set; }
        public TableDefinition<TestingOutlineCost> TestingOutlineCosts { get; set; }

        public TableDefinition<Project> Projects { get; set; }
        public TableDefinition<ProjectUser> ProjectUsers { get; set; }
        public TableDefinition<LaborPart> LaborParts { get; set; }
        public TableDefinition<ProjectTask> ProjectTasks { get; set; }
        public TableDefinition<ProjectTaskLaborPart> ProjectTaskLaborParts { get; set; }
        public TableDefinition<MaterialPart> MaterialParts { get; set; }
        public TableDefinition<ProjectMaterial> ProjectMaterials { get; set; }
        public TableDefinition<ProjectMaterialPart> ProjectMaterialParts { get; set; }
        public TableDefinition<ProjectMaterialHistory> ProjectMaterialHistory { get; set; }
        public TableDefinition<ProjectTaskDependency> ProjectTaskDependency { get; set; }

        public TableDefinition<TimeZone> TimeZone { get; set; }
        public TableDefinition<Territory> Territory { get; set; }
        public TableDefinition<Customer> Customer { get; set; }
        public TableDefinition<CustomerProduct> CustomerProduct { get; set; }
        public TableDefinition<Order> Order { get; set; }
        public TableDefinition<OrderDetail> OrderDetail { get; set; }
        public TableDefinition<CustomerComputer> CustomerComputer { get; set; }
        public TableDefinition<SupportTicket> SupportTicket { get; set; }
        public TableDefinition<SupportTicketUser> SupportTicketUser { get; set; }
        public TableDefinition<CustomerUser> CustomerUser { get; set; }
        public TableDefinition<SupportTicketError> SupportTicketError { get; set; }
        public TableDefinition<CustomerStatus> CustomerStatus { get; set; }
        public TableDefinition<SupportTicketStatus> SupportTicketStatus { get; set; }

        public LookupDefinition<DevLogixChartLookup, SystemMaster> SystemMasterLookupDefinition { get; set; }
        public LookupDefinition<DevLogixChartLookup, DevLogixChart> DevLogixChartLookup { get; set; }
        public LookupDefinition<DevLogixChartLookup, DevLogixChartBar> DevLogixChartBarLookup { get; set; }
        public LookupDefinition<SystemPreferencesLookup, SystemPreferences> SystemPreferencesLookup { get; set; }
        public LookupDefinition<DevLogixChartLookup, SystemPreferencesHolidays> SystemPreferencesHolidaysLookup { get; set; }

        public LookupDefinition<UserLookup, User> UserLookup { get; set; }
        public LookupDefinition<UserTimeOffLookup, UserTimeOff> UserTimeOffLookup { get; set; }
        public LookupDefinition<GroupLookup, Group> GroupLookup { get; set; }
        public LookupDefinition<UsersGroupsLookup, UsersGroup> UsersGroupsLookup { get; set; }
        public LookupDefinition<DepartmentLookup, Department> DepartmentLookup { get; set; }
        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockLookup { get; set; }
        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockTabLookup { get; set; }
        public LookupDefinition<UserTrackerLookup, UserTracker> UserTrackerLookup { get; set; }
        public LookupDefinition<UserTrackerUserLookup, UserTrackerUser> UserTrackerUserLookup { get; set; }
        public LookupDefinition<UserMonthlySalesLookup, UserMonthlySales> UserMonthlySalesLookup { get; set; }

        public LookupDefinition<ErrorStatusLookup, ErrorStatus> ErrorStatusLookup { get; set; }
        public LookupDefinition<ErrorPriorityLookup, ErrorPriority> ErrorPriorityLookup { get; set; }
        public LookupDefinition<ProductLookup, Product> ProductLookup { get; set; }
        public LookupDefinition<ProductVersionLookup, ProductVersion> ProductVersionLookup { get; set; }
        public LookupDefinition<ProductVersionDepartmentLookup, ProductVersionDepartment> ProductVersionDepartmentLookup { get; set; }
        public LookupDefinition<ErrorLookup, Error> ErrorLookup { get; set; }
        public LookupDefinition<ErrorDeveloperLookup, ErrorDeveloper> ErrorDeveloperLookup { get; set; }
        public LookupDefinition<ErrorTesterLookup, ErrorQa> ErrorTesterLookup { get; set; }
        public LookupDefinition<ErrorUserLookup, ErrorUser> ErrorUsersLookup { get; set; }
        public LookupDefinition<TestingTemplateLookup, TestingTemplate> TestingTemplateLookup { get; set; }
        public LookupDefinition<TestingTemplateItemLookup, TestingTemplateItem> TestingTemplateItemLookup { get; set; }
        public LookupDefinition<TestingOutlineLookup, TestingOutline> TestingOutlineLookup { get; set; }
        public LookupDefinition<TestingOutlineDetailsLookup, TestingOutlineDetails> TestingOutlineDetailsLookup { get; set; }
        public LookupDefinition<TestingOutlineTemplateLookup, TestingOutlineTemplate> TestingOutlineTemplateLookup { get; set; }
        public LookupDefinition<TestingOutlineCostLookup, TestingOutlineCost> TestingOutlineCostLookup { get; set; }

        public LookupDefinition<ProjectLookup, Project> ProjectLookup { get; set; }
        public LookupDefinition<ProjectUserLookup, ProjectUser> ProjectUserLookup { get; set; }
        public LookupDefinition<LaborPartLookup, LaborPart> LaborPartLookup { get; set; }
        public LookupDefinition<ProjectTaskLookup, ProjectTask> ProjectTaskLookup { get; set; }
        public LookupDefinition<ProjectTaskLaborPartLookup, ProjectTaskLaborPart> ProjectTaskLaborPartLookup { get; set; }
        public LookupDefinition<MaterialPartLookup, MaterialPart> MaterialPartLookup { get; set; }
        public LookupDefinition<ProjectMaterialLookup, ProjectMaterial> ProjectMaterialLookup { get; set; }
        public LookupDefinition<ProjectMaterialPartLookup, ProjectMaterialPart> ProjectMaterialPartLookup { get; set; }
        public LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> ProjectMaterialHistoryLookup { get; set; }
        public LookupDefinition<ProjectTaskDependencyLookup, ProjectTaskDependency> ProjectTaskDependencyLookup { get; set; }

        public LookupDefinition<TimeZoneLookup, TimeZone> TimeZoneLookup { get; set; }
        public LookupDefinition<TerritoryLookup, Territory> TerritoryLookup { get; set; }
        public LookupDefinition<CustomerLookup, Customer> CustomerLookup { get; set; }
        public LookupDefinition<CustomerProductLookup, CustomerProduct> CustomerProductsLookup { get; set; }
        public LookupDefinition<OrderLookup, Order> OrderLookup { get; set; }
        public LookupDefinition<OrderDetailLookup, OrderDetail> OrderDetailLookup { get; set; }
        public LookupDefinition<CustomerComputerLookup, CustomerComputer> CustomerComputerLookup { get; set; }
        public LookupDefinition<SupportTicketLookup, SupportTicket> SupportTicketLookup { get; set; }
        public LookupDefinition<SupportTicketUserLookup, SupportTicketUser> SupportTicketUserLookup { get; set; }
        public LookupDefinition<CustomerUserLookup, CustomerUser> CustomerUserLookup { get; set; }
        public LookupDefinition<SupportTicketErrorLookup, SupportTicketError> SupportTicketErrorLookup { get; set; }
        public LookupDefinition<CustomerStatusLookup, CustomerStatus> CustomerStatusLookup { get; set; }
        public LookupDefinition<SupportTicketStatusLookup, SupportTicketStatus> SupportTicketStatusLookup { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; }
        public SqlServerDataProcessor SqlServerDataProcessor { get; }

        public event EventHandler<UserAutoFillArgs> GetUserAutoFillEvent;


        //private DbContext _dbContext;
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
            SetDbContext(dbContext.DbContext);

            SetProcessor(dbPlatform);
            if (_initialized)
            {
                return;
            }
            Initialize();
            _initialized = true;
        }
        protected override void InitializeLookupDefinitions()
        {
            SystemMasterLookupDefinition = new LookupDefinition<DevLogixChartLookup, SystemMaster>(SystemMaster);
            SystemMasterLookupDefinition.AddVisibleColumnDefinition(
                p => p.Name
                , "Name"
                , p => p.OrganizationName, 99);
            SystemMaster.HasLookupDefinition(SystemMasterLookupDefinition);

            SystemPreferencesLookup =
                new LookupDefinition<SystemPreferencesLookup, SystemPreferences>(SystemPreferences);
            SystemPreferencesLookup.AddVisibleColumnDefinition(p => p.Id
                ,  "Id", p => p.Id, 99);
            SystemPreferences.HasLookupDefinition(SystemPreferencesLookup);

            SystemPreferencesHolidaysLookup =
                new LookupDefinition<DevLogixChartLookup, SystemPreferencesHolidays>(SystemPreferencesHolidays);
            SystemPreferencesHolidaysLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Holiday"
                , p => p.Name, 99);
            SystemPreferencesHolidays.HasLookupDefinition(SystemPreferencesHolidaysLookup);

            DevLogixChartLookup = new LookupDefinition<DevLogixChartLookup, DevLogixChart>(DevLogixCharts);
            DevLogixChartLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 50);
            DevLogixCharts.HasLookupDefinition(DevLogixChartLookup);

            DevLogixChartBarLookup = new LookupDefinition<DevLogixChartLookup, DevLogixChartBar>(DevLogixChartBars);
            DevLogixChartBarLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 50);
            DevLogixChartBars.HasLookupDefinition(DevLogixChartBarLookup);

            UserLookup = new LookupDefinition<UserLookup, User>(Users);
            UserLookup.AddVisibleColumnDefinition(p => p.UserName, "Name", p => p.Name, 70);
            UserLookup.Include(p => p.Department)
                .AddVisibleColumnDefinition(p => p.Department, "Department", 
                    p => p.Description, 30);
            Users.HasLookupDefinition(UserLookup);

            UserTimeOffLookup = new LookupDefinition<UserTimeOffLookup, UserTimeOff>(UsersTimeOff);
            UserTimeOffLookup
                .Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, "User", p => p.Name, 40);
            UserTimeOffLookup.AddVisibleColumnDefinition(p => p.StartDate, "Start Date", p => p.StartDate, 30);
            UserTimeOffLookup.AddVisibleColumnDefinition(p => p.EndDate, "End Date", p => p.EndDate, 30);
            UsersTimeOff.HasLookupDefinition(UserTimeOffLookup);

            UserMonthlySalesLookup = new LookupDefinition<UserMonthlySalesLookup, UserMonthlySales>(UseerMonthlySales);
            UserMonthlySalesLookup.Include(
                    p => p.User)
                .AddVisibleColumnDefinition(
                    p => p.UserName
                    , "User"
                    , p => p.Name, 34);

            UserMonthlySalesLookup.AddVisibleColumnDefinition(
                p => p.MonthEnding
                , "Month Ending"
                , p => p.MonthEnding, 33);

            UserMonthlySalesLookup.AddVisibleColumnDefinition(
                p => p.TotalSales
                , "Total Sales"
                , p => p.TotalSales, 33);

            UseerMonthlySales.HasLookupDefinition(UserMonthlySalesLookup);

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

            TimeClockLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Time Clock ID"
                , p => p.Name, 25);
            TimeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(
                    p => p.UserName
                    , "User"
                    , p => p.Name, 25);
            TimeClockLookup.AddVisibleColumnDefinition(
                p => p.PunchInDate
                , "Punch In Date"
                , p => p.PunchInDate, 25);
            var column =
                TimeClockLookup.AddVisibleColumnDefinition(
                    p => p.MinutesSpent
                    , "Time Spent"
                    , p => p.MinutesSpent,
                    25);
            column.HasSearchForHostId(TimeSpentHostId);
            TimeClocks.HasLookupDefinition(TimeClockLookup);

            TimeClockTabLookup = new LookupDefinition<TimeClockLookup, TimeClock>(TimeClocks);

            TimeClockTabLookup.AddVisibleColumnDefinition(
                p => p.PunchInDate
                , "Punch In Date"
                , p => p.PunchInDate, 25);
            
            TimeClockTabLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(
                    p => p.UserName
                    , "User"
                    , p => p.Name, 25);

            TimeClockTabLookup.AddVisibleColumnDefinition(
                    p => p.MinutesSpent
                    , "Time Spent"
                    , p => p.MinutesSpent,
                    25);


            UserTrackerLookup = new LookupDefinition<UserTrackerLookup, UserTracker>(UserTracker);
            UserTrackerLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 95);
            UserTracker.HasLookupDefinition(UserTrackerLookup);

            UserTrackerUserLookup = new LookupDefinition<UserTrackerUserLookup, UserTrackerUser>(UserTrackerUsers);
            UserTrackerUserLookup.Include(p => p.UserTracker)
                .AddVisibleColumnDefinition(p => p.UserTracker, "User Tracker"
                    , p => p.Name, 50);
            UserTrackerUserLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.User, "User"
                , p => p.Name, 50);
            UserTrackerUsers.HasLookupDefinition(UserTrackerUserLookup);

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
            ProductLookup.AddVisibleColumnDefinition(
                p => p.Description
                , "Description"
                , p => p.Description, 50);
            ProductLookup.AddVisibleColumnDefinition(
                p => p.Revenue
                , "Revenue"
                , p => p.Revenue, 25);
            ProductLookup.AddVisibleColumnDefinition(
                p => p.Revenue
                , "Cost"
                , p => p.Cost, 25);

            Products.HasLookupDefinition(ProductLookup);

            ProductVersionLookup = new LookupDefinition<ProductVersionLookup, ProductVersion>(ProductVersions);
            ProductVersionLookup = MakeProductVersionLookupDefinition();

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
            ErrorLookup.InitialOrderByField = Errors.GetFieldDefinition(p => p.Id);
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

            ErrorUsersLookup = new LookupDefinition<ErrorUserLookup, ErrorUser>(ErrorUsers);
            ErrorUsersLookup.Include(p => p.Error)
                .AddVisibleColumnDefinition(p => p.ErrorId, "Error Id", p => p.ErrorId, 30);
            ErrorUsersLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, "User", p => p.Name, 70);
            ErrorUsers.HasLookupDefinition(ErrorUsersLookup);

            TestingTemplateLookup = new LookupDefinition<TestingTemplateLookup, TestingTemplate>(TestingTemplates);
            TestingTemplateLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 95);
            TestingTemplates.HasLookupDefinition(TestingTemplateLookup);

            TestingTemplateItemLookup =
                new LookupDefinition<TestingTemplateItemLookup, TestingTemplateItem>(TestingTemplatesItems);
            TestingTemplateItemLookup.Include(p => p.TestingTemplate)
                .AddVisibleColumnDefinition(p => p.TestingTemplate
                    , "Testing Template"
                    , p => p.Name, 50);
            TestingTemplateItemLookup.AddVisibleColumnDefinition(p => p.Description
                , "Item"
                , p => p.Description, 50);
            TestingTemplatesItems.HasLookupDefinition(TestingTemplateItemLookup);

            TestingOutlineLookup = new LookupDefinition<TestingOutlineLookup, TestingOutline>(TestingOutlines);
            TestingOutlineLookup.AddVisibleColumnDefinition(p => p.Name
                , "Name"
                , p => p.Name, 34);
            TestingOutlineLookup.Include(p => p.Product)
                .AddVisibleColumnDefinition(p => p.Name
                    , "Product"
                    , p => p.Description, 33);
            TestingOutlineLookup.Include(p => p.AssignedToUser)
                .AddVisibleColumnDefinition(p => p.AssignedTo
                    , "Assigned To"
                    , p => p.Name, 33);
            TestingOutlines.HasLookupDefinition(TestingOutlineLookup);

            TestingOutlineDetailsLookup =
                new LookupDefinition<TestingOutlineDetailsLookup, TestingOutlineDetails>(TestingOutlineDetails);
            TestingOutlineDetailsLookup.Include(p => p.TestingOutline)
                .AddVisibleColumnDefinition(p => p.TestingOutline
                    , "Testing Outline"
                    , p => p.Name, 50);
            TestingOutlineDetailsLookup.AddVisibleColumnDefinition(p => p.Step
                , "Step"
                , p => p.Step, 50);
            TestingOutlineDetails.HasLookupDefinition(TestingOutlineDetailsLookup);

            TestingOutlineTemplateLookup =
                new LookupDefinition<TestingOutlineTemplateLookup, TestingOutlineTemplate>(TestingOutlineTemplates);
            TestingOutlineTemplateLookup.Include(p => p.TestingOutline)
                .AddVisibleColumnDefinition(p => p.TestingOutline
                , "Testing Outline"
                , p => p.Name, 50);
            TestingOutlineTemplateLookup.Include(p => p.TestingTemplate)
                .AddVisibleColumnDefinition(p => p.TestingTemplate
                , "Testing Template"
                , p => p.Name, 50);
            TestingOutlineTemplates.HasLookupDefinition(TestingOutlineTemplateLookup);

            TestingOutlineCostLookup =
                new LookupDefinition<TestingOutlineCostLookup, TestingOutlineCost>(TestingOutlineCosts);
            TestingOutlineCostLookup.Include(p => p.TestingOutline)
                .AddVisibleColumnDefinition(p => p.TestingOutline
                    , "Testing Outline"
                    , p => p.Name, 40);
            TestingOutlineCostLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName
                    , "User"
                    , p => p.Name, 40);
            column = TestingOutlineCostLookup.AddVisibleColumnDefinition(
                p => p.MinutesSpent
                , "Time Spent"
                , p => p.TimeSpent, 20);
            column.HasSearchForHostId(TimeSpentHostId);
            TestingOutlineCosts.HasLookupDefinition(TestingOutlineCostLookup);

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

            LaborPartLookup = new LookupDefinition<LaborPartLookup, LaborPart>(LaborParts);
            LaborPartLookup.AddVisibleColumnDefinition(p => p.Name, "Labor Part", p => p.Name, 100);
            LaborParts.HasLookupDefinition(LaborPartLookup);

            ProjectTaskLookup = new LookupDefinition<ProjectTaskLookup, ProjectTask>(ProjectTasks);
            ProjectTaskLookup.AddVisibleColumnDefinition(p => p.Name, "Task Name", p => p.Name, 25);
            ProjectTaskLookup.Include(p => p.Project)
                .AddVisibleColumnDefinition(p => p.ProjectName, "Project", p => p.Name, 25);

            ProjectTaskLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, "User", p => p.Name, 25);

            ProjectTaskLookup.AddVisibleColumnDefinition(p => p.PercentComplete, "Percent Complete",
                p => p.PercentComplete, 25);

            ProjectTasks.HasLookupDefinition(ProjectTaskLookup);

            ProjectTaskLaborPartLookup =
                new LookupDefinition<ProjectTaskLaborPartLookup, ProjectTaskLaborPart>(ProjectTaskLaborParts);

            ProjectTaskLaborPartLookup.Include(p => p.ProjectTask)
                .AddVisibleColumnDefinition(p => p.ProjectName, "Project"
                    , p => p.Name, 34);
            ProjectTaskLaborPartLookup.Include(p => p.LaborPart)
                .AddVisibleColumnDefinition(p => p.LaborPart
                    , "Labor Part", p => p.Name, 33);
            ProjectTaskLaborPartLookup.AddVisibleColumnDefinition(p => p.Description
                , "Description", p => p.Description, 33);
            ProjectTaskLaborParts.HasLookupDefinition(ProjectTaskLaborPartLookup);

            MaterialPartLookup = new LookupDefinition<MaterialPartLookup, MaterialPart>(MaterialParts);
            MaterialPartLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 70);
            MaterialPartLookup.AddVisibleColumnDefinition(p => p.Cost, "Cost", p => p.Cost, 30);
            MaterialParts.HasLookupDefinition(MaterialPartLookup);

            ProjectMaterialLookup = new LookupDefinition<ProjectMaterialLookup, ProjectMaterial>(ProjectMaterials);
            ProjectMaterialLookup.AddVisibleColumnDefinition(p => p.Name, "Name", p => p.Name, 50);
            ProjectMaterialLookup.AddVisibleColumnDefinition(p => p.Cost, "Cost", p => p.Cost, 25);
            ProjectMaterialLookup.AddVisibleColumnDefinition(p => p.ActualCost, "Actual Cost", p => p.ActualCost, 25);
            ProjectMaterials.HasLookupDefinition(ProjectMaterialLookup);

            ProjectMaterialPartLookup =
                new LookupDefinition<ProjectMaterialPartLookup, ProjectMaterialPart>(ProjectMaterialParts);

            ProjectMaterialPartLookup.Include(p => p.ProjectMaterial)
                .AddVisibleColumnDefinition(p => p.ProjectName, "Project"
                    , p => p.Name, 34);
            ProjectMaterialPartLookup.Include(p => p.MaterialPart)
                .AddVisibleColumnDefinition(p => p.MaterialPart
                    , "Material Part", p => p.Name, 33);
            ProjectMaterialPartLookup.AddVisibleColumnDefinition(p => p.Description
                , "Description", p => p.Description, 33);
            ProjectMaterialParts.HasLookupDefinition(ProjectMaterialPartLookup);

            ProjectMaterialHistoryLookup =
                new LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory>(ProjectMaterialHistory);
            ProjectMaterialHistoryLookup.AddVisibleColumnDefinition(p => p.Date, "Date", p => p.Date, 20);
            ProjectMaterialHistoryLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, "User", p => p.Name, 40);
            ProjectMaterialHistoryLookup.AddVisibleColumnDefinition(p => p.Quantity, "Quantity", p => p.Quantity, 20);

            ProjectMaterialHistoryLookup.AddVisibleColumnDefinition(p => p.Cost, "Cost", p => p.Cost, 20);
            ProjectMaterialHistory.HasLookupDefinition(ProjectMaterialHistoryLookup);

            ProjectTaskDependencyLookup =
                new LookupDefinition<ProjectTaskDependencyLookup, ProjectTaskDependency>(ProjectTaskDependency);
            ProjectTaskDependencyLookup.Include(p => p.ProjectTask)
                .AddVisibleColumnDefinition(p => p.ProjectTask, "Project Task", p => p.Name, 50);
            ProjectTaskDependencyLookup.Include(p => p.DependsOnProjectTask)
                .AddVisibleColumnDefinition(p => p.DependsOn, "Depends On", p => p.Name, 50);
            ProjectTaskDependency.HasLookupDefinition(ProjectTaskDependencyLookup);

            TimeZoneLookup = new LookupDefinition<TimeZoneLookup, TimeZone>(TimeZone);
            TimeZoneLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Name"
                , p => p.Name, 99);
            TimeZone.HasLookupDefinition(TimeZoneLookup);

            TerritoryLookup = new LookupDefinition<TerritoryLookup, Territory>(Territory);
            TerritoryLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Name"
                , p => p.Name, 50);

            TerritoryLookup.Include(p => p.Salesperson)
                .AddVisibleColumnDefinition(
                    p => p.Salesperson
                    , "Salesperson"
                    , p => p.Name, 50);
            Territory.HasLookupDefinition(TerritoryLookup);

            CustomerLookup = new LookupDefinition<CustomerLookup, Customer>(Customer);
            CustomerLookup.AddVisibleColumnDefinition(
                p => p.CompanyName
                , "Company Name"
                , p => p.CompanyName, 50);
            CustomerLookup.AddVisibleColumnDefinition(
                p => p.PhoneNumber
                , "Phone Number"
                , p => p.Phone, 50);
            Customer.HasLookupDefinition(CustomerLookup);

            CustomerProductsLookup = new LookupDefinition<CustomerProductLookup, CustomerProduct>(
                CustomerProduct);
            CustomerProductsLookup
                .Include(p => p.Customer)
                .AddVisibleColumnDefinition(
                    p => p.Customer
                    , "Customer"
                    , p => p.CompanyName, 34);

            CustomerProductsLookup
                .Include(p => p.Product)
                .AddVisibleColumnDefinition(
                    p => p.Product
                    , "Product"
                    , p => p.Description, 33);

            CustomerProductsLookup
                .AddVisibleColumnDefinition(
                    p => p.ExpirationDate
                    , "Expiration Date"
                    , p => p.ExpirationDate, 33);

            CustomerProduct.HasLookupDefinition(CustomerProductsLookup);

            OrderLookup = new LookupDefinition<OrderLookup, Order>(Order);
            OrderLookup.InitialOrderByField = Order.GetFieldDefinition(p => p.Id);
            OrderLookup.AddVisibleColumnDefinition(
                p => p.OrderId
                , "Order ID"
                , p => p.OrderId, 15);

            OrderLookup.Include(
                    p => p.Customer)
                .AddVisibleColumnDefinition(
                    p => p.Customer
                    , "Customer"
                    , p => p.CompanyName, 35);

            OrderLookup.AddVisibleColumnDefinition(
                p => p.OrderDate
                , "Order Date"
                , p => p.OrderDate, 25);

            OrderLookup.AddVisibleColumnDefinition(
                p => p.Total
                , "Order\r\nTotal"
                , p => p.Total, 25)
                .HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Right);

            OrderLookup.InitialOrderByField = Order.GetFieldDefinition(p => p.Id);
            
            Order.HasLookupDefinition(OrderLookup);

            OrderDetailLookup = new LookupDefinition<OrderDetailLookup, OrderDetail>(OrderDetail);

            OrderDetailLookup.Include(
                    p => p.Order)
                .AddVisibleColumnDefinition(
                    p => p.OrderId
                    , "Order ID"
                    , p => p.OrderId, 30);

            OrderDetailLookup.Include(
                    p => p.Product)
                .AddVisibleColumnDefinition(
                    p => p.Product
                    , "Product"
                    , p => p.Description, 70);

            OrderDetail.HasLookupDefinition(OrderDetailLookup);

            CustomerComputerLookup = new LookupDefinition<CustomerComputerLookup, CustomerComputer>(
                CustomerComputer);

            CustomerComputerLookup.AddVisibleColumnDefinition(
                p => p.Name
                , "Name"
                , p => p.Name, 99);
            CustomerComputer.HasLookupDefinition(CustomerComputerLookup);

            SupportTicketLookup = new LookupDefinition<SupportTicketLookup, SupportTicket>(SupportTicket);
            SupportTicketLookup
                .AddVisibleColumnDefinition(
                    p => p.TicketId
                    , "Ticket ID"
                    , p => p.TicketId, 20);

            SupportTicketLookup
                .AddVisibleColumnDefinition(
                    p => p.CreateDate
                    , "Date"
                    , p => p.CreateDate, 25);

            SupportTicketLookup
                .Include(p => p.Customer)
                .AddVisibleColumnDefinition(
                    p => p.Customer
                    , "Customer"
                    , p => p.CompanyName, 25);

            SupportTicketLookup
                .AddVisibleColumnDefinition(
                    p => p.MinutesSpent
                    , "Time Spent"
                    , p => p.MinutesSpent, 30);
            SupportTicket.HasLookupDefinition(SupportTicketLookup);
            SupportTicketLookup.InitialOrderByField = SupportTicket.GetFieldDefinition(p => p.Id);
            
            SupportTicketUserLookup =
                new LookupDefinition<SupportTicketUserLookup, SupportTicketUser>(SupportTicketUser);

            SupportTicketUserLookup
                .Include(p => p.SupportTicket)
                .AddVisibleColumnDefinition(
                    p => p.TicketId
                    , "Ticket ID"
                    , p => p.TicketId, 50);

            SupportTicketUserLookup
                .Include(p => p.User)
                .AddVisibleColumnDefinition(
                    p => p.UserName
                    , "User"
                    , p => p.Name, 50);

            SupportTicketUser.HasLookupDefinition(SupportTicketUserLookup);

            CustomerUserLookup =
                new LookupDefinition<CustomerUserLookup, CustomerUser>(CustomerUser);

            CustomerUserLookup
                .Include(p => p.Customer)
                .AddVisibleColumnDefinition(
                    p => p.CustomerName
                    , "Customer"
                    , p => p.CompanyName, 50);

            CustomerUserLookup
                .Include(p => p.User)
                .AddVisibleColumnDefinition(
                    p => p.UserName
                    , "User"
                    , p => p.Name, 50);

            CustomerUser.HasLookupDefinition(CustomerUserLookup);

            SupportTicketErrorLookup = new LookupDefinition<SupportTicketErrorLookup, SupportTicketError>(
                SupportTicketError);
            SupportTicketErrorLookup.Include(p => p.SupportTicket)
                .AddVisibleColumnDefinition(p => p.TicketId
                    , "Support Ticket"
                    , p => p.TicketId, 50);
            SupportTicketErrorLookup.Include(p => p.Error)
                .AddVisibleColumnDefinition(p => p.ErrorId
                    , "Error"
                    , p => p.ErrorId, 50);
            SupportTicketError.HasLookupDefinition(SupportTicketErrorLookup);

            CustomerStatusLookup = new LookupDefinition<CustomerStatusLookup, CustomerStatus>(CustomerStatus);
            CustomerStatusLookup.AddVisibleColumnDefinition(
                p => p.Description
                , "Description"
                , p => p.Description, 99);
            CustomerStatus.HasLookupDefinition(CustomerStatusLookup);

            SupportTicketStatusLookup = new LookupDefinition<SupportTicketStatusLookup, SupportTicketStatus>(SupportTicketStatus);
            SupportTicketStatusLookup.AddVisibleColumnDefinition(
                p => p.Description
                , "Description"
                , p => p.Description, 99);
            SupportTicketStatus.HasLookupDefinition(SupportTicketStatusLookup);

        }

        public LookupDefinition<ProductVersionLookup, ProductVersion> MakeProductVersionLookupDefinition()
        {
            var result = new LookupDefinition<ProductVersionLookup, ProductVersion>(ProductVersions);

            result.AddVisibleColumnDefinition(p => p.Description, "Version", p => p.Description, 34);

            result.AddVisibleColumnDefinition(
                p => p.VersionDate
                , "Release Date"
                , p => p.VersionDate, 33);

            result.Include(
                    p => p.Department)
                .AddVisibleColumnDefinition(
                    p => p.MaxDepartment
                    , "Released To Department"
                    , p => p.Description, 33);

            result.InitialSortColumnDefinition = result.VisibleColumns[0];
            result.InitialOrderByColumn = result.VisibleColumns[1];
            result.InitialOrderByType = OrderByTypes.Descending;

            //var tableDefinition = ProductVersions;
            //var query = new SelectQuery(tableDefinition.TableName);
            //foreach (var fieldDefinition in tableDefinition.FieldDefinitions)
            //{
            //    query.AddSelectColumn(fieldDefinition.FieldName);
            //}
            ////query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Id).FieldName);
            ////query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.ProductId).FieldName);
            ////query.AddSelectColumn(tableDefinition.GetFieldDefinition(p => p.Description).FieldName);
            //query.AddSelectFormulaColumn("VersionDate", MakeVersionDateFormula());
            //query.AddSelectFormulaColumn("MaxDepartment", MakeMaxDepartmentFormula());

            //var sql = DataProcessor.SqlGenerator.GenerateSelectStatement(query);
            //result.HasFromFormula(sql);

            //var column = result.AddVisibleColumnDefinition(p => p.VersionDate
            //    , "VersionDate", "");
            //column.HasDateType(DbDateTypes.DateTime)
            //    .HasDateFormatString(string.Empty)
            //    .HasConvertToLocalTime();

            //column = result.AddVisibleColumnDefinition(p => p.MaxDepartment
            //    , "MaxDepartment", "");

            //result.VisibleColumns[0].UpdatePercentWidth(35);
            //result.VisibleColumns[0].UpdateCaption("Version");
            //result.VisibleColumns[1].UpdatePercentWidth(25);
            //result.VisibleColumns[1].UpdateCaption("Release Date");
            //result.VisibleColumns[2].UpdatePercentWidth(40);
            //result.VisibleColumns[2].UpdateCaption("Released To Department");

            //result.InitialSortColumnDefinition = result.VisibleColumns[0];
            //result.InitialOrderByColumn = result.VisibleColumns[1];
            //result.InitialOrderByType = OrderByTypes.Descending;

            return result;
        }

        //private string MakeVersionDateFormula()
        //{
        //    var result = string.Empty;

        //    var tableDefinition = ProductVersionDepartments;
        //    var field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
        //    field = DataProcessor.SqlGenerator.FormatSqlObject(field);
        //    field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";
        //    var query = new SelectQuery(tableDefinition.TableName);
        //    query.AddSelectFormulaColumn("VersionDate", $"MAX({field})");

        //    field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
        //    field = DataProcessor.SqlGenerator.FormatSqlObject(field);
        //    field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

        //    var targetField = ProductVersions
        //        .GetFieldDefinition(p => p.Id).FieldName;
        //    targetField = DataProcessor.SqlGenerator.FormatSqlObject(targetField);
        //    targetField = $"{DataProcessor.SqlGenerator.FormatSqlObject(ProductVersions.TableName)}.{targetField}";

        //    query.AddWhereItemFormula($"{field} = {targetField}");

        //    result = DataProcessor.SqlGenerator.GenerateSelectStatement(query);

        //    return result;
        //}

        //private string MakeMaxDepartmentFormula()
        //{
        //    var result = string.Empty;
        //    var tableDefinition = ProductVersionDepartments;
        //    var descriptionField = Departments.GetFieldDefinition(p => p.Description).FieldName;
        //    var query = new SelectQuery(tableDefinition.TableName);
        //    var departmentJoin =
        //        query.AddPrimaryJoinTable(JoinTypes.InnerJoin, Departments.TableName)
        //            .AddJoinField(Departments.GetFieldDefinition(p => p.Id).FieldName
        //            , tableDefinition.GetFieldDefinition(p => p.DepartmentId).FieldName);
        //    query.AddSelectColumn(descriptionField, departmentJoin);

        //    var field = tableDefinition.GetFieldDefinition(p => p.VersionId).FieldName;
        //    field = DataProcessor.SqlGenerator.FormatSqlObject(field);
        //    field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

        //    var targetField = ProductVersions
        //        .GetFieldDefinition(p => p.Id).FieldName;
        //    targetField = DataProcessor.SqlGenerator.FormatSqlObject(targetField);
        //    targetField = $"{DataProcessor.SqlGenerator.FormatSqlObject(ProductVersions.TableName)}.{targetField}";

        //    query.AddWhereItemFormula($"{field} = {targetField}");

        //    field = tableDefinition.GetFieldDefinition(p => p.ReleaseDateTime).FieldName;
        //    field = DataProcessor.SqlGenerator.FormatSqlObject(field);
        //    field = $"{DataProcessor.SqlGenerator.FormatSqlObject(tableDefinition.TableName)}.{field}";

        //    query.AddWhereItemFormula($"{field} = ({MakeVersionDateFormula()})");

        //    result = DataProcessor.SqlGenerator.GenerateSelectStatement(query);
        //    return result;
        //}


        protected override void SetupModel()
        {
            //var advancedField = AdvancedFinds.GetFieldDefinition(p => p.Id);
            //var chartAdvFindField = DevLogixChartBars.GetFieldDefinition(p => p.AdvancedFindId);
            //chartAdvFindField.SetParentField(advancedField, advancedField.PropertyName);

            Groups.PriorityLevel = 100;
            Groups.GetFieldDefinition(p => p.Rights).DoSkipPrint().IsMemo();

            ErrorStatuses.PriorityLevel = 100;
            ErrorPriorities.PriorityLevel = 100;
            
            Departments.PriorityLevel = 200;
            Departments.GetFieldDefinition(p => p.Description)
                .HasDescription("Name");
            DevLogixCharts.PriorityLevel = 200;

            AdvancedFinds.PriorityLevel = 250;
            DevLogixChartBars.PriorityLevel = 300;

            Products.PriorityLevel = 300;
            Products.GetFieldDefinition(p => p.Notes).IsMemo();

            Users.PriorityLevel = 300;
            //Users.GetFieldDefinition(p => p.SupervisorId).DoesAllowRecursion(false);
            Users.GetFieldDefinition(p => p.Rights).DoSkipPrint().IsMemo();
            Users.GetFieldDefinition(p => p.Notes).IsMemo();
            Users.GetFieldDefinition(p => p.Password).DoSkipPrint();
            Users.GetFieldDefinition(p => p.ClockDate).DoConvertToLocalTime().HasDateType(DbDateTypes.DateTime);
            Users.GetFieldDefinition(p => p.HourlyRate).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Users.GetFieldDefinition(p => p.ClockOutReason).IsEnum<ClockOutReasons>();
            Users.GetFieldDefinition(p => p.TotalSales)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            Users.GetFieldDefinition(p => p.CustomerMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Users.GetFieldDefinition(p => p.BillableProjectsMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Users.GetFieldDefinition(p => p.NonBillableProjectsMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Users.GetFieldDefinition(p => p.TestingOutlinesMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Users.GetFieldDefinition(p => p.SupportTicketsMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Users.GetFieldDefinition(p => p.MonthlySalesQuota)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            UseerMonthlySales.PriorityLevel = 400;
            UseerMonthlySales.GetFieldDefinition(p => p.MonthEnding)
                .HasDateType(DbDateTypes.DateOnly);
            UseerMonthlySales.GetFieldDefinition(p => p.TotalSales)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            UseerMonthlySales.GetFieldDefinition(p => p.Quota)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            UseerMonthlySales.GetFieldDefinition(p => p.Difference)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);


            UserTracker.PriorityLevel = 300;
            UserTracker.GetFieldDefinition(p => p.RefreshType).IsEnum<RefreshRate>();

            UserTrackerUsers.PriorityLevel = 400;

            UsersTimeOff.PriorityLevel = 400;
            UsersTimeOff.GetFieldDefinition(p => p.StartDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            UsersTimeOff.GetFieldDefinition(p => p.EndDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();


            UsersGroups.PriorityLevel = 400;

            Products.GetFieldDefinition(p => p.Price).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Products.GetFieldDefinition(p => p.Revenue).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Products.GetFieldDefinition(p => p.Cost).HasDecimalFieldType(DecimalFieldTypes.Currency);

            ProductVersions.PriorityLevel = 400;
            ProductVersions.GetFieldDefinition(p => p.Notes).IsMemo();
            ProductVersions.GetFieldDefinition(p => p.ArchiveDateTime)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            ProductVersions.GetFieldDefinition(p => p.VersionDate)
                .HasDateType(DbDateTypes.Millisecond)
                .DoConvertToLocalTime();

            ProductVersionDepartments.PriorityLevel = 500;
            ProductVersionDepartments.SetHeaderEntity<ProductVersion>();
            ProductVersionDepartments.GetFieldDefinition(p => p.ReleaseDateTime)
                .HasDateType(DbDateTypes.DateTime);

            TestingOutlines.PriorityLevel = 300;
            TestingOutlines.GetFieldDefinition(p => p.DueDate).HasDateType(DbDateTypes.DateTime).DoConvertToLocalTime(true);
            TestingOutlines.GetFieldDefinition(p => p.Notes).IsMemo();
            TestingOutlines.GetFieldDefinition(p => p.PercentComplete).HasDecimalFieldType(DecimalFieldTypes.Percent);

            TestingOutlineDetails.PriorityLevel = 500;

            Errors.PriorityLevel = 450;
            Errors.GetFieldDefinition(p => p.ErrorDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            Errors.GetFieldDefinition(p => p.Description)
                .IsMemo();
            Errors.GetFieldDefinition(p => p.Resolution)
                .IsMemo();
            Errors.GetFieldDefinition(p => p.FoundByUserId).CanSetNull(false);
            Errors.GetFieldDefinition(p => p.ErrorId).IsGeneratedKey();

            ErrorDevelopers.PriorityLevel = 500;
            ErrorDevelopers.GetFieldDefinition(p => p.DateFixed)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            ErrorTesters.PriorityLevel = 500;
            ErrorTesters.GetFieldDefinition(p => p.DateChanged)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            TestingTemplates.PriorityLevel = 100;
            TestingTemplates.GetFieldDefinition(p => p.Notes).IsMemo();

            TestingOutlineTemplates.PriorityLevel = 600;

            TimeClocks.PriorityLevel = 20000;
            TimeClocks.GetFieldDefinition(p => p.Name).IsGeneratedKey();
            TimeClocks.GetFieldDefinition(p => p.Notes).IsMemo();
            TimeClocks.GetFieldDefinition(p => p.PunchInDate).HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            TimeClocks.GetFieldDefinition(p => p.PunchOutDate).HasDateType(DbDateTypes.DateTime).DoConvertToLocalTime();
            TimeClocks.GetFieldDefinition(p => p.ErrorId)
                .CanSetNull(false);
            TimeClocks.GetFieldDefinition(p => p.MinutesSpent).HasSearchForHostId(TimeSpentHostId);

            Projects.PriorityLevel = 400;
            Projects.GetFieldDefinition(p => p.Notes).IsMemo();
            Projects.GetFieldDefinition(p => p.Deadline).HasDateType(DbDateTypes.DateTime);
            Projects.GetFieldDefinition(p => p.OriginalDeadline).HasDateType(DbDateTypes.DateTime);
            Projects.GetFieldDefinition(p => p.StartDateTime).HasDescription("Start Date");

            LaborParts.PriorityLevel = 100;
            LaborParts.GetFieldDefinition(p => p.Comment).IsMemo();

            MaterialParts.GetFieldDefinition(p => p.Cost).HasDecimalFieldType(DecimalFieldTypes.Currency);
            MaterialParts.PriorityLevel = 100;
            MaterialParts.GetFieldDefinition(p => p.Comment).IsMemo();

            ProjectMaterials.GetFieldDefinition(p => p.Cost).HasDecimalFieldType(DecimalFieldTypes.Currency);

            ProjectMaterials.PriorityLevel = 500;

            ProjectMaterialParts.PriorityLevel = 600;

            ProjectMaterialHistory.PriorityLevel = 600;
            ProjectMaterialHistory.GetFieldDefinition(p => p.Date).HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            ProjectMaterialHistory.GetFieldDefinition(p => p.Cost).HasDecimalFieldType(DecimalFieldTypes.Currency);

            ProjectMaterialParts.GetFieldDefinition(p => p.Cost).HasDecimalFieldType(DecimalFieldTypes.Currency);

            ProjectTasks.PriorityLevel = 500;
            ProjectTasks.GetFieldDefinition(p => p.Notes).IsMemo();
            ProjectTasks.GetFieldDefinition(p => p.PercentComplete).HasDecimalFieldType(DecimalFieldTypes.Percent);
            ProjectTasks.GetFieldDefinition(p => p.MinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);

            ProjectTaskLaborParts.PriorityLevel = 600;
            ProjectTaskLaborParts.GetFieldDefinition(p => p.PercentComplete)
                .HasDecimalFieldType(DecimalFieldTypes.Percent);

            ProjectTaskDependency.PriorityLevel = 600;
            //ProjectTaskDependency.GetFieldDefinition(p => p.ProjectTaskId).DoesAllowRecursion(false);
            //ProjectTaskDependency.GetFieldDefinition(p => p.DependsOnProjectTaskId).DoesAllowRecursion(false);

            ProjectUsers.PriorityLevel = 500;

            Territory.PriorityLevel = 400;

            TimeZone.PriorityLevel = 100;

            CustomerStatus.PriorityLevel = 100;

            Customer.PriorityLevel = 500;
            Customer.GetFieldDefinition(p => p.Notes).IsMemo();
            Customer.GetFieldDefinition(p => p.SupportMinutesPurchased)
                .HasSearchForHostId(TimeSpentHostId);
            Customer.GetFieldDefinition(p => p.SupportMinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Customer.GetFieldDefinition(p => p.SupportCost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            Customer.GetFieldDefinition(p => p.MinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            Customer.GetFieldDefinition(p => p.MinutesCost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            Customer.GetFieldDefinition(p => p.StatusId)
                .CanSetNull(false);
            Customer.GetFieldDefinition(p => p.LastContactDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime()
                .HasDescription("Last Sales Contact Date");

            CustomerProduct.PriorityLevel = 600;
            CustomerProduct.GetFieldDefinition(
                p => p.ExpirationDate).HasDateType(DbDateTypes.DateOnly)
                .DoConvertToLocalTime(false);

            Order.PriorityLevel = 700;
            Order.GetFieldDefinition(p => p.OrderId).IsGeneratedKey();
            Order.GetFieldDefinition(p => p.OrderDate).HasDateType(DbDateTypes.DateTime);
            Order.GetFieldDefinition(p => p.ShippedDate).HasDateType(DbDateTypes.DateTime);
            Order.GetFieldDefinition(p => p.SubTotal).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Order.GetFieldDefinition(p => p.Freight).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Order.GetFieldDefinition(p => p.TotalDiscount).HasDecimalFieldType(DecimalFieldTypes.Currency);
            Order.GetFieldDefinition(p => p.Total).HasDecimalFieldType(DecimalFieldTypes.Currency);

            OrderDetail.PriorityLevel = 800;
            OrderDetail.GetFieldDefinition(p => p.UnitPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            OrderDetail.GetFieldDefinition(p => p.ExtendedPrice)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            OrderDetail.GetFieldDefinition(p => p.Discount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            CustomerUser.PriorityLevel = 700;

            CustomerComputer.PriorityLevel = 700;
            CustomerComputer.GetFieldDefinition(p => p.Notes).IsMemo();
            CustomerComputer.GetFieldDefinition(p => p.Speed)
                .HasSearchForHostId(SpeedHostId);

            CustomerComputer.GetFieldDefinition(p => p.HardDriveFree)
                .HasSearchForHostId(MemoryHostId);

            CustomerComputer.GetFieldDefinition(p => p.HardDriveSize)
                .HasSearchForHostId(MemoryHostId);

            CustomerComputer.GetFieldDefinition(p => p.RamSize)
                .HasSearchForHostId(MemoryHostId);

            SupportTicketStatus.PriorityLevel = 100;
            SupportTicket.PriorityLevel = 800;
            SupportTicket.GetFieldDefinition(p => p.TicketId)
                .IsGeneratedKey();
            SupportTicket.GetFieldDefinition(p => p.CreateDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
            SupportTicket.GetFieldDefinition(p => p.Notes)
                .IsMemo();
            SupportTicket.GetFieldDefinition(p => p.Cost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);
            SupportTicket.GetFieldDefinition(p => p.MinutesSpent)
                .HasSearchForHostId(TimeSpentHostId);
            SupportTicket.GetFieldDefinition(p => p.StatusId)
                .CanSetNull(false);


            SupportTicketUser.PriorityLevel = 900;
            SupportTicketUser.GetFieldDefinition(p => p.Cost)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            SupportTicketError.PriorityLevel = 900;
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
