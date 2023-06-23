using System;
using System.Linq;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.UserManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectTaskHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchInButton { get; set; }

        public DbMaintenanceButton RecalcButton { get; set; }

        static ProjectTaskHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectTaskHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as DbMaintenanceButton;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProjectTaskMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectTaskMaintenanceWindow : IProjectTaskView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Task";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
        public RecalcProcedure RecalcProcedure { get; set; }

        private bool _settingUserFocus;
        private int _dependencyRowFocus = -1;
        private int _laborPartRowFocus = -1;

        public ProjectTaskMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProjectTaskHeaderControl projectHeaderControl)
                {
                    projectHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    projectHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Alt + U)";
                    projectHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Project Task. ";

                    projectHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost (Alt + R)";
                    projectHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of project tasks.";


                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        projectHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }
                    projectHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };


            UserControl.GotFocus += (sender, args) =>
            {
                if (!LocalViewModel.ProjectAutoFillValue.IsValid() && !_settingUserFocus)
                {
                    _settingUserFocus = true;
                    var message = "You must first select a valid project";
                    var caption = "Invalid Project";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    _settingUserFocus = false;
                    ProjectControl.Focus();
                }
            };

            var showingLookup = false;
            ProjectControl.LookupShown += (sender, args) =>
            {
                showingLookup = true;
                args.LookupWindow.Closed += (o, eventArgs) =>
                {
                    showingLookup = false;
                };
            };

            ProjectControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!showingLookup && !LocalViewModel.ValidateProjectChange())
                {
                    args.Handled = true;
                }
            };
            DependenciesGrid.PreviewGotKeyboardFocus += (sender, args) =>
            {
                if (!DependenciesGrid.IsKeyboardFocusWithin && !LocalViewModel.ValidateDependencyGridFocus())
                {
                    SetFocusToTab(DetailsTabItem);
                    ProjectControl.TextBox.Focus();
                    args.Handled = true;
                }
            };

            DependenciesGrid.Loaded += (sender, args) =>
            {
                if (_dependencyRowFocus >= 0)
                {
                    TabControl.SelectedItem = DependenciesTab;
                    var rows = LocalViewModel.ProjectTaskDependencyManager.Rows.OfType<ProjectTaskDependencyRow>();
                    var row = rows.FirstOrDefault(p => p.DependencyTaskId == _dependencyRowFocus);
                    DependenciesGrid.GotoCell(row, 1);
                    _dependencyRowFocus = -1;
                }
            };

            LaborPartsGrid.Loaded += (sender, args) =>
            {
                if (_laborPartRowFocus >= 0)
                {
                    TabControl.SelectedItem = LaborPartsTabItem;
                    var row = LocalViewModel.LaborPartsManager.Rows[_laborPartRowFocus];
                    LaborPartsGrid.GotoCell(row, ProjectTaskLaborPartsManager.LaborPartColumnId);
                    _laborPartRowFocus = -1;
                }
            };
        }

        
        protected override void OnLoaded()
        {
            RegisterFormKeyControl(KeyControl);
            base.OnLoaded();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.ProjectId))
            {
                SetFocusToTab(DetailsTabItem);
                ProjectControl.Focus();
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.UserId))
            {
                SetFocusToTab(DetailsTabItem);
                _settingUserFocus = true;
                UserControl.Focus();
                _settingUserFocus = false;
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.Name))
            {
                KeyControl.Focus();
            }

            base.OnValidationFail(fieldDefinition, text, caption);
        }

        private void SetFocusToTab(TabItem tabItem)
        {
            TabControl.SelectedItem = tabItem;
            tabItem.UpdateLayout();
        }

    public override void ResetViewForNewRecord()
        {
            //TabControl.SelectedItem = DetailsTabItem;
            KeyControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void GetNewLineType(string text, out PrimaryKeyValue laborPartPkValue, out LaborPartLineTypes lineType)
        {
            var result = LaborPartLineTypes.LaborPart;
            var window = new TaskLaborPartLtSelectorWindow(text);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            laborPartPkValue = window.ViewModel.NewLaborPartPkValue;
            lineType = window.ViewModel.NewLineType;

        }
        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }

        public void SetTaskReadOnlyMode(bool value)
        {
            UserControl.SetReadOnlyMode(true);
            ProjectControl.SetReadOnlyMode(true);
            HourlyRateControl.IsEnabled = false;
            TimeControl.IsEnabled = false;
            LaborPartsGrid.SetReadOnlyMode(value);
            PercentCompleteControl.IsEnabled = !value;
            NotesControl.SetReadOnlyMode(value);
        }

        public void PunchIn(ProjectTask projectTask)
        {
            AppGlobals.MainViewModel.MainView.PunchIn(projectTask);
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Project Task",
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

        public void UpdateRecalcProcedure(int currentProjectTask, int totalProjectTasks, string currentProjectTaskText)
        {
            var progress = $"Recalculating Project {currentProjectTaskText} {currentProjectTask} / {totalProjectTasks}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }

        public void SetupView()
        {
            if (LocalViewModel.ProjectAutoFillValue.IsValid())
            {
                DependenciesGrid.IsEnabled = true;
            }
            else
            {
                //DependenciesGrid.IsEnabled = false;
                ResetViewForNewRecord();
            }
        }

        public void SetDependencyRowFocus(int dependencyRowFocus)
        {
            _dependencyRowFocus = dependencyRowFocus;
        }

        public void SetLaborPartRowFocus(int rowId)
        {
            _laborPartRowFocus = rowId;
        }

        public void SetFocusToGrid(ProjectTaskGrids grid)
        {
            switch (grid)
            {
                case ProjectTaskGrids.LaborPart:
                    SetFocusToTab(LaborPartsTabItem);
                    LaborPartsGrid.Focus();
                    break;
                case ProjectTaskGrids.Dependencies:
                    SetFocusToTab(DependenciesTab);
                    DependenciesGrid.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(grid), grid, null);
            }
        }
    }
}
