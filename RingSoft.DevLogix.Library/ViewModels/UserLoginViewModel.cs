using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IUserLoginView
    {
        void CloseWindow();
    }
    public class UserLoginViewModel : INotifyPropertyChanged
    {
        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup  == value)
                {
                    return;
                }
                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue  _userAutoFillValue;

        public AutoFillValue  UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                {
                    return;
                }
                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        public IUserLoginView View { get; private set; }

        public bool DialogResult { get; private set; }

        public RelayCommand OkCommand { get; set; }

        public UserLoginViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
        }

        public void Initialize(IUserLoginView view)
        {
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
            UserAutoFillSetup.AllowLookupAdd = UserAutoFillSetup.AllowLookupView = false;
            View = view;
        }

        public void OnOk()
        {
            if (UserAutoFillValue == null || !UserAutoFillValue.IsValid())
            {
                var message = "Invalid user";
                var caption = "Invalid User";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return;
            }

            var user = AppGlobals.LookupContext.Users.GetEntityFromPrimaryKeyValue(UserAutoFillValue.PrimaryKeyValue);
            var userTable = AppGlobals.DataRepository.GetTable<User>();
            user = userTable.FirstOrDefault(p => p.Id == user.Id);

            AppGlobals.LoggedInUser = user;
            SystemGlobals.UserName = user.Name;
            AppGlobals.Rights.UserRights.LoadRights(user.Rights.Decrypt());

            DialogResult = true;
            View.CloseWindow();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
