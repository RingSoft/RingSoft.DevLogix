using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DevLogix.Library
{
    public static class ExtensionMethods
    {
        public static bool HasRight(this TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return AppGlobals.Rights.HasRight(tableDefinition, rightType);
        }

        //public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        //    where T : class
        //{
        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query,
        //            (current, include) => current.Include(include));
        //    }

        //    return query;
        //}
    }
}
