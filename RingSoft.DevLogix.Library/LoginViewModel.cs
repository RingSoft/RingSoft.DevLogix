using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.MasterData;

namespace RingSoft.DevLogix.Library
{
    public interface ILoginView
    {
        bool LoginToOrganization(Organization Organization);

        Organization ShowAddOrganization();

        bool EditOrganization(ref Organization Organization);

        AddEditOrganizationViewModel GetOrganizationConnection();

        void CloseWindow();

        void ShutDownApplication();
    }

    public class LoginListBoxItem : INotifyPropertyChanged
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                _text = value;
                OnPropertyChanged();
            }
        }


        //public string Text { get; set; }

        public Organization Organization { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class LoginViewModel : INotifyPropertyChanged
    {
        public ILoginView View { get; private set; }

        private ObservableCollection<LoginListBoxItem> _listBoxItems;

        public ObservableCollection<LoginListBoxItem> Items
        {
            get => _listBoxItems;
            set
            {
                if (_listBoxItems == value)
                    return;

                _listBoxItems = value;
                OnPropertyChanged();
            }
        }

        private LoginListBoxItem _selectedItem;

        public LoginListBoxItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value)
                    return;

                _selectingOrganization = true;

                _selectedItem = value;

                if (SelectedItem == null)
                    IsDefault = false;
                else
                    IsDefault = SelectedItem.Organization.IsDefault;

                _selectingOrganization = false;

                OnPropertyChanged();
            }
        }

        private bool _isDefault;
        public bool IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                if (_isDefault == value)
                    return;

                _isDefault = value;
                UpdateDefaults();

                OnPropertyChanged();
            }
        }

        public bool DialogResult { get; private set; }

        public RelayCommand AddNewCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ConnectToDataFileCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }

        private readonly bool _initialized;
        private bool _selectingOrganization;

        public LoginViewModel()
        {
            Items = new ObservableCollection<LoginListBoxItem>();
            var dbOrganizations = MasterDbContext.GetOrganizations();
            
            foreach (var organization in dbOrganizations)
            {
                var listBoxItem = new LoginListBoxItem
                {
                    Organization = organization,
                    Text = organization.Name
                };
                Items.Add(listBoxItem);

                if (AppGlobals.LoggedInOrganization != null && AppGlobals.LoggedInOrganization.Id == organization.Id)
                {
                    listBoxItem.Text = $"(Active) {listBoxItem.Text}";
                    SelectedItem = listBoxItem;
                    IsDefault = organization.IsDefault;
                }
            }

            if (SelectedItem == null && Items.Any())
                SelectedItem = Items[0];

            AddNewCommand = new RelayCommand(AddNewOrganization);
            EditCommand = new RelayCommand(EditOrganization) { IsEnabled = CanDeleteOrganization() };
            DeleteCommand = new RelayCommand(DeleteOrganization) { IsEnabled = CanDeleteOrganization() };
            ConnectToDataFileCommand = new RelayCommand(ConnectToDataFile);
            LoginCommand = new RelayCommand(Login) { IsEnabled = CanLogin() };
            CancelCommand = new RelayCommand(Cancel);

            _initialized = true;
        }

        public void OnViewLoaded(ILoginView loginView) => View = loginView;

        private bool CanLogin() => SelectedItem != null;

        private bool CanDeleteOrganization()
        {
            if (SelectedItem == null)
                return false;

            if (SelectedItem.Organization.Id == 1)
                return false;

            if (AppGlobals.LoggedInOrganization != null)
                return AppGlobals.LoggedInOrganization.Id != SelectedItem.Organization.Id;

            return true;
        }

        private void AddNewOrganization()
        {
            var newOrganization = View.ShowAddOrganization();
            if (newOrganization != null)
                AddNewOrganization(newOrganization);
        }

        private void EditOrganization()
        {
            var organization = SelectedItem.Organization;
            if (View.EditOrganization(ref organization))
            {
                SelectedItem.Organization = organization;
                MasterDbContext.SaveOrganization(SelectedItem.Organization);
                SelectedItem.Text = SelectedItem.Organization.Name;
            }
        }

        private void AddNewOrganization(Organization newOrganization)
        {
            var item = new LoginListBoxItem
            {
                Organization = newOrganization,
                Text = newOrganization.Name
            };
            Items.Add(item);
            Items = new ObservableCollection<LoginListBoxItem>(Items.OrderBy(o => o.Text));
            MasterDbContext.SaveOrganization(newOrganization);
            SelectedItem = item;
        }

        private void ConnectToDataFile()
        {
            //var connection = View.GetOrganizationConnection();
            //if (connection.DialogResult)
            //{
            //    var currentPlatform = AppGlobals.DbPlatform;
            //    AppGlobals.LookupContext.DbPlatform = connection.DbPlatform;
            //    AppGlobals.DbPlatform = connection.DbPlatform;

            //    switch (connection.DbPlatform)
            //    {
            //        case DbPlatforms.Sqlite:
            //            var currentFilePath = AppGlobals.LookupContext.SqliteDataProcessor.FilePath;
            //            var currentFileName = AppGlobals.LookupContext.SqliteDataProcessor.FileName;

            //            AppGlobals.LookupContext.SqliteDataProcessor.FilePath = connection.SqliteLoginViewModel.FilePath;
            //            AppGlobals.LookupContext.SqliteDataProcessor.FileName = connection.SqliteLoginViewModel.FileName;

            //            ConnectToOrganization(connection);

            //            AppGlobals.LookupContext.SqliteDataProcessor.FilePath = currentFilePath;
            //            AppGlobals.LookupContext.SqliteDataProcessor.FileName = currentFileName;
            //            break;
            //        case DbPlatforms.SqlServer:
            //            var currentServer = AppGlobals.LookupContext.SqlServerDataProcessor.Server;
            //            var currentDatabase = AppGlobals.LookupContext.SqlServerDataProcessor.Database;
            //            var currentSecurityType = AppGlobals.LookupContext.SqlServerDataProcessor.SecurityType;
            //            var currentUserName = AppGlobals.LookupContext.SqlServerDataProcessor.UserName;
            //            var currentPassword = AppGlobals.LookupContext.SqlServerDataProcessor.Password;

            //            AppGlobals.LookupContext.SqlServerDataProcessor.Server = connection.SqlServerLoginViewModel.Server;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.Database = connection.SqlServerLoginViewModel.Database;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.SecurityType = connection.SqlServerLoginViewModel.SecurityType;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.UserName = connection.SqlServerLoginViewModel.UserName;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.Password = connection.SqlServerLoginViewModel.Password;

            //            ConnectToOrganization(connection);

            //            AppGlobals.LookupContext.SqlServerDataProcessor.Server = currentServer;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.Database = currentDatabase;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.SecurityType = currentSecurityType;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.UserName = currentUserName;
            //            AppGlobals.LookupContext.SqlServerDataProcessor.Password = currentPassword;

            //            break;
            //        case DbPlatforms.MySql:
            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }

            //    AppGlobals.LookupContext.DbPlatform = currentPlatform;
            //    AppGlobals.DbPlatform = currentPlatform;
            //}
        }

        private void ConnectToOrganization(AddEditOrganizationViewModel connection)
        {
            //var systemMaster = AppGlobals.DataRepository.GetSystemMaster();
            //if (systemMaster != null)
            //{
            //    var organization = new Organization
            //    {
            //        Name = systemMaster.OrganizationName,
            //        Platform = (byte)connection.DbPlatform,
            //        FileName = connection.SqliteLoginViewModel.FileName,
            //        FilePath = connection.SqliteLoginViewModel.FilePath,
            //        Server = connection.SqlServerLoginViewModel.Server,
            //        Database = connection.SqlServerLoginViewModel.Database,
            //        AuthenticationType = (byte)connection.SqlServerLoginViewModel.SecurityType,
            //        Username = connection.SqlServerLoginViewModel.UserName,
            //        Password = connection.SqlServerLoginViewModel.Password
            //    };
            //    AddNewOrganization(Organization);
            //}
        }

        private void DeleteOrganization()
        {
            string message;
            if (SelectedItem.Organization.Id == 1)
            {
                message = "Deleting Demo Organization is not allowed.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Operation",
                    RsMessageBoxIcons.Exclamation);
                return;
            }

            message = "Are you sure you wish to delete this Organization?";
            if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Confirm Delete") ==
                MessageBoxButtonsResult.Yes)
            {
                if (MasterDbContext.DeleteOrganization(SelectedItem.Organization.Id))
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = Items[0];
                }
            }
        }

        private void Login()
        {
            if (View.LoginToOrganization(SelectedItem.Organization))
            {
                AppGlobals.LoggedInOrganization = SelectedItem.Organization;
                DialogResult = true;
                View.CloseWindow();
            }
        }

        private void Cancel()
        {
            DialogResult = false;
            View.CloseWindow();
        }

        public bool DoCancelClose()
        {
            if (AppGlobals.LoggedInOrganization == null && !DialogResult)
            {
                var message = "Application will shut down if you do not login.  Do you wish to continue?";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Login Failure") ==
                    MessageBoxButtonsResult.Yes)
                {
                    View.ShutDownApplication();
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateDefaults()
        {
            if (_selectingOrganization)
                return;

            SelectedItem.Organization.IsDefault = IsDefault;
            MasterDbContext.SaveOrganization(SelectedItem.Organization);

            if (IsDefault)
            {
                foreach (var item in Items)
                {
                    if (item != SelectedItem && item.Organization.IsDefault)
                    {
                        item.Organization.IsDefault = false;
                        MasterDbContext.SaveOrganization(item.Organization);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (_initialized)
            {
                DeleteCommand.IsEnabled = CanDeleteOrganization();
                EditCommand.IsEnabled = CanDeleteOrganization();
                LoginCommand.IsEnabled = CanLogin();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
