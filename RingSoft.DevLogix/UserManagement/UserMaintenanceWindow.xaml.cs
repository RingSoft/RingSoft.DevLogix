using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    public class UserHeaderControl : DbMaintenanceCustomPanel
    {
        public Button ClockOutButton { get; set; }

        public Button ChangeChartButton { get; set; }

        public Button RecalcButton { get; set; }

        static UserHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserHeaderControl), new FrameworkPropertyMetadata(typeof(UserHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            ClockOutButton = GetTemplateChild(nameof(ClockOutButton)) as Button;
            ChangeChartButton = GetTemplateChild(nameof(ChangeChartButton)) as Button;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for UserMaintenanceWindow.xaml
    /// </summary>
    public partial class UserMaintenanceWindow : IUserView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        public UserMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is UserHeaderControl userHeaderControl)
                {
                    userHeaderControl.ChangeChartButton.Command =
                        UserMaintenanceViewModel.ChangeChartCommand;

                    userHeaderControl.ClockOutButton.Command = UserMaintenanceViewModel.ClockOutCommand;

                    userHeaderControl.RecalcButton.Command = UserMaintenanceViewModel.RecalcCommand;

                    if (!UserMaintenanceViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        userHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }
                }
            };
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "User";
        public override DbMaintenanceViewModelBase ViewModel => UserMaintenanceViewModel;

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            RefreshView();
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            RefreshView();
            base.ResetViewForNewRecord();
        }

        public string GetRights()
        {
            return RightsTree.GetRights();
        }

        public void LoadRights(string rightsString)
        {
            RightsTree.LoadRights(rightsString);
        }

        public void ResetRights()
        {
            RightsTree.Reset();
        }

        public void RefreshView()
        {
            if (UserMaintenanceViewModel.EmailAddress.IsNullOrEmpty())
            {
                SendEmailControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                SendEmailControl.Visibility = Visibility.Visible;
                SendEmailControl.Inlines.Clear();
                try
                {
                    var uri = new Uri($"mailto:{UserMaintenanceViewModel.EmailAddress}");
                    var hyperLink = new Hyperlink
                    {
                        NavigateUri = uri
                    };
                    hyperLink.Inlines.Add("Send Email");
                    hyperLink.RequestNavigate += (sender, args) =>
                    {
                        Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
                        args.Handled = true;
                    };
                    SendEmailControl.Inlines.Add(hyperLink);
                }
                catch (Exception e)
                {
                    SendEmailControl.Visibility = Visibility.Collapsed;
                }
            }

            GroupsTab.Visibility = !AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowView) ? Visibility.Collapsed : Visibility.Visible;
        }

        public void OnValGridFail()
        {
            TabControl.SelectedItem = GroupsTab;
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "User",
                KeyAutoFillValue = UserMaintenanceViewModel.KeyAutoFillValue,
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
                result = UserMaintenanceViewModel.StartRecalculateProcedure(lookupDefinition);
            };
            RecalcProcedure.Start();
            return result;
        }

        public void UpdateRecalcProcedure(int currentUser, int totalUsers, string currentUserText)
        {
            var progress = $"Recalculating User {currentUserText} {currentUser} / {totalUsers}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }


        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == UserMaintenanceViewModel.TableDefinition.GetFieldDefinition(p => p.DepartmentId))
            {
                TabControl.SelectedItem = DetailsTabItem;
                TabControl.UpdateLayout();
                DepartmentControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
