using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Sqlite
{
    public class DevLogixSqliteDbContext : DbContext, IDevLogixDbContext
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }

        public bool IsDesignTime { get; set; }

        private static DevLogixLookupContext _lookupContext;

        public DevLogixSqliteDbContext()
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();
            DataAccessGlobals.DbContext = this;
        }
        public DevLogixSqliteDbContext(DevLogixLookupContext lookupContext)
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            _lookupContext = lookupContext;
        }


        public DbContext GetDbContextEf()
        {
            return this;
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new DevLogixSqliteDbContext();
        }

        public DbSet<RecordLock> RecordLocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDesignTime)
                optionsBuilder.UseSqlite("DataSource=C:\\");
            else
                optionsBuilder.UseSqlite(_lookupContext.SqliteDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public void SetLookupContext(DevLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            DbConstants.ConstantGenerator = new SqliteDbConstants();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataAccessGlobals.ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DataAccessGlobals.SaveNoCommitEntity(entity, message);
        }

        public bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DataAccessGlobals.SaveEntity(entity, message);
        }

        public bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DataAccessGlobals.DeleteEntity(entity, message);
        }

        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DataAccessGlobals.DeleteNoCommitEntity(entity, message);
        }

        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DataAccessGlobals.AddNewNoCommitEntity(entity, message);
        }

        public bool Commit(string message)
        {
            return DataAccessGlobals.Commit(message);
        }

        public void RemoveRange<TEntity>(List<TEntity> listToRemove) where TEntity : class
        {
            DataAccessGlobals.RemoveRange(listToRemove);
        }

        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        {
            DataAccessGlobals.AddRange(listToAdd);
        }
    }
}
