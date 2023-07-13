using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.SqlServer
{
    public class DevLogixSqlServerDbContext : DbContextEfCore, IDevLogixDbContext
    {
        public DbContext DbContext => this;
        public DbSet<SystemMaster> SystemMaster { get; set; }
        public DbSet<DevLogixChart> DevLogixCharts { get; set; }
        public DbSet<DevLogixChartBar> DevLogixChartsBars { get; set; }
        public DbSet<SystemPreferences> SystemPreferences { get; set; }
        public DbSet<SystemPreferencesHolidays> SystemPreferencesHolidays { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTimeOff> UsersTimeOff { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UsersGroup> UsersGroups { get; set; }
        public DbSet<TimeClock> TimeClocks { get; set; }
        public DbSet<UserTracker> UserTracker { get; set; }
        public DbSet<UserTrackerUser> UserTrackerUsers { get; set; }
        public DbSet<UserMonthlySales> UserMonthlySales { get; set; }
        public DbSet<ErrorStatus> ErrorStatuses { get; set; }
        public DbSet<ErrorPriority> ErrorPriorities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<ErrorDeveloper> ErrorDevelopers { get; set; }
        public DbSet<ErrorQa> ErrorTesters { get; set; }
        public DbSet<ErrorUser> ErrorUsers { get; set; }
        public DbSet<TestingTemplateItem> TestingTemplateItems { get; set; }
        public DbSet<TestingOutline> TestingOutlines { get; set; }
        public DbSet<TestingOutlineDetails> TestingOutlineDetails { get; set; }
        public DbSet<TestingOutlineTemplate> TestingOutlineTemplates { get; set; }
        public DbSet<TestingOutlineCost> TestingOutlineCosts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<LaborPart> LaborParts { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectTaskLaborPart> ProjectTaskLaborParts { get; set; }
        public DbSet<MaterialPart> MaterialParts { get; set; }
        public DbSet<ProjectMaterial> ProjectMaterials { get; set; }
        public DbSet<ProjectMaterialPart> ProjectMaterialParts { get; set; }
        public DbSet<ProjectMaterialHistory> ProjectMaterialHistory { get; set; }
        public DbSet<ProjectTaskDependency> ProjectTaskDependency { get; set; }
        public DbSet<TimeZone> TimeZone { get; set; }
        public DbSet<Territory> Territory { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerProduct> CustomerProduct { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<CustomerComputer> CustomerComputer { get; set; }
        public DbSet<SupportTicket> SupportTicket { get; set; }
        public DbSet<SupportTicketUser> SupportTicketUser { get; set; }
        public DbSet<CustomerUser> CustomerUser { get; set; }
        public DbSet<SupportTicketError> SupportTicketError { get; set; }
        public DbSet<CustomerStatus> CustomerStatus { get; set; }
        public DbSet<SupportTicketStatus> SupportTicketStatus { get; set; }
        public DbSet<TestingTemplate> TestingTemplates { get; set; }

        public bool IsDesignTime { get; set; }

        private static string? _connectionString;
        public static string? ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    return _lookupContext.SqlServerDataProcessor.ConnectionString;
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }


        private static DevLogixLookupContext _lookupContext;

        public DevLogixSqlServerDbContext()
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();

            DataAccessGlobals.DbContext = this;
        }

        public DevLogixSqlServerDbContext(DevLogixLookupContext lookupContext)
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            _lookupContext = lookupContext;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDesignTime)
            {
                var sqlProcessor = new SqlServerDataProcessor();
                sqlProcessor.Server = "localhost\\SQLEXPRESS";
                sqlProcessor.Database = "RSDevLogixTemp";
                sqlProcessor.SecurityType = SecurityTypes.WindowsAuthentication;
                optionsBuilder.UseSqlServer(sqlProcessor.ConnectionString);
            }
            else
                optionsBuilder.UseSqlServer(ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            DataAccessGlobals.ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public void SetLookupContext(DevLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return new DevLogixSqlServerDbContext();
        }

        public override void SetProcessor(DbDataProcessor processor)
        {
            
        }

        public override void SetConnectionString(string? connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
