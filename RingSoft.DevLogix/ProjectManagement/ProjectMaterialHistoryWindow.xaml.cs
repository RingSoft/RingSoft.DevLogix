﻿using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialHistoryWindow.xaml
    /// </summary>
    public partial class ProjectMaterialHistoryWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Material History";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ProjectMaterialHistoryWindow()
        {
            InitializeComponent();
        }
    }
}
