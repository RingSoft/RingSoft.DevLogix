using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for MaterialPartMaintenanceUserControl.xaml
    /// </summary>
    public partial class MaterialPartMaintenanceUserControl
    {
        public MaterialPartMaintenanceUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(KeyControl);
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
            return "Material Part";
        }
    }
}
