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
        }

        protected override bool SaveEntity(User entity)
        {
            return AppGlobals.DataRepository.SaveUser(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteUser(Id);
        }
    }
}
