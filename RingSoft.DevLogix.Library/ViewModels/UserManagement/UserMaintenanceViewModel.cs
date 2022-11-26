using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IUserView : IDbMaintenanceView
    {
        public string GetRights();

        public void LoadRights(string rightsString);

        public void ResetRights();
    }
    public class UserMaintenanceViewModel : DevLogixDbMaintenanceViewModel<User>
    {
        public override TableDefinition<User> TableDefinition => AppGlobals.LookupContext.Users;

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

        private TextComboBoxControlSetup _typeComboboxSetup;

        public TextComboBoxControlSetup TypeComboboxSetup
        {
            get => _typeComboboxSetup;
            set
            {
                if (_typeComboboxSetup == value)
                {
                    return;
                }
                _typeComboboxSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _typeComboboxValue;

        public TextComboBoxItem TypeComboboxValue
        {
            get => _typeComboboxValue;
            set
            {
                if (_typeComboboxValue == value)
                {
                    return;
                }
                _typeComboboxValue = value;
                OnPropertyChanged();
            }
        }

        private UsersGroupsManager _groupsManager;

        public UsersGroupsManager GroupsManager
        {
            get => _groupsManager;
            set
            {
                if (_groupsManager == value)
                {
                    return;
                }
                _groupsManager = value;
                OnPropertyChanged();
            }
        }

        public UserTypes UserType
        {
            get
            {
                if (TypeComboboxValue != null) return (UserTypes) TypeComboboxValue.NumericValue;
                return UserTypes.Miscellaneous;
            }

            set => TypeComboboxValue = TypeComboboxSetup.GetItem((byte) value);
        }

        public new IUserView View { get; private set; }

        protected override void Initialize()
        {
            View = base.View as IUserView;
            if (View == null)
                throw new Exception($"User View interface must be of type '{nameof(IUserView)}'.");

            TypeComboboxSetup = new TextComboBoxControlSetup();
            TypeComboboxSetup.LoadFromEnum<UserTypes>();

            GroupsManager = new UsersGroupsManager( this);

            base.Initialize();
        }

        protected override User PopulatePrimaryKeyControls(User newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<User> query = AppGlobals.DataRepository.GetTable<User>();
            query = query.Include(p => p.UserGroups);

            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            return result;
        }

        protected override void LoadFromEntity(User entity)
        {
            UserType = (UserTypes) entity.Type;
            View.LoadRights(entity.Rights.Decrypt());
            GroupsManager.LoadGrid(entity.UserGroups);
        }

        protected override User GetEntityData()
        {
            var user = new User
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Type = (byte) UserType,
                Rights = View.GetRights().Encrypt()
            };

            return user;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            UserType = UserTypes.Miscellaneous;
            View.ResetRights();
            GroupsManager.SetupForNewRecord();
        }

        protected override bool SaveEntity(User entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, $"Saving User '{entity.Name}.'"))
            {
                var ugQuery = AppGlobals.DataRepository.GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userGroups);
                userGroups = GroupsManager.GetEntityList();
                if (userGroups != null)
                {
                    foreach (var userGroup in userGroups)
                    {
                        userGroup.UserId = entity.Id;
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
            var query = AppGlobals.DataRepository.GetTable<User>();
            var user = query.FirstOrDefault(f => f.Id == Id);
            if (user != null)
            {
                var ugQuery = AppGlobals.DataRepository.GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userGroups);
                if (context.DeleteNoCommitEntity(user, $"Deleting User '{user.Name}'"))
                {
                    return context.Commit($"Deleting User '{user.Name}'");
                }
            }
            return false;

        }
    }
}
