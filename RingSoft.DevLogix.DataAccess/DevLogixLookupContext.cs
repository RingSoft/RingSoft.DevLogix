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
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess
{

    public class DevLogixLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public override DbDataProcessor DataProcessor => _dbDataProcessor;
        protected override DbContext DbContext => _dbContext;
        public LookupContextBase Context { get; }

        public TableDefinition<SystemMaster> SystemMaster { get; set; }
        public TableDefinition<User> Users { get; set; }

        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        public LookupDefinition<UserLookup, User> UserLookup { get; set; }

        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; }
        public SqlServerDataProcessor SqlServerDataProcessor { get; }
        

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
            UserLookup.AddVisibleColumnDefinition(p => p.Type, "Type", p => p.Type, 30);
            Users.HasLookupDefinition(UserLookup);
        }

        protected override void SetupModel()
        {
            
        }
    }
}
