using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for DepartmentMaintenanceUserControl.xaml
    /// </summary>
    public partial class DepartmentMaintenanceUserControl : IDepartmentView
    {
        public string FtpPassword
        {
            get
            {
                return FtpPasswordBox.Password;
            }
            set
            {
                FtpPasswordBox.Password = value;
            }
        }

        public DepartmentMaintenanceUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);

            Loaded += (sender, args) =>
            {
                if (!AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowAdd))
                {
                    AddModifyUserButton.Visibility = Visibility.Collapsed;
                }
            };
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return LocalViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Department";
        }
    }
}
