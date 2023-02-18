using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using System.Windows;
using System.Windows.Forms;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using Button = System.Windows.Controls.Button;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class ProductHeaderControl : DbMaintenanceCustomPanel
    {
        public Button UpdateVersionsButton { get; set; }

        static ProductHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProductHeaderControl), new FrameworkPropertyMetadata(typeof(ProductHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            UpdateVersionsButton = GetTemplateChild(nameof(UpdateVersionsButton)) as Button;

            base.OnApplyTemplate();
        }
    }

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

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.CreateDepartmentId))
            {
                TabControl.SelectedItem = DeploymentTabItem;
                TabControl.UpdateLayout();
                CreateDepartmentControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
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

        public string GetInstallerName()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                if (!LocalViewModel.InstallerFileName.IsNullOrEmpty())
                {
                    var file = new System.IO.FileInfo(LocalViewModel.InstallerFileName);
                    fileDialog.InitialDirectory = file.DirectoryName;
                    fileDialog.FileName = file.Name;
                }
                
                var dialogResult = fileDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK && !fileDialog.FileName.IsNullOrEmpty())
                {
                    return fileDialog.FileName;
                }
            }

            return LocalViewModel.InstallerFileName;
        }

        public string GetArchivePath()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.InitialDirectory = LocalViewModel.ArchivePath;
                var dialogResult = folderDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK && !folderDialog.SelectedPath.IsNullOrEmpty())
                {
                    return folderDialog.SelectedPath;
                }
            }

            return LocalViewModel.ArchivePath;
        }

        public void SetViewToVersions()
        {
            TabControl.SelectedItem = VersionsTabItem;
            VersionsTabItem.UpdateLayout();
            VersionLookupControl.Focus();
        }
    }
}
