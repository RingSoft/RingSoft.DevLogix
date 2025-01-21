using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using Control = System.Windows.Controls.Control;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class ProductHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton UpdateVersionsButton { get; set; }

        public DbMaintenanceButton RecalculateButton { get; set; }

        public DbMaintenanceButton GotoVersionsButton { get; set; }

        static ProductHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProductHeaderControl), new FrameworkPropertyMetadata(typeof(ProductHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            UpdateVersionsButton = GetTemplateChild(nameof(UpdateVersionsButton)) as DbMaintenanceButton;
            RecalculateButton = GetTemplateChild(nameof(RecalculateButton)) as DbMaintenanceButton;
            GotoVersionsButton = GetTemplateChild(nameof(GotoVersionsButton)) as DbMaintenanceButton;
            ;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProductMaintenanceUserControl.xaml
    /// </summary>
    public partial class ProductMaintenanceUserControl : IProductView
    {
        public RecalcProcedure RecalcProcedure { get; set; }
        private RelayCommand _gotoVersionsCommand;

        public ProductMaintenanceUserControl()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProductHeaderControl productHeaderControl)
                {
                    productHeaderControl.UpdateVersionsButton.Command =
                        LocalViewModel.UpdateVersionsCommand;

                    productHeaderControl.RecalculateButton.Command =
                        LocalViewModel.RecalcCommand;


                    if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowEdit))
                    {
                        productHeaderControl.UpdateVersionsButton.Visibility = Visibility.Collapsed;
                    }

                    productHeaderControl.UpdateVersionsButton.ToolTip.HeaderText = "Update Department Versions (Ctrl + P, Ctrl + U)";
                    productHeaderControl.UpdateVersionsButton.ToolTip.DescriptionText = "Update Versions departments.";

                    productHeaderControl.GotoVersionsButton.ToolTip.HeaderText = "Goto Versions List (Ctrl + P, Ctrl + G)";
                    productHeaderControl.GotoVersionsButton.ToolTip.DescriptionText = "Goto the Versions list.";

                    productHeaderControl.RecalculateButton.ToolTip.HeaderText = "Recalculate Data (Ctrl + P, Ctrl + R)";
                    productHeaderControl.RecalculateButton.ToolTip.DescriptionText = "Recalculate Totals.";

                    //Peter Ringering - 01/16/2025 04:04:59 PM - E-114
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        productHeaderControl.RecalculateButton.Visibility = Visibility.Collapsed;
                        
                        Grid.SetRow(productHeaderControl.GotoVersionsButton, 0);
                        Grid.SetRowSpan(productHeaderControl.GotoVersionsButton, 2);
                    }
                }
            };
            Loaded += (sender, args) =>
            {
                if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowAdd))
                {
                    AddModifyButton.Visibility = Visibility.Collapsed;
                }
                if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowView))
                {
                    VersionsTabItem.Visibility = Visibility.Collapsed;
                    TabControl.SelectedItem = NotesTabItem;
                }
            };
            RegisterFormKeyControl(DescriptionControl);

            _gotoVersionsCommand = new RelayCommand(GotoVersions);

            var hotKey = new HotKey(LocalViewModel.UpdateVersionsCommand);
            hotKey.AddKey(Key.P);
            hotKey.AddKey(Key.U);
            AddHotKey(hotKey);

            hotKey = new HotKey(_gotoVersionsCommand);
            hotKey.AddKey(Key.P);
            hotKey.AddKey(Key.G);
            AddHotKey(hotKey);

            hotKey = new HotKey(LocalViewModel.RecalcCommand);
            hotKey.AddKey(Key.P);
            hotKey.AddKey(Key.R);
            AddHotKey(hotKey);

            AddModifyButton.ToolTip.HeaderText = "Add/Modify Version (Ctrl + P, Ctrl + A)";
            AddModifyButton.ToolTip.DescriptionText =
                "Add or modify a product version.";

            hotKey = new HotKey(LocalViewModel.VersionsAddModifyCommand);
            hotKey.AddKey(Key.P);
            hotKey.AddKey(Key.A);
            AddHotKey(hotKey);

            TestOutlinesAddModifyButton.ToolTip.HeaderText = "Add/Modify Testing Outline (Ctrl + P, Ctrl + A)";
            TestOutlinesAddModifyButton.ToolTip.DescriptionText =
                "Add or modify a testing outline.";

            hotKey = new HotKey(LocalViewModel.TestOutlinesAddModifyCommand);
            hotKey.AddKey(Key.P);
            hotKey.AddKey(Key.M);
            AddHotKey(hotKey);
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
            return "Product";
        }

        //Peter Ringering - 01/15/2025 12:59:40 PM - E-107
        private void GotoVersions()
        {
            TabControl.SelectedItem = VersionsTabItem;
            TabControl.UpdateLayout();
            VersionsTabItem.UpdateLayout();
            VersionLookupControl.Focus();
        }

        public bool UpdateVersions(ProductViewModel viewModel)
        {
            var window = new ProductUpdateVersionsWindow(viewModel);
            window.Owner = OwnerWindow;
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

        public void RefreshView()
        {
            if (LocalViewModel.Difference < 0)
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.LightPink);
            }
            else if (LocalViewModel.Difference > 0)
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Product",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = OwnerWindow;
            genericWindow.ShowInTaskbar = false;
            genericWindow.ShowDialog();
            return genericWindow.ViewModel.DialogReesult;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupDefinition)
        {
            var result = string.Empty;
            RecalcProcedure = new RecalcProcedure();
            RecalcProcedure.StartRecalculate += (sender, args) =>
            {
                result = LocalViewModel.StartRecalcProcedure(lookupDefinition, RecalcProcedure);
            };
            RecalcProcedure.Start();
            return result;
        }

        public void UpdateRecalcProcedure(int currentProduct, int totalProducts, string currentProductText)
        {
            var progress = $"Recalculating Customer {currentProductText} {currentProduct} / {totalProducts}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }

        public bool IsVersionsTabSelected()
        {
            return TabControl.SelectedItem == VersionsTabItem;
        }

        public bool IsOutlinesTabSelected()
        {
            return TabControl.SelectedItem == TestOutlinesTabItem;
        }

        public void PlayExclSound()
        {
            SystemSounds.Exclamation.Play();
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == DeptFilterControl)
            {
                return;
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
