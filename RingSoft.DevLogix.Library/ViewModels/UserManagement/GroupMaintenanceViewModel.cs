using System;
using System.Collections.Generic;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IGroupView : IDbMaintenanceView
    {
        public string GetRights();

        public void LoadRights(string rightsString);

        public void ResetRights();
    }

    public class GroupMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Group>
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

        //public GroupsUsersManager UsersManager { get; private set; }

        protected override void Initialize()
        {
            View = base.View as IGroupView;
            if (View == null)
                throw new Exception($"Group View interface must be of type '{nameof(IGroupView)}'.");

            UsersManager = new GroupsUsersManager(this);

            base.Initialize();
        }


        protected override Group PopulatePrimaryKeyControls(Group newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = AppGlobals.DataRepository.GetGroup(newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
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

        protected override bool SaveEntity(Group entity)
        {
            return AppGlobals.DataRepository.SaveGroup(entity, new List<UsersGroup>());
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteGroup(Id);
        }
    }
}
