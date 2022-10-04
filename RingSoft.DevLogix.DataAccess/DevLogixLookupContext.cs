using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DevLogix.DataAccess
{
    public enum DbPlatforms
    {
        Sqlite = 0,
        SqlServer = 1
    }

    public class DevLogixLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public override DbDataProcessor DataProcessor => _dbDataProcessor;
        protected override DbContext DbContext => _dbContext;
        public LookupContextBase Context { get; }
        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; }
        public SqlServerDataProcessor SqlServerDataProcessor { get; }
        

        private DbContext _dbContext;
        private DbDataProcessor _dbDataProcessor;

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
            _dbContext = dbContext.DbContext;
            SystemGlobals.AdvancedFindLookupContext = this;
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();

            SetProcessor(dbPlatform);
            Initialize();
        }
        protected override void InitializeLookupDefinitions()
        {
            
        }

        protected override void SetupModel()
        {
            
        }
    }
}
