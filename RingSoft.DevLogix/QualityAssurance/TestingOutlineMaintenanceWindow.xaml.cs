using RingSoft.App.Controls;
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
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingOutlineHeaderControl : DbMaintenanceCustomPanel
    {
        public Button GenerateDetailsButton { get; set; }

        public Button RetestButton { get; set; }

        public Button PunchInButton { get; set; }

        public Button RecalcButton { get; set; }

        static TestingOutlineHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingOutlineHeaderControl), new FrameworkPropertyMetadata(typeof(TestingOutlineHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            GenerateDetailsButton = GetTemplateChild(nameof(GenerateDetailsButton)) as Button;
            RetestButton = GetTemplateChild(nameof(RetestButton)) as Button;
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as Button;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as Button;

            base.OnApplyTemplate();
        }
    }
    /// <summary>
    /// Interaction logic for TestingOutlineMaintenanceWindow.xaml
    /// </summary>
    public partial class TestingOutlineMaintenanceWindow : ITestingOutlineView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Testing Outline";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public RecalcProcedure RecalcProcedure { get; set; }
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public TestingOutlineMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is TestingOutlineHeaderControl outlineHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        outlineHeaderControl.GenerateDetailsButton.Visibility = Visibility.Collapsed;
                        outlineHeaderControl.RetestButton.Visibility = Visibility.Collapsed;
                    }
                    outlineHeaderControl.GenerateDetailsButton.Command = LocalViewModel.GenerateDetailsCommand;
                    outlineHeaderControl.RetestButton.Command = LocalViewModel.RetestCommand;
                    outlineHeaderControl.PunchInButton.Command = LocalViewModel.PunchInCommand;
                    outlineHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };
            ProductControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if ((WPFControlsGlobals.ActiveWindow is TestingOutlineMaintenanceWindow))
                {
                    if (!LocalViewModel.ChangeProduct())
                    {
                        args.Handled = true;
                    }
                }
            };
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void PunchIn(TestingOutline testingOutline)
        {
            AppGlobals.MainViewModel.MainView.PunchIn(testingOutline);
        }

        public bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookup,
                CodeNameToFilter = "Testing Outline",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = this;
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
                result = LocalViewModel.StartRecalcProcedure(lookup);
            };
            RecalcProcedure.Start();
            return result;
        }

        public void UpdateRecalcProcedure(int currentOutline, int totalOutlines, string currentOutlineText)
        {
            var progress = $"Recalculating Testing Outline {currentOutlineText} {currentOutline} / {totalOutlines}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }
    }
}
