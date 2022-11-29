using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for ProductMaintenanceWindow.xaml
    /// </summary>
    public partial class ProductMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Product";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProductMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);
            //if (!AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowAdd))
            //{
            //    AddModifyUserButton.Visibility = Visibility.Collapsed;
            //}
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }


    }
}
