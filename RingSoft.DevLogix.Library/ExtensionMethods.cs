using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library
{
    public static class ExtensionMethods
    {
        public static bool HasRight(this TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return AppGlobals.Rights.HasRight(tableDefinition, rightType);
        }

        public static bool IsSupervisor(this User user, IQueryable<User> userQuery = null)
        {
            if (user == null)
            {
                return false;
            }
            if (user.SupervisorId.HasValue)
            {
                if (userQuery == null)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    userQuery = context.GetTable<User>();
                }

                var supervisor = userQuery.FirstOrDefault(p => p.Id == user.SupervisorId.Value);
                if (supervisor != null)
                {
                    if (supervisor.Id == AppGlobals.LoggedInUser.Id)
                    {
                        return true;
                    }
                    return supervisor.IsSupervisor(userQuery);
                }
            }
            return false;
        }
    }
}
