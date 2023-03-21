using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using MySqlX.XDevAPI.Common;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.DataProcessor;
using System.Data;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectTaskGrids
    {
        LaborPart = 1,
        Dependencies = 2,
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
    public class ProjectTaskViewModel : DevLogixDbMaintenanceViewModel<ProjectTask>
    {
        public override TableDefinition<ProjectTask> TableDefinition => AppGlobals.LookupContext.ProjectTasks;

        public override bool SetReadOnlyMode => false;

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
                SetUserFilter();
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

        private decimal _minutesCost;

        public decimal MinutesCost
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

        private decimal _totalMinutesCost;

        public decimal TotalMinutesCost
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


        private decimal _hourlyRate;

        public decimal HourlyRate
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

        private decimal _percentComplete;

        public decimal PercentComplete
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

        private LookupCommand _timeClockLookupCommand;

        public LookupCommand TimeClockLookupCommand
        {
            get => _timeClockLookupCommand;
            set
            {
                if (_timeClockLookupCommand == value)
                    return;

                _timeClockLookupCommand = value;
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


        public AutoFillValue DefaultProjectAutoFillValue { get; private set; }

        public new IProjectTaskView View { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public ProjectTotalsRow ActualRow { get; private set; }

        public ProjectTotalsRow StatusRow { get; private set; }

        private bool _calculating;
        private bool _loading;
        private int _originalProjectId;
        private int _dependencyRowFocusId = -1;
        private int _laborPartRowFocus = -1;

        public ProjectTaskViewModel()
        {
            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);

            UserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.UserId))
            {
                AllowLookupAdd = false,
            };
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));

            ProjectTotalsManager = new ProjectTotalsManager();

            LaborPartsManager = new ProjectTaskLaborPartsManager(this);
            ProjectTaskDependencyManager = new ProjectTaskDependencyManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.ProjectTaskLaborParts);
            TablesToDelete.Add(AppGlobals.LookupContext.ProjectTaskDependency);

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            var column = timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            column.HasSearchForHostId(DevLogixLookupContext.TimeSpentHostId);

            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
        }


        protected override void Initialize()
        {
            AppGlobals.MainViewModel.ProjectTaskViewModels.Add(this);
            if (base.View is IProjectTaskView projectTaskView)
            {
                View = projectTaskView;
            }
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Projects)
                {
                    var project =
                        AppGlobals.LookupContext.Projects.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    DefaultProjectAutoFillValue = 
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Projects,
                            project.Id.ToString());
                }
            }
            ProjectTotalsManager.Initialize();
            ActualRow = new ProjectTotalsRow(ProjectTotalsManager);
            ActualRow.RowTitle = "Actual";
            ProjectTotalsManager.InsertRow(ActualRow);

            StatusRow = new ProjectTotalsRow(ProjectTotalsManager);
            StatusRow.RowTitle = "Status";
            StatusRow.NegativeDisplayStyleId = ProjectTotalsManager.NegativeDisplayStyleId;
            StatusRow.PositiveDisplayStyleId = ProjectTotalsManager.PositiveDisplayStyleId;
            ProjectTotalsManager.InsertRow(StatusRow);

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

        protected override ProjectTask PopulatePrimaryKeyControls(ProjectTask newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetProjectTask(newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = result.GetAutoFillValue();

            var hasEditRight = TableDefinition.HasRight(RightTypes.AllowEdit);
            if (!hasEditRight)
            {
                if (result.UserId == AppGlobals.LoggedInUser.Id)
                {
                    SaveButtonEnabled = true;
                    View.SetTaskReadOnlyMode(false);
                }
                else
                {
                    SaveButtonEnabled = false;
                    View.SetTaskReadOnlyMode(true);
                }
            }

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.ProjectTaskId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            PunchInCommand.IsEnabled = true;

            return result;
        }

        private ProjectTask? GetProjectTask(int taskId)
        {
            ProjectTask newEntity;
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
            return result;
        }


        private void SetUserFilter()
        {
            UserAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            if (ProjectAutoFillValue.IsValid())
            {
                var project = ProjectAutoFillValue.GetEntity<Project>();
                if (project != null)
                {
                    var formula = string.Empty;
                    var selectQuery = new SelectQuery(AppGlobals.LookupContext.ProjectUsers.TableName);
                    selectQuery.AddSelectColumn(AppGlobals.LookupContext.ProjectUsers.GetFieldDefinition(p => p.UserId).FieldName);
                    selectQuery.AddWhereItem(AppGlobals.LookupContext.ProjectUsers.GetFieldDefinition(p => p.ProjectId).FieldName
                        , Conditions.Equals, project.Id.ToString(), false, ValueTypes.Numeric);
                    var selectStatement =
                        TableDefinition.Context.DataProcessor.SqlGenerator.GenerateSelectStatement(selectQuery);
                    formula = $"{AppGlobals.LookupContext.Users.GetFieldDefinition(p => p.Id).GetSqlFormatObject()} IN ({selectStatement})";
                    UserAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter("User", null, "", formula);

                }
            }
        }
        protected override void LoadFromEntity(ProjectTask entity)
        {
            _loading = true;
            ProjectAutoFillValue = entity.Project.GetAutoFillValue();
            UserAutoFillValue = entity.User.GetAutoFillValue();
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
                UserId = UserAutoFillValue.GetEntity<User>().Id,
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
            if (entity.UserId == 0)
            {
                var message =
                    "The Assigned To User is invalid.  Please select a valid User that is added to this project.";
                var caption = "Invalid Assigned To User";

                View.OnValidationFail(TableDefinition.GetFieldDefinition(p => p.UserId),
                    message, caption);
                return false;
            }

            if (entity.ProjectId == 0)
            {
                return base.ValidateEntity(entity);
            }
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectUser>();
            if (table != null)
            {
                var projectUser = table.FirstOrDefault(p => p.UserId == entity.UserId
                  && p.ProjectId == ProjectAutoFillValue.GetEntity<Project>().Id);
                if (projectUser == null)
                {
                    var message =
                        "The Assigned To User you have chosen has not been added to the project. Please select a user that has been added to the project.";
                    var caption = "Invalid User";
                    View.OnValidationFail(TableDefinition.GetFieldDefinition(p => p.UserId),
                        message, caption);
                    return false;
                }
            }

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
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
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
            var details = LaborPartsManager.GetEntityList();
            var dependencies = ProjectTaskDependencyManager.GetEntityList();

            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Project Task");

            if (result)
            {
                foreach (var projectTaskLaborPart in details)
                {
                    projectTaskLaborPart.ProjectTaskId = entity.Id;
                }

                foreach (var projectTaskDependency in dependencies)
                {
                    projectTaskDependency.ProjectTaskId = entity.Id;
                }

                var table = context.GetTable<ProjectTaskLaborPart>();
                context.RemoveRange(table.Where(p => p.ProjectTaskId == entity.Id));
                context.AddRange(details);

                var dependencyTable = context.GetTable<ProjectTaskDependency>();
                context.RemoveRange(dependencyTable.Where(p => p.ProjectTaskId == entity.Id));
                context.AddRange(dependencies);

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
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();

            var entity = context.GetTable<ProjectTask>()
                .FirstOrDefault(p => p.Id == Id);

            var table = context.GetTable<ProjectTaskLaborPart>();
            context.RemoveRange(table.Where(p => p.ProjectTaskId == entity.Id));

            var dependencyTable = context.GetTable<ProjectTaskDependency>();
            context.RemoveRange(dependencyTable.Where(p => p.ProjectTaskId == entity.Id));

            return context.DeleteEntity(entity, "Deleting Project Task");
        }

        public void SetTotalMinutesCost(decimal total)
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
            var lookupData = new LookupDataBase(recalcFilter, new LookupUserInterface()
            {
                PageSize = 10
            });
            var recordCount = lookupData.GetRecordCountWait();
            var currentProjectTask = 1;
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectTaskTable = context.GetTable<ProjectTask>();
            var timeClocksTable = context.GetTable<TimeClock>();
            lookupData.PrintDataChanged += (sender, args) =>
            {
                var table = args.OutputTable;
                foreach (DataRow tableRow in table.Rows)
                {
                    var projectTaskPrimaryKey = new PrimaryKeyValue(TableDefinition);
                    projectTaskPrimaryKey.PopulateFromDataRow(tableRow);
                    if (projectTaskPrimaryKey.IsValid)
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
            lookupData.GetPrintData();
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

        public bool ValidateProjectChange()
        {
            var projectId = ProjectAutoFillValue.GetEntity<Project>().Id;
            if (!_loading && projectId != ProjectTaskDependencyManager.ProjectFilter)
            {
                var result = ProjectTaskDependencyManager.ValidateProjectChange();
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
    }
}
