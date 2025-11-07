using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;

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
    /// Interaction logic for SupportTicketMaintenanceUserControl.xaml
    /// </summary>
    public partial class SupportTicketMaintenanceUserControl : ISupportTicketView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        public SupportTicketMaintenanceUserControl()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is SupportTicketHeaderControl supportTicketHeaderControl)
                {
                    supportTicketHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    supportTicketHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Ctrl + S, Ctrl + U)";
                    supportTicketHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Ticket. ";

                    supportTicketHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Ctrl + S, Ctrl + R)";
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

            var punchInHotKey = new HotKey(LocalViewModel.PunchInCommand);
            punchInHotKey.AddKey(Key.S);
            punchInHotKey.AddKey(Key.U);
            AddHotKey(punchInHotKey);

            var recalcHotKey = new HotKey(LocalViewModel.RecalcCommand);
            recalcHotKey.AddKey(Key.S);
            recalcHotKey.AddKey(Key.R);
            AddHotKey(recalcHotKey);
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
            return "Support Ticket";
        }

        protected override void ShowRecordTitle()
        {
            Host.ChangeTitle($"{Title} - {LocalViewModel.CustomerAutoFillValue.Text}");
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookup)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookup,
                CodeNameToFilter = "Support Ticket",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = OwnerWindow;
            genericWindow.ShowInTaskbar = false;
            genericWindow.ShowDialog();
            return genericWindow.ViewModel.DialogReesult;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookup)
        {
            var result = string.Empty;
            RecalcProcedure = new RecalcProcedure();
            RecalcProcedure.StartRecalculate += (sender, args) =>
            {
                result = LocalViewModel.StartRecalcProcedure(lookup, RecalcProcedure);
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
