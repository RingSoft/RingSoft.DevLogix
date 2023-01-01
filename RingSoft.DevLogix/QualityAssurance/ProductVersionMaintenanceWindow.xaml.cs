using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
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
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for ProductVersionMaintenanceWindow.xaml
    /// </summary>
    public partial class ProductVersionMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Product Version";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProductVersionMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);
            if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
            {
                ArchiveButton.Visibility = Visibility.Collapsed;
                DeployLabel.Visibility = Visibility.Collapsed;
                DeployControl.Visibility = Visibility.Collapsed;
                DeployButton.Visibility = Visibility.Collapsed;
            }
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == GetVersionButton)
            {
                if (LocalViewModel.ArchiveDateTime != null)
                {
                    GetVersionButton.IsEnabled = true;
                    return;
                }
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
