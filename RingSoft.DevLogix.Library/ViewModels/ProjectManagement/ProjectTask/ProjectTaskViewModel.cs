using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectTaskGrids
    {
        LaborPart = 1,
        Dependencies = 2,
    }

    public class UserLookup
    {
        public string ProjectUser { get; set; }

        public string Department { get; set; }
    }
    public interface IProjectTaskView : IDbMaintenanceView
    {
        void GetNewLineType(string text, out PrimaryKeyValue laborPartPkValue, out LaborPartLineTypes lineType);

        bool ShowCommentEditor(DataEntryGridMemoValue comment);

        void SetTaskReadOnlyMode(bool value);

        void PunchIn(ProjectTask projectTask);

        bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition);

        string StartRecalcProcedure(LookupDefinitionBase lookupDefinition);

        void UpdateRecalcProcedure(int currentProjectTask, int totalProjectTasks, string currentProjectTaskText);

        void SetupView();

        void SetDependencyRowFocus(int dependencyRowFocus);

        void SetLaborPartRowFocus(int rowId);

        void SetFocusToGrid(ProjectTaskGrids grid);
    }
    public class ProjectTaskViewModel : DbMaintenanceViewModel<ProjectTask>
    {
        #region Properties

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _projectAutoFillSetup;

        public AutoFillSetup ProjectAutoFillSetup
        {
            get => _projectAutoFillSetup;
            set
            {
                if (_projectAutoFillSetup == value)
                {
                    return;
                }
                _projectAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectAutoFillValue;

        public AutoFillValue ProjectAutoFillValue
        {
            get => _projectAutoFillValue;
            set
            {
                var oldValue = _projectAutoFillValue;

                if (_projectAutoFillValue == value)
                    return;

                if (_projectAutoFillValue == null)
                {
                    _originalProjectId = 0;
                }
                else 
                {
                    _originalProjectId = _projectAutoFillValue.GetEntity<Project>().Id;
                }

                _projectAutoFillValue = value;
                RefreshUserFilter();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }
                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                    return;

                _userAutoFillValue = value;
                SetUser();
                OnPropertyChanged();
            }
        }

        private double _minutesCost;

        public double MinutesCost
        {
            get => _minutesCost;
            set
            {
                if (_minutesCost == value)
                    return;

                _minutesCost = value;
                if (!_calculating)
                {
                    TimeEdited = true;
                }
                if (!_loading)
                {
                    RefreshTotals();
                }
                OnPropertyChanged(null, !_calculating);
            }
        }

        private double _totalMinutesCost;

        public double TotalMinutesCost
        {
            get => _totalMinutesCost;
            set
            {
                if (_totalMinutesCost == value)
                    return;

                _totalMinutesCost = value;
                TotalMinutesCostText = AppGlobals.MakeTimeSpent(value);
            }
        }

        private string _totalMinutesCostText;

        public string TotalMinutesCostText
        {
            get => _totalMinutesCostText;
            set
            {
                if (_totalMinutesCostText == value)
                    return;

                _totalMinutesCostText = value;
                OnPropertyChanged(null, false);
            }
        }

        private bool _timeEdited;

        public bool TimeEdited
        {
            get => _timeEdited;
            set
            {
                if (_timeEdited == value)
                    return;

                _timeEdited = value;
                OnPropertyChanged();
            }
        }


        private double _hourlyRate;

        public double HourlyRate
        {
            get => _hourlyRate;
            set
            {
                if (_hourlyRate == value)
                    return;

                _hourlyRate = value;
                if (!_loading)
                {
                    RefreshTotals();
                }

                OnPropertyChanged();
            }
        }

        private double _percentComplete;

        public double PercentComplete
        {
            get => _percentComplete;
            set
            {
                if (_percentComplete == value)
                    return;

                _percentComplete = value;
                if (!_loading)
                {
                    RefreshTotals();
                }
                OnPropertyChanged();
            }
        }

        private ProjectTaskLaborPartsManager _laborPartsManager;

        public ProjectTaskLaborPartsManager LaborPartsManager
        {
            get => _laborPartsManager;
            set
            {
                if (_laborPartsManager == value)
                    return;

                _laborPartsManager = value;
                OnPropertyChanged();
            }
        }

        private ProjectTaskDependencyManager _projectTaskDependencyManager;

        public ProjectTaskDependencyManager ProjectTaskDependencyManager
        {
            get => _projectTaskDependencyManager;
            set
            {
                if (_projectTaskDependencyManager == value)
                    return;

                _projectTaskDependencyManager = value;
                OnPropertyChanged();
            }
        }


        private LookupDefinition<TimeClockLookup, TimeClock> _timeClockLookup;

        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockLookup
        {
            get => _timeClockLookup;
            set
            {
                if (_timeClockLookup == value)
                    return;

                _timeClockLookup = value;
                OnPropertyChanged();
            }
        }

        private string? _notes;

        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        private ProjectTotalsManager _projectTotalsManager;

        public ProjectTotalsManager ProjectTotalsManager
        {
            get => _projectTotalsManager;
            set
            {
                if (_projectTotalsManager == value)
                    return;

                _projectTotalsManager = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AutoFillValue DefaultProjectAutoFillValue { get; private set; }

        public new IProjectTaskView View { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public UiCommand UserUiCommand { get; }

        public ProjectTotalsRow ActualRow { get; private set; }

        public ProjectTotalsRow StatusRow { get; private set; }

        private bool _calculating;
        private bool _loading;
        private int _dependencyRowFocusId = -1;
        private int _laborPartRowFocus = -1;
        private int _originalProjectId;

        private LookupDefinition<UserLookup, ProjectUser> _userLookupDefinition
            = new LookupDefinition<UserLookup, ProjectUser>(AppGlobals.LookupContext.ProjectUsers);

        public ProjectTaskViewModel()
        {
            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);

            _userLookupDefinition.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.ProjectUser
                    , "Project User"
                    , p => p.Name, 50);

            UserAutoFillSetup = new AutoFillSetup(_userLookupDefinition)
            {
                AllowLookupAdd = AppGlobals.LookupContext.Projects.CanAddToTable,
                AllowLookupView = AppGlobals.LookupContext.Projects.CanViewTable,
            };
            UserAutoFillSetup.LookupAdd += UserAutoFillSetup_LookupAdd;
            UserAutoFillSetup.LookupView += UserAutoFillSetup_LookupView;
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));

            ProjectTotalsManager = new ProjectTotalsManager();
            ProjectTotalsManager.Initialize();
            ActualRow = new ProjectTotalsRow(ProjectTotalsManager);
            ActualRow.RowTitle = "Actual";
            ProjectTotalsManager.InsertRow(ActualRow);

            StatusRow = new ProjectTotalsRow(ProjectTotalsManager);
            StatusRow.RowTitle = "Status";
            StatusRow.NegativeDisplayStyleId = ProjectTotalsManager.NegativeDisplayStyleId;
            StatusRow.PositiveDisplayStyleId = ProjectTotalsManager.PositiveDisplayStyleId;
            ProjectTotalsManager.InsertRow(StatusRow);

            LaborPartsManager = new ProjectTaskLaborPartsManager(this);
            ProjectTaskDependencyManager = new ProjectTaskDependencyManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.ProjectTaskLaborParts);
            TablesToDelete.Add(AppGlobals.LookupContext.ProjectTaskDependency);

            TimeClockLookup = AppGlobals.LookupContext.TimeClockTabLookup.Clone();
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            RegisterLookup(TimeClockLookup);

            UserUiCommand = new UiCommand();
        }

        private void UserAutoFillSetup_LookupView(object? sender, LookupAddViewArgs e)
        {
            e.FromLookupControl = true;
        }

        private void UserAutoFillSetup_LookupAdd(object? sender, LookupAddViewArgs e)
        {
            e.Handled = true;
            var inputParam= new ProjectInputParameter
            {
                AddToUsersGrid = true,
            };
            SystemGlobals.TableRegistry.ShowEditAddOnTheFly(ProjectAutoFillValue.PrimaryKeyValue, inputParam, e);
        }

        private void RefreshUserFilter()
        {
            if (ProjectAutoFillValue.IsValid())
            {
                _userLookupDefinition.FilterDefinition.ClearFixedFilters();
                var project = ProjectAutoFillValue.GetEntity<Project>();
                _userLookupDefinition.FilterDefinition
                    .AddFixedFilter(p => p.ProjectId, Conditions.Equals, project.Id);
            }
        }

        protected override void Initialize()
        {
            AppGlobals.MainViewModel.ProjectTaskViewModels.Add(this);
            if (base.View is IProjectTaskView projectTaskView)
            {
                View = projectTaskView;
            }

            Project project = null;
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Projects)
                {
                    project =
                        AppGlobals.LookupContext.Projects.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    project = project.FillOutProperties(false);
                    DefaultProjectAutoFillValue = project.GetAutoFillValue();
                }
            }

            if (DefaultProjectAutoFillValue.IsValid())
            {
                var defaultLookup = AppGlobals.LookupContext.ProjectTaskLookup.Clone();
                defaultLookup.FilterDefinition.AddFixedFilter(p => p.ProjectId
                    , Conditions.Equals, project.Id);
                var taskColumn = defaultLookup.GetColumnDefinition(p => p.ProjectName);
                defaultLookup.DeleteVisibleColumn(taskColumn);
                FindButtonLookupDefinition = defaultLookup;
                KeyAutoFillSetup.LookupDefinition = defaultLookup;
            }
            base.Initialize();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.ProjectTaskDependency)
            {
                var dependencyRow =
                    AppGlobals.LookupContext.ProjectTaskDependency.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);
                if (dependencyRow != null)
                {
                    _dependencyRowFocusId = dependencyRow.DependsOnProjectTaskId;
                }
            }

            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.ProjectTaskLaborParts)
            {
                var laborPartRow =
                    AppGlobals.LookupContext.ProjectTaskLaborParts.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);
                if (laborPartRow != null)
                {
                    _laborPartRowFocus = laborPartRow.DetailId;
                }
            }

            var result = base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
            return result;
        }

        protected override void PopulatePrimaryKeyControls(ProjectTask newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            PunchInCommand.IsEnabled = true;
        }

        protected override ProjectTask GetEntityFromDb(ProjectTask newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetProjectTask(newEntity.Id);

            var hasEditRight = TableDefinition.HasRight(RightTypes.AllowEdit);
            if (!hasEditRight)
            {
                if (result.UserId == AppGlobals.LoggedInUser.Id)
                {
                    SaveButtonEnabled = true;
                    View.SetTaskReadOnlyMode(false);
                    LaborPartsManager.SetDisplayMode(DisplayModes.User);
                }
                else
                {
                    SaveButtonEnabled = false;
                    View.SetTaskReadOnlyMode(true);
                    LaborPartsManager.SetDisplayMode(DisplayModes.Disabled);
                }
            }
            else
            {
                LaborPartsManager.SetDisplayMode(DisplayModes.All);
            }

            return result;
        }

        private ProjectTask? GetProjectTask(int taskId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.GetTable<ProjectTask>()
                .Include(p => p.User)
                .Include(p => p.Project)
                .ThenInclude(p => p.ProjectUsers)
                .Include(p => p.LaborParts)
                .ThenInclude(p => p.LaborPart)
                .Include(p => p.SourceDependencies)
                .ThenInclude(p => p.DependsOnProjectTask)
                .FirstOrDefault(p => p.Id == taskId);

            if (result == null)
            {
                result = new ProjectTask
                {
                    Id = taskId,
                };
            }

            return result;
        }

        protected override void LoadFromEntity(ProjectTask entity)
        {
            _loading = true;
            ProjectAutoFillValue = entity.Project.GetAutoFillValue();

            //UserAutoFillValue = entity.User.GetAutoFillValue();
            var projectUser = new ProjectUser
            {
                ProjectId = entity.ProjectId,
                UserId = entity.UserId,
            };
            var primaryKey = AppGlobals.LookupContext.ProjectUsers.GetPrimaryKeyValueFromEntity(projectUser);
            var text = entity.User.GetAutoFillValue().Text;
            UserAutoFillValue = new AutoFillValue(primaryKey, text);

            MinutesCost = entity.MinutesCost;
            PercentComplete = entity.PercentComplete;
            Notes = entity.Notes;
            TimeEdited = entity.MinutesEdited;
            HourlyRate = entity.HourlyRate;
            LaborPartsManager.LoadGrid(entity.LaborParts);
            LaborPartsManager.CalculateTotalMinutesCost();
            ProjectTaskDependencyManager.LoadGrid(entity.SourceDependencies);
            ActualRow.Minutes = entity.MinutesSpent;
            ActualRow.Cost = entity.Cost;
            LaborPartsManager.CalcPercentComplete();
            RefreshTotals();
            if (_dependencyRowFocusId >= 0)
            {
                View.SetDependencyRowFocus(_dependencyRowFocusId);
                _dependencyRowFocusId = -1;
            }

            if (_laborPartRowFocus >= 0)
            {
                View.SetLaborPartRowFocus(_laborPartRowFocus);
                _laborPartRowFocus = -1;
            }

            _loading = false;
        }

        protected override ProjectTask GetEntityData()
        {
            var result = new ProjectTask
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                ProjectId = ProjectAutoFillValue.GetEntity<Project>().Id,
                UserId = UserAutoFillValue.GetEntity<ProjectUser>().UserId,
                MinutesCost = MinutesCost,
                PercentComplete = PercentComplete,
                HourlyRate = HourlyRate,
                MinutesEdited = TimeEdited,
                Notes = Notes,
                EstimatedCost = ProjectTotalsManager.EstimatedRow.Cost,
            };
            if (!TableDefinition.HasRight(RightTypes.AllowEdit) && Entity != null)
            {
                result.Name = Entity.Name;
            }
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectTask>();
            var existTask = table.FirstOrDefault(p => p.Id == Id);
            if (existTask != null)
            {
                result.MinutesSpent = existTask.MinutesSpent;
                result.Cost = existTask.Cost;
            }

            return result;
        }

        protected override bool ValidateEntity(ProjectTask entity)
        {
            if (entity.ProjectId == 0)
            {
                return base.ValidateEntity(entity);
            }

            if (!UserAutoFillValue.IsValid(true))
            {
                UserAutoFillSetup.HandleValFail("Assigned To User");
                return false;
            }
            //var context = AppGlobals.DataRepository.GetDataContext();
            //var table = context.GetTable<ProjectUser>();
            //if (table != null)
            //{
            //    var projectUser = table.FirstOrDefault(p => p.UserId == entity.UserId
            //      && p.ProjectId == ProjectAutoFillValue.GetEntity<Project>().Id);
            //    if (projectUser == null)
            //    {
            //        var message =
            //            "The Assigned To User you have chosen has not been added to the project. Please select a user that has been added to the project.";
            //        var caption = "Invalid User";
            //        UserUiCommand.SetFocus();
            //        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            //        UserAutoFillSetup.Control.ShowLookupWindow();
            //        return false;
            //    }
            //}

            if (!ProjectTaskDependencyManager.ValidateGrid())
            {
                return false;
            }
            if (!ProjectTaskDependencyManager.ValidateCircular())
            {
                return false;
            }

            if (!LaborPartsManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override void ClearData()
        {
            _loading = true;
            Id = 0;
            ProjectAutoFillValue = DefaultProjectAutoFillValue;
            UserAutoFillValue = null;
            ActualRow.ClearData();
            StatusRow.ClearData();
            MinutesCost = 0;
            PercentComplete = 0;
            Notes = string.Empty;
            TimeEdited = false;
            HourlyRate = 0;
            PunchInCommand.IsEnabled = false;
            LaborPartsManager.SetupForNewRecord();
            LaborPartsManager.CalculateTotalMinutesCost();
            ProjectTaskDependencyManager.SetupForNewRecord();
            View.SetupView();
            _loading = false;
        }

        protected override bool SaveEntity(ProjectTask entity)
        {
            var result = base.SaveEntity(entity);
            if (!result)
            {
                return result;
            }
            var details = LaborPartsManager.GetEntityList();
            var dependencies = ProjectTaskDependencyManager.GetEntityList();

            var context = SystemGlobals.DataRepository.GetDataContext();

            foreach (var projectTaskDependency in dependencies)
            {
                projectTaskDependency.ProjectTaskId = entity.Id;
            }

            var dependencyTable = context.GetTable<ProjectTaskDependency>();
            context.RemoveRange(dependencyTable.Where(p => p.ProjectTaskId == entity.Id));
            context.AddRange(dependencies);

            LaborPartsManager.SaveNoCommitData(entity, context);

            result = context.Commit("Saving Project Task");

            if (result)
            {
                var projectId = ProjectAutoFillValue.GetEntity<Project>().Id;
                var projectViewModels = AppGlobals.MainViewModel.ProjectViewModels.Where(p => p.Id == projectId);
                foreach (var projectMaintenanceViewModel in projectViewModels)
                {
                    projectMaintenanceViewModel.CalcTotals();
                }
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();

            var entity = new ProjectTask
            {
                Id = Id,
            };
            entity = entity.FillOutProperties(false);
            //var entity = context.GetTable<ProjectTask>()
            //    .FirstOrDefault(p => p.Id == Id);

            LaborPartsManager.DeleteNoCommitData(entity, context);

            var dependencyTable = context.GetTable<ProjectTaskDependency>();
            context.RemoveRange(dependencyTable.Where(p => p.ProjectTaskId == entity.Id));

            return context.DeleteEntity(entity, "Deleting Project Task");
        }

        public void SetTotalMinutesCost(double total)
        {
            _calculating = true;
            TotalMinutesCost = total;
            if (!TimeEdited)
            {
                MinutesCost = total;
            }

            RefreshTotals();
            
            _calculating = false;
        }

        private void RefreshTotals()
        {
            var hourCost = MinutesCost / 60;
            ProjectTotalsManager.EstimatedRow.Minutes = MinutesCost;
            ProjectTotalsManager.EstimatedRow.Cost = hourCost * HourlyRate;

            ProjectTotalsManager.RemainingRow.Minutes = MinutesCost * (1 - PercentComplete);
            ProjectTotalsManager.RemainingRow.Cost = ProjectTotalsManager.EstimatedRow.Cost * (1 - PercentComplete);

            StatusRow.Minutes = MinutesCost
                                - (ActualRow.Minutes + ProjectTotalsManager.RemainingRow.Minutes);
            StatusRow.Cost = ProjectTotalsManager.EstimatedRow.Cost - (ActualRow.Cost + ProjectTotalsManager.RemainingRow.Cost);

            ProjectTotalsManager.RefreshGrid();
        }

        private void SetUser()
        {
            if (!_loading && UserAutoFillValue.IsValid())
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                var user = UserAutoFillValue.GetEntity<User>();
                if (user != null)
                {
                    user = context.GetTable<User>()
                        .FirstOrDefault(p => p.Id == user.Id);
                }

                if (user != null)
                {
                    HourlyRate = user.HourlyRate;
                }
            }
        }

        public void PunchIn()
        {
            var projectTask = GetProjectTask(Id);
            if (projectTask != null)
            {
                var projectUser = projectTask.Project.ProjectUsers
                    .FirstOrDefault(p => p.UserId == AppGlobals.LoggedInUser.Id);
                if (projectUser == null)
                {
                    var message =
                        "You're not listed as a user to this project.  Please contact someone who has the right to edit projects to add you as a user to this project in order to punch in.";
                    var caption = "Punch In Validation";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                }
                else
                {
                    View.PunchIn(projectTask);
                }
            }
        }

        public void Recalc()
        {
            var recalcFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(recalcFilter))
                return;

            var result = View.StartRecalcProcedure(recalcFilter);
            if (result.IsNullOrEmpty())
            {
                var message = "Recalculation complete.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Recalculation", RsMessageBoxIcons.Information);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Recalculation Error", RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase recalcFilter)
        {
            var result = string.Empty;
            DbDataProcessor.DontDisplayExceptions = true;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(recalcFilter, false);
            var recordCount = lookupData.GetRecordCount();
            var currentProjectTask = 1;
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectTaskTable = context.GetTable<ProjectTask>();
            var timeClocksTable = context.GetTable<TimeClock>();
            lookupData.PrintOutput += (sender, args) =>
            {
                foreach (var primaryKeyValue in args.Result)
                {
                    var projectTaskPrimaryKey = primaryKeyValue;
                    if (projectTaskPrimaryKey.IsValid())
                    {
                        var projectTask = TableDefinition.GetEntityFromPrimaryKeyValue(projectTaskPrimaryKey);
                        projectTask = projectTaskTable.FirstOrDefault(p => p.Id == projectTask.Id);
                        if (projectTask != null)
                        {
                            var timeClocks = timeClocksTable.Where(p => p.ProjectTaskId == projectTask.Id
                            && p.MinutesSpent != null).ToList();
                            var actualMinutes = timeClocks.Sum(p => p.MinutesSpent.Value);
                            projectTask.MinutesSpent = actualMinutes;
                            projectTask.Cost = (actualMinutes / 60) * projectTask.HourlyRate;
                            if (!context.SaveNoCommitEntity(projectTask, "Updating Project Task"))
                            {
                                result = DbDataProcessor.LastException;
                                args.Abort = true;
                                return;
                            }

                            if (projectTask.Id == Id)
                            {
                                RefreshCostGrid(projectTask);
                            }
                            View.UpdateRecalcProcedure(currentProjectTask, recordCount, projectTask.Name);
                        }

                    }
                }
            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (!context.Commit("Saving Project Tasks"))
                {
                    result = DbDataProcessor.LastException;
                }
            }


            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        public void RefreshCostGrid(ProjectTask projectTask)
        {
            ActualRow.Minutes = projectTask.MinutesSpent;
            ActualRow.Cost = projectTask.Cost;
            RefreshTotals();
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            //DbDataProcessor.ShowSqlStatementWindow();
            AppGlobals.MainViewModel.ProjectTaskViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        public async Task<bool> ValidateProjectChange()
        {
            var projectId = ProjectAutoFillValue.GetEntity<Project>().Id;
            if (!_loading && projectId != ProjectTaskDependencyManager.ProjectFilter)
            {
                var result = await ProjectTaskDependencyManager.ValidateProjectChange();
                return result;
            }
            return true;
        }

        public bool ValidateDependencyGridFocus()
        {
            if (!ProjectAutoFillValue.IsValid())
            {
                var message = "You must first select a valid project before adding dependencies.";
                var caption = "Grid Focus Validation Fail";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }
        public void RefreshTimeClockLookup()
        {
            TimeClockLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh));
        }

    }
}
