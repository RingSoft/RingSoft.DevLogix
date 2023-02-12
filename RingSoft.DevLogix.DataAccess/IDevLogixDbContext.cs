﻿using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDevLogixDbContext : IAdvancedFindDbContextEfCore, IDbContext
    {
        DbContext DbContext { get; }

        DbSet<SystemMaster> SystemMaster { get; set; }
        DbSet<DevLogixChart> DevLogixCharts { get; set; }
        DbSet<DevLogixChartBar> DevLogixChartsBars { get; set;}

        DbSet<User> Users { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<UsersGroup> UsersGroups { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<TimeClock> TimeClocks { get; set; }

        DbSet<ErrorStatus> ErrorStatuses { get; set; }
        DbSet<ErrorPriority> ErrorPriorities { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductVersion> ProductVersions { get; set; }
        DbSet<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        DbSet<Error> Errors { get; set; }
        DbSet<ErrorDeveloper> ErrorDevelopers { get; set; }
        DbSet<ErrorQa> ErrorTesters { get; set; }

        void SetLookupContext(DevLogixLookupContext lookupContext);
    }
}
