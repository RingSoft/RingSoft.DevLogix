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
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.CustomerManagement
{
    /// <summary>
    /// Interaction logic for TimeZoneMaintenanceWindow.xaml
    /// </summary>
    public partial class TimeZoneMaintenanceWindow
    {
        public TimeZoneMaintenanceWindow()
        {
            InitializeComponent();
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Time Zone";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
