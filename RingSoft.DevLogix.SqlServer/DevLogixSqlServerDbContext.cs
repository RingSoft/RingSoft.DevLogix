using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.SqlServer
{
    public class DevLogixSqlServerDbContext : DbContext, IDevLogixDbContext
    {
        public DbSet<AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public DbContext DbContext => this;
        public DbSet<SystemMaster> SystemMaster { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UsersGroup> UsersGroups { get; set; }
        public DbSet<ErrorStatus> ErrorStatuses { get; set; }
        public DbSet<ErrorPriority> ErrorPriorities { get; set; }

        public bool IsDesignTime { get; set; }

        private static DevLogixLookupContext _lookupContext;

        public DevLogixSqlServerDbContext()
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();

        }

        public DevLogixSqlServerDbContext(DevLogixLookupContext lookupContext)
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            _lookupContext = lookupContext;

        }

        public DbContext GetDbContextEf()
        {
            return this;
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new DevLogixSqlServerDbContext();
        }

        public DbSet<RecordLock> RecordLocks { get; set; }

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
                optionsBuilder.UseSqlServer(_lookupContext.SqlServerDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataAccessGlobals.ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public void SetLookupContext(DevLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
        }

    }
}
