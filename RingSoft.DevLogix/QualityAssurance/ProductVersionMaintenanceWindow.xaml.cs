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
            //RegisterFormKeyControl(DescriptionControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            //DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }

    }
}
