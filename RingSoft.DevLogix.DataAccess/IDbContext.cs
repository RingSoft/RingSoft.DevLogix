﻿using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDbContext
    {
        bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool Commit(string message);

        void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class;

        void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class;

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class;
    }
}
