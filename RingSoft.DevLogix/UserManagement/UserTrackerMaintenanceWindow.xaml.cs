using System;
using System.Collections.Generic;
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
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix.UserManagement
{
    public class UserTrackerHeaderControl : DbMaintenanceCustomPanel
    {
        public Button RefreshNowButton { get; set; }

        static UserTrackerHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserTrackerHeaderControl)
                , new FrameworkPropertyMetadata(typeof(UserTrackerHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RefreshNowButton = GetTemplateChild(nameof(RefreshNowButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for UserTrackerMaintenanceWindow.xaml
    /// </summary>
    public partial class UserTrackerMaintenanceWindow : IUserTrackerView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "User Tracker";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public UserTrackerMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is UserTrackerHeaderControl userHeaderControl)
                {
                    userHeaderControl.RefreshNowButton.Command = LocalViewModel.RefreshNowCommand;
                }
            };
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
