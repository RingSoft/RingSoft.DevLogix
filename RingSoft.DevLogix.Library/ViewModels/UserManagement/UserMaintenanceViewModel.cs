using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserMaintenanceViewModel : AppDbMaintenanceViewModel<User>
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

        protected override void Initialize()
        {
            TypeComboboxSetup = new TextComboBoxControlSetup();
            TypeComboboxSetup.LoadFromEnum<UserTypes>();

            base.Initialize();
        }

        protected override User PopulatePrimaryKeyControls(User newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = AppGlobals.DataRepository.GetUser(newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            return result;
        }

        protected override void LoadFromEntity(User entity)
        {
            UserType = (UserTypes) entity.Type;
        }

        protected override User GetEntityData()
        {
            var user = new User
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Type = (byte) UserType,
            };

            return user;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            UserType = UserTypes.Miscellaneous;
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
