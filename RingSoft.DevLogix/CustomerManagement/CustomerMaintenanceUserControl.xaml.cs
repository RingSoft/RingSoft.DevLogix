using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
    /// Interaction logic for CustomerMaintenanceUserControl.xaml
    /// </summary>
    public partial class CustomerMaintenanceUserControl : ICustomerView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        public VmUiControl SendEmailUiControl { get; }

        public VmUiControl ClickWebUiControl { get; }

        public CustomerMaintenanceUserControl()
        {
            InitializeComponent();
            SendEmailUiControl = new VmUiControl(SendEmailControl, LocalViewModel.SendEmailUiCommand);
            ClickWebUiControl = new VmUiControl(ClickWebControl, LocalViewModel.ClickWebUiCommand);

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is CustomerHeaderControl customerHeaderControl)
                {
                    customerHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    customerHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Ctrl + S, Ctrl + U)";
                    customerHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Customer. ";

                    customerHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Ctrl + S, Ctrl + R)";
                    customerHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of customers.";


                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        customerHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }

                    customerHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };
            RegisterFormKeyControl(CompanyControl);

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
            return "Customer";
        }

        public void RefreshView()
        {
            var textBlock = SendEmailControl;
            if (!LocalViewModel.EmailAddress.IsNullOrEmpty())
            {
                SendEmailControl.Inlines.Clear();
                var url = $"mailto:{LocalViewModel.EmailAddress}";
                SetupUrl(textBlock, url, "Send Email");
            }

            textBlock = ClickWebControl;
            if (!LocalViewModel.WebAddress.IsNullOrEmpty())
            {
                var prefix = "http://www.";
                if (LocalViewModel.WebAddress.Contains(prefix))
                {
                    prefix = string.Empty;
                }
                ClickWebControl.Inlines.Clear();
                var url = $"{prefix}{LocalViewModel.WebAddress}";
                SetupUrl(textBlock, url, "Goto Web Site");
            }

            if (LocalViewModel.SalesDifference < 0)
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.LightPink);
            }
            else if (LocalViewModel.SalesDifference > 0)
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                DifferenceControl.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SetupUrl(TextBlock textBlock, string url, string text)
        {
            try
            {
                var uri = new Uri(url);
                var hyperLink = new Hyperlink
                {
                    NavigateUri = uri
                };
                hyperLink.Inlines.Add(text);
                hyperLink.RequestNavigate += (sender, args) =>
                {
                    Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
                    args.Handled = true;
                };
                textBlock.Inlines.Add(hyperLink);
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    e.Message
                    , "Internet Error"
                    , RsMessageBoxIcons.Error);
                textBlock.Visibility = Visibility.Collapsed;
            }
        }
        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Customer",
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

        public void UpdateRecalcProcedure(int currentCustomer, int totalCustomers, string currentCustomerText)
        {
            var progress = $"Recalculating Customer {currentCustomerText} {currentCustomer} / {totalCustomers}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }

        public void SelectGrid(CustomerGrids grid)
        {
            switch (grid)
            {
                case CustomerGrids.Products:
                    TabControl.SelectedItem = ProductsTabItem;
                    break;
                case CustomerGrids.Sales:
                    TabControl.SelectedItem = SalesTabItem;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(grid), grid, null);
            }
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (readOnlyValue)
            {
                if (control == AddModifyOrderButton && !AppGlobals.LookupContext.Order.CanEditTabe)
                {
                    AddModifyOrderButton.Visibility = Visibility.Collapsed;
                    return;
                }

                if (control == AddModifyComputerButton && !AppGlobals.LookupContext.CustomerComputer.CanEditTabe)
                {
                    AddModifyComputerButton.Visibility = Visibility.Collapsed;
                    return;
                }

                if (control == AddModifyTicketsButton && !AppGlobals.LookupContext.SupportTicket.CanEditTabe)
                {
                    AddModifyTicketsButton.Visibility = Visibility.Collapsed;
                    return;
                }
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
