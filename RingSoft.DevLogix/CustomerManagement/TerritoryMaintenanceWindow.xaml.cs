using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.CustomerManagement
{
    /// <summary>
    /// Interaction logic for TerritoryMaintenanceWindow.xaml
    /// </summary>
    public partial class TerritoryMaintenanceWindow
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Territory";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public TerritoryMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);
        }
    }
}
