using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using RingSoft.App.Library;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.MasterData;
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
            //var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Add)
            //{
            //    Owner = this
            //};
            //addEditOrganizationWindow.ShowDialog();
            //return addEditOrganizationWindow.ViewModel.Object;
            return null;
        }

        public bool EditOrganization(ref Organization organization)
        {
            //var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Edit, organization)
            //{
            //    Owner = this,
            //};
            //addEditOrganizationWindow.ShowDialog();
            //if (addEditOrganizationWindow.DataCopied)
            //{
            //    var message = "You must restart the application in order to continue.";
            //    var caption = "Restart Application";
            //    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            //    Application.Current.Shutdown();
            //}

            //if (addEditOrganizationWindow.ViewModel.DialogResult)
            //{
            //    if (addEditOrganizationWindow.ViewModel.Object != null)
            //        organization = addEditOrganizationWindow.ViewModel.Object;
            //}

            //return organization != null;
            return true;
        }

        public AddEditOrganizationViewModel GetOrganizationConnection()
        {
            //var addEditOrganizationWindow = new AddEditOrganizationWindow(DbLoginProcesses.Connect)
            //{
            //    Owner = this
            //};
            //addEditOrganizationWindow.ShowDialog();
            //return addEditOrganizationWindow.ViewModel;
            return null;
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
            Close();
        }

        public void ShutDownApplication()

        {
            Application.Current.Shutdown(0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = ViewModel.DoCancelClose();
            base.OnClosing(e);
        }
    }
}
