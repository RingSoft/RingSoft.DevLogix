using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for ProductMaintenanceWindow.xaml
    /// </summary>
    public partial class ProductMaintenanceWindow : IProductView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Product";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProductMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProductHeaderControl productHeaderControl)
                {
                    productHeaderControl.UpdateVersionsButton.Command =
                        LocalViewModel.UpdateVersionsCommand;

                    if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowEdit))
                    {
                        productHeaderControl.UpdateVersionsButton.Visibility = Visibility.Collapsed;
                    }
                }
            };

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);
            if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowAdd))
            {
                AddModifyButton.Visibility = Visibility.Collapsed;
            }
            if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowView))
            {
                VersionsTabItem.Visibility = Visibility.Collapsed;
                TabControl.SelectedItem = NotesTabItem;
            }
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }


        public bool UpdateVersions(ProductViewModel viewModel)
        {
            var window = new ProductUpdateVersionsWindow(viewModel);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            if (window.DialogResult.HasValue)
            {
                return window.DialogResult.Value;
            }
            return false;
        }
    }
}
