using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.SqlServer;

namespace RingSoft.DevLogix.DbPlatform
{
    public class DbPlatform
    {
        public DevLogixSqliteDbContext SqliteDbContext { get; private set; }

        public static DbPlatforms DevLogixPlatform { get; set; } = DbPlatforms.Sqlite;

        public IDevLogixDbContext GetNewDbContext(DevLogixLookupContext lookupContext)
        {
            switch (DevLogixPlatform)
            {
                case DbPlatforms.Sqlite:
                    return new DevLogixSqliteDbContext(lookupContext);
                
                case DbPlatforms.SqlServer:
                    return new DevLogixSqlServerDbContext(lookupContext);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDevLogixDbContext GetNewDbContext()
        {
            switch (DevLogixPlatform)
            {
                case DbPlatforms.Sqlite:
                    return new DevLogixSqliteDbContext();

                case DbPlatforms.SqlServer:
                    return new DevLogixSqlServerDbContext();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
