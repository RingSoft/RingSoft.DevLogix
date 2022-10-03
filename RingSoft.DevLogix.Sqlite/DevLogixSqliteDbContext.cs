using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess;

namespace RingSoft.DevLogix.Sqlite
{
    public class DevLogixSqliteDbContext : DbContext, IDevLogixDbContext
    {
        public DbSet<AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public DbContext DbContext => this;
        public bool IsDesignTime { get; set; }

        private static DevLogixLookupContext _lookupContext;

        public DevLogixSqliteDbContext()
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDesignTime)
                optionsBuilder.UseSqlite("DataSource=C:\\");
            else
                optionsBuilder.UseSqlite(_lookupContext.SqliteDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataAccessGlobals.ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
