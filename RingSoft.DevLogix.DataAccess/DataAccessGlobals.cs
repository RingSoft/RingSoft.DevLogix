using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Configurations;
using System.Collections.Generic;

namespace RingSoft.DevLogix.DataAccess
{
    public class DataAccessGlobals
    {
        public static DbContext DbContext { get; set; }

        public static void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorPriorityConfiguration());

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);
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

        public static void RemoveRange<TEntity>(List<TEntity> listToRemove) where TEntity : class
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
