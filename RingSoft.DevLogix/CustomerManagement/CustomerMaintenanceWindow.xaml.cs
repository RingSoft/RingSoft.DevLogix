using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
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
    public partial class CustomerMaintenanceWindow : ICustomerView
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

        public void RefreshView()
        {
            var textBlock = SendEmailControl;
            if (LocalViewModel.EmailAddress.IsNullOrEmpty())
            {
                SendEmailControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                SendEmailControl.Visibility = Visibility.Visible;
                SendEmailControl.Inlines.Clear();
                var url = $"mailto:{LocalViewModel.EmailAddress}";
                SetupUrl(textBlock, url, "Send Email");

            }

            textBlock = ClickWebControl;
            if (LocalViewModel.WebAddress.IsNullOrEmpty())
            {
                ClickWebControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                ClickWebControl.Visibility = Visibility.Visible;
                var prefix = "http://www.";
                if (LocalViewModel.WebAddress.Contains(prefix))
                {
                    prefix = string.Empty;
                }
                ClickWebControl.Inlines.Clear();
                var url = $"{prefix}{LocalViewModel.WebAddress}";
                SetupUrl(textBlock, url, "Goto Web Site");
            }
        }

        private void SetupUrl(TextBlock textBlock, string url, string text)
        {
            try
            {
                var uri= new Uri(url);
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
    }
}
