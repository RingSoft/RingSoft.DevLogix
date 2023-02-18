using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;
using System.IO;
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
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.QualityAssurance
{
    internal class DeployProcedure : AppProcedure
    {

        public override ISplashWindow SplashWindow => _splashWindow;

        public event EventHandler DoProcessEvent;

        private ProcessingSplashWindow _splashWindow;

        protected override void ShowSplash()
        {
            _splashWindow = new ProcessingSplashWindow("Deploying");
            _splashWindow.ShowDialog();

        }

        protected override bool DoProcess()
        {
            DoProcessEvent?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }

    public class ProductVersionHeaderControl : DbMaintenanceCustomPanel
    {
        public Button CreateVersionButton { get; set; }

        static ProductVersionHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProductVersionHeaderControl), new FrameworkPropertyMetadata(typeof(ProductVersionHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            CreateVersionButton = GetTemplateChild(nameof(CreateVersionButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProductVersionMaintenanceWindow.xaml
    /// </summary>
    public partial class ProductVersionMaintenanceWindow : IProductVersionView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Product Version";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        private DeployProcedure _deployProcedure;

        public ProductVersionMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProductVersionHeaderControl productVersionHeaderControl)
                {
                    productVersionHeaderControl.CreateVersionButton.Command =
                        LocalViewModel.CreateVersionCommand;

                    if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowAdd))
                    {
                        productVersionHeaderControl.CreateVersionButton.Visibility = Visibility.Collapsed;
                    }
                }
            };
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

            LocalViewModel.UpdateStatusEvent += (sender, args) =>
            {
                if (_deployProcedure != null)
                {
                    _deployProcedure.SplashWindow.SetProgress(args.Status);
                }
            };

            LocalViewModel.DeployErrorEvent += (sender, args) =>
            {
                if (_deployProcedure != null)
                {
                    _deployProcedure.SplashWindow.ShowMessageBox(args.Status, "Deploy Error", RsMessageBoxIcons.Error);
                }
            };
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }

        public bool UploadFile(FileInfo file, Department department, Product product)
        {
            var result = true;
            _deployProcedure = new DeployProcedure();
            _deployProcedure.DoProcessEvent += (sender, args) =>
            {
                result = LocalViewModel.UploadFile(file, department, product);
            };
            _deployProcedure.Start();
            _deployProcedure.SplashWindow.CloseSplash();
            _deployProcedure = null;
            return result;
        }

        public void SetFocusToGrid()
        {
            TabControl.SelectedItem = GridTabItem;
            TabControl.UpdateLayout();
            GridControl.Focus();
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
