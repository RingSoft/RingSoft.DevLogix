using System;
using System.Runtime.InteropServices;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DevLogix.QualityAssurance;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectHeaderControl : DbMaintenanceCustomPanel
    {
        public Button RecalcButton { get; set; }

        public Button CalcDeadlineButton { get; set; }

        static ProjectHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as Button;
            CalcDeadlineButton = GetTemplateChild(nameof(CalcDeadlineButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProjectMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaintenanceWindow : IProjectView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
        public RecalcProcedure RecalcProcedure { get; set; }

        private int _userFocus = -1;

        public ProjectMaintenanceWindow()
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

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();

            base.ResetViewForNewRecord();
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
            window.Owner = this;
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
