using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Configurations;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DevLogix.DataAccess.Configurations.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Configurations.QualityAssurance;
using Microsoft.EntityFrameworkCore.Migrations;
using RingSoft.DevLogix.DataAccess.Configurations.UserManagement;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.DataAccess
{
    public class DataAccessGlobals
    {
        public static DbContext DbContext { get; set; }

        public static void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DevLogixChartConfiguration());
            modelBuilder.ApplyConfiguration(new DevLogixChartBarConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTimeOffConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new TimeClockConfiguration());
            modelBuilder.ApplyConfiguration(new UserTrackerConfiguration());
            modelBuilder.ApplyConfiguration(new UserTrackerUserConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorPriorityConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductVersionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductVersionDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorDeveloperConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorQaConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectUserConfiguration());
            modelBuilder.ApplyConfiguration(new LaborPartConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskLaborPartConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialPartConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMaterialPartConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMaterialHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SystemPreferencesConfiguration());
            modelBuilder.ApplyConfiguration(new SystemPreferencesHolidaysConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskDependenciesConfiguration());
            modelBuilder.ApplyConfiguration(new TestingTemplateConfiguration());

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);
        }

        public static void UpgradeProjects(MigrationBuilder migrationBuilder)
        {
            var userSet = DbContext.Set<User>();
            var firstUser = userSet.FirstOrDefault();
            if (firstUser != null)
            {
                migrationBuilder.UpdateData(
                    table: "Projects",
                    keyColumn: "ManagerId",
                    keyValue: null,
                    column: "ManagerId",
                    value: firstUser.Id);
            }
        }

        public static void SetupSysPrefs()
        {
            var sysPrefsSet = DbContext.Set<SystemPreferences>();
            var sysPrefsRecord = sysPrefsSet.FirstOrDefault();
            if (sysPrefsRecord == null)
            {
                sysPrefsRecord = new SystemPreferences();
                SaveEntity(sysPrefsRecord, "Creating New System Preferences Record");
            }
        }

        public static bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            if (!DbContext.SaveNoCommitEntity(DbContext.Set<TEntity>(), entity, message))
                return false;

            return true;
        }

        public static bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.SaveEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.DeleteEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.AddNewNoCommitEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool Commit(string message)
        {
            var result = DbContext.SaveEfChanges(message);

            return result;
        }

        public static void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class
        {
            var dbSet = DbContext.Set<TEntity>();

            dbSet.RemoveRange(listToRemove);
        }

        public static void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        {
            var dbSet = DbContext.Set<TEntity>();

            dbSet.AddRange(listToAdd);
        }

        public static bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.DeleteNoCommitEntity(DbContext.Set<TEntity>(), entity, message);
        }
    }
}
