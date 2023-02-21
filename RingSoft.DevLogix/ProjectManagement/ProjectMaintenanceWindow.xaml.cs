using RingSoft.App.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProjectMaintenanceWindow()
        {
            InitializeComponent();
        }

    }
}
