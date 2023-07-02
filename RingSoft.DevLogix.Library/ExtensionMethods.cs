using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using System.Linq;

namespace RingSoft.DevLogix.Library
{
    public static class ExtensionMethods
    {
        public static bool HasRight(this TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return AppGlobals.Rights.HasRight(tableDefinition, rightType);
        }

        public static bool HasSpecialRight(this TableDefinitionBase tableDefinition, int rightType)
        {
            return AppGlobals.Rights.HasSpecialRight(tableDefinition, rightType);
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

        public static string GetHolidayText(this DateTime date)
        {
            var result = string.Empty;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<SystemPreferencesHolidays>();
            if (table != null)
            {
                var holiday = table.FirstOrDefault(p => p.Date == date);
                if (holiday != null)
                {
                    result = holiday.Name;
                }
            }
            return result;
        }
    }
}
