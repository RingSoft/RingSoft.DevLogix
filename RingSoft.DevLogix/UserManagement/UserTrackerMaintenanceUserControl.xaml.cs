﻿using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Controls;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace RingSoft.DevLogix.UserManagement
{
    public class UserTrackerHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton RefreshNowButton { get; set; }

        static UserTrackerHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserTrackerHeaderControl)
                , new FrameworkPropertyMetadata(typeof(UserTrackerHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RefreshNowButton = GetTemplateChild(nameof(RefreshNowButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for UserTrackerMaintenanceUserControl.xaml
    /// </summary>
    public partial class UserTrackerMaintenanceUserControl : IUserTrackerView
    {
        public UserTrackerMaintenanceUserControl()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is UserTrackerHeaderControl userHeaderControl)
                {
                    userHeaderControl.RefreshNowButton.Command = LocalViewModel.RefreshNowCommand;
                    userHeaderControl.RefreshNowButton.ToolTip.HeaderText = "Refresh Now (Alt + R)";
                    userHeaderControl.RefreshNowButton.ToolTip.DescriptionText = "Refresh the data in the grid from the database.";
                }
            };

            RegisterFormKeyControl(NameControl);
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
            return "User Tracker";
        }

        public void SetAlertLevel(AlertLevels level, string message)
        {
            Dispatcher.Invoke(() =>
            {
                var win = Window.GetWindow(this);
                if (win == null)
                {
                    return;
                }
                LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(level, false
                    , win, message);
            });
        }

        public void RefreshGrid()
        {
            Dispatcher.Invoke(() =>
            {
                UsersGrid.RefreshGridView();
            });
        }
    }
}
