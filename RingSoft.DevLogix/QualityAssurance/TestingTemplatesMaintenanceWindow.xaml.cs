﻿using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingTemplateHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton UpdateOutlinesButton { get; set; }

        static TestingTemplateHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingTemplateHeaderControl), new FrameworkPropertyMetadata(typeof(TestingTemplateHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            UpdateOutlinesButton = GetTemplateChild(nameof(UpdateOutlinesButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for TestingTemplatesMaintenanceWindow.xaml
    /// </summary>
    public partial class TestingTemplatesMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Testing Template";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public TestingTemplatesMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is TestingTemplateHeaderControl templateHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        templateHeaderControl.UpdateOutlinesButton.Visibility = Visibility.Collapsed;
                    }
                    templateHeaderControl.UpdateOutlinesButton.Command = LocalViewModel.UpdateOutlinesCommand;
                    templateHeaderControl.UpdateOutlinesButton.ToolTip.HeaderText = "Update Testing Outlines (Alt + U)";
                    templateHeaderControl.UpdateOutlinesButton.ToolTip.DescriptionText =
                        "Generate Steps for the attached Testing Outlines";
                }
            };

        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
