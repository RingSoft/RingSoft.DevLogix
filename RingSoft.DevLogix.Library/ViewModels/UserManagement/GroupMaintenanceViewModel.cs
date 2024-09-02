using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using Group = RingSoft.DevLogix.DataAccess.Model.Group;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IGroupView : IDbMaintenanceView
    {
        public string GetRights();

        public void LoadRights(string rightsString);

        public void ResetRights();
    }

    public class GroupMaintenanceViewModel : DbMaintenanceViewModel<Group>
    {
        public override TableDefinition<Group> TableDefinition => AppGlobals.LookupContext.Groups;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged();
            }
        }

        private bool _rightsChanged;

        public bool RightsChanged
        {
            get => _rightsChanged;
            set
            {
                if (_rightsChanged == value)
                    return;

                _rightsChanged = value;
                OnPropertyChanged();
            }
        }

        private GroupsUsersManager _usersManager;

        public GroupsUsersManager UsersManager
        {
            get => _usersManager;
            set
            {
                if (_usersManager == value)
                {
                    return;
                }
                _usersManager = value;
                OnPropertyChanged();
            }
        }


        public new IGroupView View { get; private set; }

        protected override void Initialize()
        {
            View = base.View as IGroupView;
            if (View == null)
                throw new Exception($"Group View interface must be of type '{nameof(IGroupView)}'.");

            UsersManager = new GroupsUsersManager(this);

            base.Initialize();
        }


        protected override void PopulatePrimaryKeyControls(Group newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override Group GetEntityFromDb(Group newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<Group> query = AppGlobals.DataRepository.GetDataContext().GetTable<Group>();
            query = query.Include(p => p.UserGroups);
            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);

            return result;
        }

        protected override void LoadFromEntity(Group entity)
        {
            Name = entity.Name;
            View.LoadRights(entity.Rights.Decrypt());
            UsersManager.LoadGrid(entity.UserGroups);
        }

        protected override Group GetEntityData()
        {
            var group = new Group
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Rights = View.GetRights().Encrypt()
            };

            return group;

        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            View.ResetRights();
            UsersManager.SetupForNewRecord();
        }

        protected override bool ValidateEntity(Group entity)
        {
            if (!UsersManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override bool SaveEntity(Group entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, $"Saving Group '{entity .Name}.'"))
            {
                var ugQuery = AppGlobals.DataRepository.GetDataContext().GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.GroupId == Id).ToList();
                context.RemoveRange(userGroups);
                userGroups = UsersManager.GetList();
                if (userGroups != null)
                {
                    foreach (var userGroup in userGroups)
                    {
                        userGroup.GroupId = entity.Id;
                    }

                    context.AddRange(userGroups);
                }

                return context.Commit("Saving UsersGroups");
            }

            return false;

        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<Group>();
            var group = query.FirstOrDefault(f => f.Id == Id);
            if (group != null)
            {
                var ugQuery = context.GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.GroupId == Id).ToList();
                context.RemoveRange(userGroups);
                if (context.DeleteNoCommitEntity(group, $"Deleting Group '{group.Name}'"))
                {
                    return context.Commit($"Deleting Group '{group.Name}'");
                }
            }
            return false;
        }
    }
}
