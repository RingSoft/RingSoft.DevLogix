using System.Windows.Controls;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for LaborPartMaintenanceWindow.xaml
    /// </summary>
    public partial class LaborPartMaintenanceWindow
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Labor Part";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public LaborPartMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(KeyControl);
        }

    }
}
