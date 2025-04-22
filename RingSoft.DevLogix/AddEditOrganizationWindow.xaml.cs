using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.MasterData;
using System;
using System.Windows;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for AddEditOrganizationWindow.xaml
    /// </summary>
    public partial class AddEditOrganizationWindow : IAddEditOrganizationView
    {
        public Organization Organization { get; set; }
        public OrganizationProcesses OrganizationProcess { get; set; }
        public bool DataCopied { get; set; }

        private TwoTierProcedure _procedure;

        public AddEditOrganizationWindow(DbLoginProcesses loginProcess, Organization organization = null)
        {
            InitializeComponent();
            Organization = organization;

            SqliteLogin.Loaded += (sender, args) => ViewModel.Initialize(this, loginProcess, SqliteLogin.ViewModel,
                SqlServerLogin.ViewModel, organization);
            SqlServerLogin.Loaded += (sender, args) => ViewModel.Initialize(this, loginProcess, SqliteLogin.ViewModel,
                SqlServerLogin.ViewModel, organization);

            switch (loginProcess)
            {
                case DbLoginProcesses.Add:
                case DbLoginProcesses.Edit:
                    break;
                case DbLoginProcesses.Connect:
                    OrganizationNameLabel.Visibility = Visibility.Collapsed;
                    OrganizationNameTextBox.Visibility = Visibility.Collapsed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(loginProcess), loginProcess, null);
            }
        }


        public void CloseWindow()
        {
            Close();
        }

        public void SetPlatform()
        {
            SqliteLogin.Visibility = Visibility.Collapsed;
            SqlServerLogin.Visibility = Visibility.Collapsed;
            switch (ViewModel.DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    SqliteLogin.Visibility = Visibility.Visible;
                    break;
                case DbPlatforms.SqlServer:
                    SqlServerLogin.Visibility = Visibility.Visible;
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetFocus(SetFocusControls control)
        {
            switch (control)
            {
                case SetFocusControls.OrganizationName:
                    OrganizationNameTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }

        }

        //public bool DoCopyProcedure()
        //{
        //    _procedure = new TwoTierProcedure();
        //    _procedure.DoProcedure += (sender, args) =>
        //    {
        //        args.Result = ViewModel.CopyData(_procedure);
        //    };
        //    var result = _procedure.Start();
        //    return result;
        //}
    }
}
