using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using System;
using System.Linq;
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
        #region Properties
        
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

        #endregion

        public new IGroupView View { get; private set; }

        protected override void Initialize()
        {
            View = base.View as IGroupView;
            if (View == null)
                throw new Exception($"Group View interface must be of type '{nameof(IGroupView)}'.");

            UsersManager = new GroupsUsersManager(this);
            RegisterGrid(UsersManager);

            base.Initialize();
        }


        protected override void PopulatePrimaryKeyControls(Group newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(Group entity)
        {
            Name = entity.Name;
            View.LoadRights(entity.Rights.Decrypt());
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
            View.ResetRights();
        }

        protected override bool ValidateEntity(Group entity)
        {
            if (!UsersManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
        }
    }
}
