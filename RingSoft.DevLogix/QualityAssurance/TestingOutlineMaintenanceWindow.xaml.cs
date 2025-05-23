﻿using RingSoft.App.Controls;
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

    /// <summary>
    /// Interaction logic for TestingOutlineMaintenanceWindow.xaml
    /// </summary>
    public partial class TestingOutlineMaintenanceWindow : ITestingOutlineView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Testing Outline";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override Control MaintenanceButtonsControl => TopHeaderControl;
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
            genericWindow.Owner = this;
            genericWindow.ShowInTaskbar = false;
            genericWindow.ShowDialog();
            return genericWindow.ViewModel.DialogReesult;

        }

        public bool ProcessRetestLookupFilter(RetestInput input)
        {
            throw new NotImplementedException();
        }

        public string StartRetestProcedure(RetestInput input)
        {
            throw new NotImplementedException();
        }

        public void UpdateRetestProcedure(int currentOutline, int totalOutlines, string currentOutlineText)
        {
            throw new NotImplementedException();
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

        public bool IsErrorsTabSelected()
        {
            throw new NotImplementedException();
        }

        public void PlayExclSound()
        {
            throw new NotImplementedException();
        }

        public void SelectTab(SetFocusTabs tab)
        {
            throw new NotImplementedException();
        }
    }
}
