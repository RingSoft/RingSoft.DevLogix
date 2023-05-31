﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using IDbContext = RingSoft.DbLookup.IDbContext;

namespace RingSoft.DevLogix.SqlServer
{
    public class DevLogixSqlServerDbContext : DbContextEfCore, IDevLogixDbContext
    {
        //public DbSet<AdvancedFind> AdvancedFinds { get; set; }
        //public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        //public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }
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
        public DbSet<TestingTemplate> TestingTemplates { get; set; }

        public bool IsDesignTime { get; set; }

        private static DevLogixLookupContext _lookupContext;

        public DevLogixSqlServerDbContext()
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            var processor = new AdvancedFindDataProcessorEfCore();
            SystemGlobals.AdvancedFindDbProcessor = processor;
            SystemGlobals.DataRepository = processor;

            DataAccessGlobals.DbContext = this;
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

        public override IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new DevLogixSqlServerDbContext();
        }

        //public DbSet<RecordLock> RecordLocks { get; set; }

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
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            DataAccessGlobals.ConfigureModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public void SetLookupContext(DevLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
        }

        //public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return DataAccessGlobals.SaveNoCommitEntity(entity, message);
        //}

        //public bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return DataAccessGlobals.SaveEntity(entity, message);
        //}

        //public bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return DataAccessGlobals.DeleteEntity(entity, message);
        //}

        //public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return DataAccessGlobals.DeleteNoCommitEntity(entity, message);
        //}

        //public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return DataAccessGlobals.AddNewNoCommitEntity(entity, message);
        //}

        //public bool Commit(string message)
        //{
        //    return DataAccessGlobals.Commit(message);
        //}

        //public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class
        //{
        //    DataAccessGlobals.RemoveRange(listToRemove);
        //}

        //public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        //{
        //    DataAccessGlobals.AddRange(listToAdd);
        //}

        //IQueryable<TEntity> DataAccess.IDbContext.GetTable<TEntity>()
        //{
        //    return Set<TEntity>();
        //}

        //IQueryable<TEntity> IDbContext.GetTable<TEntity>()
        //{
        //    return Set<TEntity>();
        //}
    }
}
