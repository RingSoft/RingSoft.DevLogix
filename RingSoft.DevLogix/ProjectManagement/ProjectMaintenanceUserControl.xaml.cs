﻿using System;
using RingSoft.App.Controls;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton RecalcButton { get; set; }

        public DbMaintenanceButton CalcDeadlineButton { get; set; }

        static ProjectHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;
            CalcDeadlineButton = GetTemplateChild(nameof(CalcDeadlineButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProjectMaintenanceUserControl.xaml
    /// </summary>
    public partial class ProjectMaintenanceUserControl : IProjectView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        private int _userFocus = -1;

        public ProjectMaintenanceUserControl()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProjectHeaderControl projectHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        projectHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                        projectHeaderControl.CalcDeadlineButton.Visibility = Visibility.Collapsed;
                    }
                    projectHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                    projectHeaderControl.CalcDeadlineButton.Command = LocalViewModel.CalculateDeadlineCommand;

                    projectHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost";
                    projectHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of projects.";

                    projectHeaderControl.CalcDeadlineButton.ToolTip.HeaderText = "Calculate Deadline";
                    projectHeaderControl.CalcDeadlineButton.ToolTip.DescriptionText =
                        "Calculate the projected deadline for the current project.";
                }
            };

            UsersGrid.Loaded += (sender, args) =>
            {
                if (_userFocus >= 0)
                {
                    TabControl.SelectedItem = UsersTab;
                    var row = LocalViewModel.UsersGridManager.GetProjectUsersGridRow(_userFocus);
                    UsersGrid.GotoCell(row, ProjectUsersGridManager.UserColumnId);
                    _userFocus = -1;
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
            return "Project";
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Project",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = OwnerWindow;
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
                result = LocalViewModel.StartRecalcProcedure(lookupDefinition);
            };
            RecalcProcedure.Start();
            return result;
        }

        public void UpdateRecalcProcedure(int currentProject, int totalProjects, string currentProjectText)
        {
            var progress = $"Recalculating Project {currentProjectText} {currentProject} / {totalProjects}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }

        public void SetExistRecordFocus(int userId)
        {
            _userFocus = userId;
        }

        public void GotoGrid()
        {
            TabControl.SelectedItem = UsersTab;
        }

        public DateTime? GetDeadline()
        {
            var window = new ProjectScheduleWindow(LocalViewModel.Entity, DateTime.Today);
            window.Owner = OwnerWindow;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            if (window.LocalViewModel.DialogResult)
            {
                return window.LocalViewModel.CalculatedDeadline;
            }
            return null;
        }

        public void GotoTasksTab()
        {
            TabControl.SelectedItem = TasksTab;
            TabControl.UpdateLayout();
            TasksTab.UpdateLayout();
        }
    }
}
