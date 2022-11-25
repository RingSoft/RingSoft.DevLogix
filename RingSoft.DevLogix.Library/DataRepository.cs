using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine.Annotations;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.SqlServer.Migrations;
using SystemMaster = RingSoft.DevLogix.DataAccess.Model.SystemMaster;
using User = RingSoft.DevLogix.DataAccess.Model.User;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();

        [CanBeNull]
        SystemMaster GetSystemMaster();

        bool UsersExist();

        User GetUser(int userId);

        User GetUser(string username);

        bool SaveUser(User user);

        bool DeleteUser(int userId);

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class;
    }

    public class DataRepository : IDataRepository
    {
        public IDbContext GetDataContext()
        {
            return AppGlobals.GetNewDbContext();
        }

        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }
        
        public bool UsersExist()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.Users.Any();
        }

        public User GetUser(int userId)
        {
            var context = AppGlobals.GetNewDbContext();
            return  context.Users.FirstOrDefault(p => p.Id == userId);
        }

        public User GetUser(string username)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.Users.FirstOrDefault(p => p.Name == username);

        }

        public bool SaveUser(User user)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.Users, user,
                $"Saving User '{user.Name}.'");
        }

        public bool DeleteUser(int userId)
        {
            var context = AppGlobals.GetNewDbContext();
            var user = context.Users.FirstOrDefault(f => f.Id == userId);
            return user != null && context.DbContext.DeleteEntity(context.Users, user,
                $"Deleting User '{user.Name}'");
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            var context = AppGlobals.GetNewDbContext();
            var dbSet = context.DbContext.Set<TEntity>();
            return dbSet;
        }

    }
}
