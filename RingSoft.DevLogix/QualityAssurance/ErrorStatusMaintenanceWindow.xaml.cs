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

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for AddEditErrorStatuses.xaml
    /// </summary>
    public partial class ErrorStatusMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Error Status";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ErrorStatusMaintenanceWindow()
        {
            InitializeComponent();
        }

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
