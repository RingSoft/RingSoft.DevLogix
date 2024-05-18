using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
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

        string GetPassword();
        
        void EnablePassword(bool enable);
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
                EnablePassword();
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

        private bool _initializing;
        private int _initUserId = 0;

        public void Initialize(IUserLoginView view, int userId)
        {
            _initializing = true;
            View = view;
            _initUserId = userId;
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
            UserAutoFillSetup.AllowLookupAdd = UserAutoFillSetup.AllowLookupView = false;

            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = table.FirstOrDefault(p => p.Id == userId);
            if (userId > 0)
            {
                UserAutoFillValue = user.GetAutoFillValue();
            }
            _initializing = false;
        }

        private void EnablePassword()
        {
            var user = UserAutoFillValue.GetEntity<User>();
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            if (user.Id > 0)
            {
                user = table.FirstOrDefault(p => p.Id == user.Id);
                if (user.Password.IsNullOrEmpty())
                {
                    View.EnablePassword(false);
                }
                else
                {
                    View.EnablePassword(true);
                }
            }
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
            var context = AppGlobals.DataRepository.GetDataContext();
            IQueryable<User> userTable = context.GetTable<User>();
            userTable = userTable.Include(p => p.UserGroups).ThenInclude(p => p.Group)
                .Include(p => p.Department);

            user = userTable.FirstOrDefault(p => p.Id == user.Id);
            if (user != null)
            {
                var password = user.Password;
                var loginPassword = View.GetPassword();
                if (!password.IsNullOrEmpty())
                {
                    var match = loginPassword.IsValidPassword(password);
                    if (!match)
                    {
                        var message = "Invalid Password.";
                        var caption = "Invalid Password.";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return;
                    }
                }
            }

            if (_initUserId > 0)
            {
                DialogResult = true;
                View.CloseWindow();
            }

            AppGlobals.ClockInUser(context, user);

            AppGlobals.LoggedInUser = user;
            SystemGlobals.UserName = user.Name;
            SystemGlobals.Rights.UserRights.LoadRights(user.Rights.Decrypt());

            SystemGlobals.Rights.GroupRights.Clear();
            foreach (var userUserGroup in user.UserGroups)
            {
                var rights = new DevLogixRights();
                rights.LoadRights(userUserGroup.Group.Rights.Decrypt());
                SystemGlobals.Rights.GroupRights.Add(rights);
            }

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
