﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.Sqlite;

namespace RingSoft.DevLogix.DbPlatform
{
    public enum DbPlatforms
    {
        Sqlite = 0,
        SqlServer = 1
    }
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
                    throw new ArgumentOutOfRangeException();
                    break;
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
                    throw new ArgumentOutOfRangeException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
