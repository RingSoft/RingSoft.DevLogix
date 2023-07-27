using Microsoft.Win32;
using RingSoft.App.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.MasterData;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AddEditOrganizationViewModel = RingSoft.DevLogix.Library.ViewModels.AddEditOrganizationViewModel;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : ILoginView
    {
        public LoginWindow()
        {
            InitializeComponent();

            ViewModel.OnViewLoaded(this);
            ListBox.MouseDoubleClick += (sender, args) => ViewModel.LoginCommand.Execute(null);
            ListBox.GotKeyboardFocus += (sender, args) => ListBox.SelectedItem ??= ListBox.Items[0];
        }

        public bool LoginToOrganization(Organization organization)
        {
            var loginProcedure = new LoginProcedure(organization);
            return loginProcedure.Start();
        }

        public Organization ShowAddOrganization()
        {
            var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Add)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditOrganizationWindow.ShowDialog();
            return addEditOrganizationWindow.ViewModel.Object;
        }

        public bool EditOrganization(ref Organization organization)
        {
            var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Edit, organization)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditOrganizationWindow.ShowDialog();
            //if (addEditOrganizationWindow.DataCopied)
            //{
            //    var message = "You must restart the application in order to continue.";
            //    var caption = "Restart Application";
            //    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            //    Application.Current.Shutdown();
            //}

            if (addEditOrganizationWindow.ViewModel.DialogResult)
            {
                if (addEditOrganizationWindow.ViewModel.Object != null)
                    organization = addEditOrganizationWindow.ViewModel.Object;
            }
            else
            {
                return false;
            }

            return organization != null;
        }

        public AddEditOrganizationViewModel GetOrganizationConnection()
        {
            var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Connect)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditOrganizationWindow.ShowDialog();
            return addEditOrganizationWindow.ViewModel;
        }

        public string GetOrganizationDataFile()
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = "sqlite",
                Filter = "HomeLogix SQLite Files(*.sqlite)|*.sqlite"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        public void CloseWindow()
        {
            DialogResult = ViewModel.DialogResult;
            if (!ViewModel.CancelClose)
            {
                Close();
            }
        }

        public void ShutDownApplication()

        {
            Application.Current.Shutdown(0);
        }

        protected async override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = await ViewModel.DoCancelClose();
            if (e.Cancel)
            {
                return;
            }
            base.OnClosing(e);
        }
    }
}
