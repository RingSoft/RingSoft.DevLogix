using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine.Annotations;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        bool UsersExist();

        User GetUser(int userId);

        User GetUser(string username);

        bool SaveUser(User user);

        bool DeleteUser(int userId);

        Group GetGroup(int groupId);

        bool SaveGroup(Group group, List<UsersGroup> usersGroups);

        bool DeleteGroup(int groupId);

        ErrorStatus GetErrorStatus(int errorStatusId);

        bool SaveErrorStatus(ErrorStatus errorStatus);

        bool DeleteErrorStatus(int errorStatusId);
    }
    public class DataRepository : IDataRepository
    {
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

        public Group GetGroup(int groupId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.Groups.Include(p => p.UserGroups)
                .FirstOrDefault(p => p.Id == groupId);
        }

        public bool SaveGroup(Group group, List<UsersGroup> usersGroups)
        {
            var context = AppGlobals.GetNewDbContext();
            if (context.DbContext.SaveEntity(context.Groups, group,
                    $"Saving Group '{group.Name}.'"))
            {
                context.UsersGroups.AddRange(usersGroups);
                return context.DbContext.SaveEfChanges("Saving UsersGroups");
            }

            return false;
        }

        public bool DeleteGroup(int groupId)
        {
            var context = AppGlobals.GetNewDbContext();
            var group = context.Groups.FirstOrDefault(f => f.Id == groupId);
            if (group != null)
            {
                var userGroups = context.UsersGroups.Where(p => p.GroupId == groupId).ToList();
                context.UsersGroups.RemoveRange(userGroups);

            }

            if (group != null && context.DbContext.DeleteNoCommitEntity(context.Groups, group,
                    $"Deleting Group '{group.Name}'"))
            {
                return context.DbContext.SaveEfChanges($"Deleting Group '{group.Name}'");
            }
            return false;
        }

        public ErrorStatus GetErrorStatus(int errorStatusId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.ErrorStatuses.FirstOrDefault(p => p.Id == errorStatusId);
        }

        public bool SaveErrorStatus(ErrorStatus errorStatus)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.ErrorStatuses, errorStatus,
                $"Saving Error Status '{errorStatus.Description}.'");
        }

        public bool DeleteErrorStatus(int errorStatusId)
        {
            var context = AppGlobals.GetNewDbContext();
            var errorStatus = context.ErrorStatuses.FirstOrDefault(f => f.Id == errorStatusId);
            return errorStatus != null && context.DbContext.DeleteEntity(context.ErrorStatuses, errorStatus,
                $"Deleting Error Status '{errorStatus.Description}'");
        }
    }
}
