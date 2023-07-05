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
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;

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

        public SupportTicketHeaderControl()
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
    /// Interaction logic for SupportTicketMaintenanceWindow.xaml
    /// </summary>
    public partial class SupportTicketMaintenanceWindow
    {
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

        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Support Ticket";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(TicketIdControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
