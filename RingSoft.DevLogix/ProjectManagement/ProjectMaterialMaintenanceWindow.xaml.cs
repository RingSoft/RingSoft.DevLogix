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
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaterialMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Material";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProjectMaterialMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(KeyControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedItem = DetailsTabItem;
            KeyControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
