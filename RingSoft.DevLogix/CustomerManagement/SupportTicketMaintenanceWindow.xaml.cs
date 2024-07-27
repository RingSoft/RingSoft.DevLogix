using RingSoft.App.Controls;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DevLogix.CustomerManagement
{
    public class SupportTicketHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchInButton { get; set; }

        public DbMaintenanceButton RecalcButton { get; set; }

        static SupportTicketHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SupportTicketHeaderControl),
                new FrameworkPropertyMetadata(typeof(SupportTicketHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as DbMaintenanceButton;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for SupportTicketMaintenanceWindow.xaml
    /// </summary>
    public partial class SupportTicketMaintenanceWindow : ISupportTicketView
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Support Ticket";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
        public RecalcProcedure RecalcProcedure { get; set; }

        public SupportTicketMaintenanceWindow()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is SupportTicketHeaderControl supportTicketHeaderControl)
                {
                    supportTicketHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    supportTicketHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Alt + U)";
                    supportTicketHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Ticket. ";

                    supportTicketHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Alt + R)";
                    supportTicketHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of support tickets.";


                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        supportTicketHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }

                    supportTicketHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };
            RegisterFormKeyControl(TicketIdControl);

        }

        public override void ResetViewForNewRecord()
        {
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Support Ticket",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = this;
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

        public void UpdateRecalcProcedure(int currentSupportTicket, int totalSupportTickets, string currentSupportTicketText)
        {
            var progress = $"Recalculating Support Ticket {currentSupportTicketText} {currentSupportTicket} / {totalSupportTickets}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }
    }
}
