﻿using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.CustomerManagement
{
    /// <summary>
    /// Interaction logic for TerritoryMaintenanceUserControl.xaml
    /// </summary>
    public partial class TerritoryMaintenanceUserControl
    {
        public TerritoryMaintenanceUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);
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
            return "Territory";
        }
    }
}