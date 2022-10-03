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
    public class DevLogixLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public override DbDataProcessor DataProcessor { get; }
        protected override DbContext DbContext => _dbContext;
        public LookupContextBase Context { get; }
        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; }

        

        private DbContext _dbContext;


        public DevLogixLookupContext()
        {
            SqliteDataProcessor = new SqliteDataProcessor();
        }

        public void Initialize(IDevLogixDbContext dbContext)
        {
            _dbContext = dbContext.DbContext;
            SystemGlobals.AdvancedFindLookupContext = this;
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();

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
