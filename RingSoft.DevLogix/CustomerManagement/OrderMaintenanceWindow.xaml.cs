using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using System.Windows.Controls;

namespace RingSoft.DevLogix.CustomerManagement
{
    /// <summary>
    /// Interaction logic for OrderMaintenanceWindow.xaml
    /// </summary>
    public partial class OrderMaintenanceWindow
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
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
    }
}
