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
    /// Interaction logic for OrderMaintenanceWindow.xaml
    /// </summary>
    public partial class OrderMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Order";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public OrderMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(OrderIdControl);
        }

        protected override void OnLoaded()
        {
            TopHeaderControl.PrintButton.Content = "_Print Invoice";
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
