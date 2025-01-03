﻿using RingSoft.App.Controls;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingOutlineHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton GenerateDetailsButton { get; set; }

        public DbMaintenanceButton RetestButton { get; set; }

        public DbMaintenanceButton PunchInButton { get; set; }

        public DbMaintenanceButton RecalcButton { get; set; }

        static TestingOutlineHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingOutlineHeaderControl), new FrameworkPropertyMetadata(typeof(TestingOutlineHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            GenerateDetailsButton = GetTemplateChild(nameof(GenerateDetailsButton)) as DbMaintenanceButton;
            RetestButton = GetTemplateChild(nameof(RetestButton)) as DbMaintenanceButton;
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as DbMaintenanceButton;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }
    /// <summary>
    /// Interaction logic for TestingOutlineMaintenanceUserControl.xaml
    /// </summary>
    public partial class TestingOutlineMaintenanceUserControl : ITestingOutlineView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        public TestingOutlineMaintenanceUserControl()
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

                    outlineHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Alt + U)";
                    outlineHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Testing Outline. ";

                    outlineHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Alt + E)";
                    outlineHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of Testing Outlines.";

                    outlineHeaderControl.GenerateDetailsButton.ToolTip.HeaderText = "Generate Steps (Alt + G)";
                    outlineHeaderControl.GenerateDetailsButton.ToolTip.DescriptionText =
                        "Generate Steps from the attached Testing Templates.";

                    outlineHeaderControl.RetestButton.ToolTip.HeaderText = "Retest Testing Outline (Alt + R)";
                    outlineHeaderControl.RetestButton.ToolTip.DescriptionText =
                        "Clear all completed values in the Steps grid.";
                }
            };
            ProductControl.PreviewLostKeyboardFocus += async (sender, args) =>
            {
                if ((WPFControlsGlobals.ActiveWindow is TestingOutlineMaintenanceWindow))
                {
                    if (!await LocalViewModel.ChangeProduct())
                    {
                        args.Handled = true;
                    }
                }
            };
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
            return "Testing Outline";
        }

        public void PunchIn(TestingOutline testingOutline)
        {
            AppGlobals.MainViewModel.PunchIn(testingOutline);
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
