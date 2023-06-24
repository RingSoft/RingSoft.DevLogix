using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.ProjectManagement;

namespace RingSoft.DevLogix.CustomerManagement
{
    public class CustomerHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchInButton { get; set; }

        public DbMaintenanceButton RecalcButton { get; set; }

        static CustomerHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomerHeaderControl), new FrameworkPropertyMetadata(typeof(CustomerHeaderControl)));
        }

        public CustomerHeaderControl()
        {
            
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as DbMaintenanceButton;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for CustomerMaintenanceWindow.xaml
    /// </summary>
    public partial class CustomerMaintenanceWindow
    {
        public CustomerMaintenanceWindow()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is CustomerHeaderControl customerHeaderControl)
                {
                    customerHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    customerHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Alt + U)";
                    customerHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Customer. ";

                    customerHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Alt + R)";
                    customerHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of customers.";


                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        customerHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }

                    customerHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Customer";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(CompanyControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            CompanyControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
