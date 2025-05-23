﻿using RingSoft.App.Controls;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DevLogix.UserManagement
{

    /// <summary>
    /// Interaction logic for UserTrackerMaintenanceWindow.xaml
    /// </summary>
    public partial class UserTrackerMaintenanceWindow : IUserTrackerView
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "User Tracker";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        private bool _closed;

        public UserTrackerMaintenanceWindow()
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

            Closed += (sender, args) =>
            {
                _closed = true;
            };
            RegisterFormKeyControl(NameControl);
        }

        public void SetAlertLevel(AlertLevels level, string message)
        {
            if (_closed)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(level, false
                    , this, message);
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
