using RingSoft.App.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for LaborPartMaintenanceWindow.xaml
    /// </summary>
    public partial class LaborPartMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Labor Part";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public LaborPartMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(KeyControl);
        }

        public override void ResetViewForNewRecord()
        {
            KeyControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
