using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectView
    {
        bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition);

        string StartRecalcProcedure(LookupDefinitionBase lookupDefinition);

        void UpdateRecalcProcedure(int currentProject, int totalProjects, string currentProjectText);

        void SetExistRecordFocus(int userId);

        void GotoGrid();

        DateTime? GetDeadline();

        void GotoTasksTab();
    }

    public class ProjectMaintenanceViewModel : DbMaintenanceViewModel<Project>
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

        private AutoFillSetup _managerAutoFillSetup;

        public AutoFillSetup ManagerAutoFillSetup
        {
            get => _managerAutoFillSetup;
            set
            {
                if (_managerAutoFillSetup == value)
                    return;

                _managerAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _managerAutoFillValue;

        public AutoFillValue ManagerAutoFillValue
        {
            get => _managerAutoFillValue;
            set
            {
                if (_managerAutoFillValue == value)
                    return;

                _managerAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private double _contractCost;

        public double ContractCost
        {
            get => _contractCost;
            set
            {
                if (_contractCost == value)
                    return;

                _contractCost = value;
                OnPropertyChanged();
            }
        }


        private string _timeSpent;

        public string TimeSpent
        {
            get => _timeSpent;
            set
            {
                if (_timeSpent == value)
                    return;

                _timeSpent = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _totalCost;

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost == value)
                    return;

                _totalCost = value;
                OnPropertyChanged(null, false);
            }
        }

        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;
                
                _startDate = value;
                OnPropertyChanged();
            }
        }



        private DateTime _deadline;

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline == value)
                    return;

                _deadline = value;
                OnPropertyChanged();
            }
        }

        private DateTime _originalDeadline;

        public DateTime OriginalDeadline
        {
            get => _originalDeadline;
            set
            {
                if (_originalDeadline == value)
                    return;

                _originalDeadline = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _productAutoFillSetup;

        public AutoFillSetup ProductAutoFillSetup
        {
            get => _productAutoFillSetup;
            set
            {
                if (_productAutoFillSetup == value)
                    return;

                _productAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _productAutoFillValue;

        public AutoFillValue ProductAutoFillValue
        {
            get => _productAutoFillValue;
            set
            {
                if (_productAutoFillValue == value)
                    return;

                _productAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private bool _isBillable;

        public bool IsBillable
        {
            get => _isBillable;
            set
            {
                if (_isBillable == value)
                    return;

                _isBillable = value;
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
        private ProjectDaysGridManager _projectDaysGridManager;

        public ProjectDaysGridManager ProjectDaysGridManager
        {
            get => _projectDaysGridManager;
            set
            {
                if (_projectDaysGridManager == value)
                    return;

                _projectDaysGridManager = value;
                OnPropertyChanged();
            }
        }


        private ProjectUsersGridManager _usersGridManager;

        public ProjectUsersGridManager UsersGridManager
        {
            get => _usersGridManager;
            set
            {
                if (_usersGridManager == value)
                    return;

                _usersGridManager = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<ProjectTaskLookup, ProjectTask> _taskLookup;

        public LookupDefinition<ProjectTaskLookup, ProjectTask> TaskLookup
        {
            get => _taskLookup;
            set
            {
                if (_taskLookup == value)
                    return;

                _taskLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<ProjectMaterialLookup, ProjectMaterial> _materialLookup;

        public LookupDefinition<ProjectMaterialLookup, ProjectMaterial> MaterialLookup
        {
            get => _materialLookup;
            set
            {
                if (_materialLookup == value)
                    return;

                _materialLookup = value;
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
                {
                    return;
                }
                _notes = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> _historyLookup;

        public LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> HistoryLookup
        {
            get => _historyLookup;
            set
            {
                if (_historyLookup == value)
                    return;

                _historyLookup = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public new IProjectView View { get; private set; }

        public RelayCommand RecalcCommand { get; set; }

        public RelayCommand TasksAddModifyCommand { get; set; }

        public RelayCommand CalculateDeadlineCommand { get; set; }

        public RelayCommand MaterialsAddModifyCommand { get; set; }

        public double MinutesSpent { get; set; }

        public ProjectTotalsRow ActualRow { get; private set; }

        public ProjectTotalsRow StatusRow { get; private set; }

        private int _userFocusId = -1;

        public ProjectMaintenanceViewModel()
        {
            ManagerAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ManagerId));
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));

            RecalcCommand = new RelayCommand(() =>
            {
                Recalc();
            });

            TasksAddModifyCommand = new RelayCommand(OnTasksAddModify);

            MaterialsAddModifyCommand = new RelayCommand(OnMaterialsAddModify);

            CalculateDeadlineCommand = new RelayCommand((() =>
            {
                if (!UsersGridManager.ValidateCalc())
                {
                    return;
                }

                var context = AppGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<ProjectTask>();
                if (!table.Any(p => p.ProjectId == Id))
                {
                    var message = "No tasks have been defined for this project";
                    var caption = "Calculation Validation";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    View.GotoTasksTab();
                    TasksAddModifyCommand.Execute(null);
                    return;
                }
                var deadline = View.GetDeadline();
                if (deadline.HasValue)
                {
                    Deadline = deadline.Value;
                }
            }));

            ProjectDaysGridManager = new ProjectDaysGridManager(this);
            ProjectDaysGridManager.Initialize();

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

            UsersGridManager = new ProjectUsersGridManager(this);
            RegisterGrid(UsersGridManager);

            //var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            //timeClockLookup.Include(p => p.User)
            //    .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            //timeClockLookup.Include(p => p.ProjectTask)
            //    .AddVisibleColumnDefinition(p => p.ProjectTask, p => p.Name);
            //var column = timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            //column.HasSearchForHostId(DevLogixLookupContext.TimeSpentHostId);

            TimeClockLookup = AppGlobals.LookupContext.TimeClockTabLookup.Clone();
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
            //TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
            TimeClockLookup.Include(p => p.ProjectTask)
                .AddVisibleColumnDefinition(p => p.ProjectTask, p => p.Name);

            var historyLookup =
                new LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory>(AppGlobals.LookupContext
                    .ProjectMaterialHistory);
            historyLookup.AddVisibleColumnDefinition(p => p.Date, p => p.Date);
            historyLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            historyLookup.Include(p => p.ProjectMaterial)
                .AddVisibleColumnDefinition(p => p.ProjectMaterial, p => p.Name);
            historyLookup.AddVisibleColumnDefinition(p => p.Quantity, p => p.Quantity);
            historyLookup.AddVisibleColumnDefinition(p => p.Cost, p => p.Cost);
            historyLookup.InitialOrderByType = OrderByTypes.Descending;
            HistoryLookup = historyLookup;
            TaskLookup = AppGlobals.LookupContext.ProjectTaskLookup.Clone();
            MaterialLookup = AppGlobals.LookupContext.ProjectMaterialLookup.Clone();

            RegisterLookup(TimeClockLookup);
            RegisterLookup(HistoryLookup);
            RegisterLookup(TaskLookup);
            RegisterLookup(MaterialLookup);
        }

        protected override void Initialize()
        {
            if (base.View is IProjectView projectView)
            {
                View = projectView;
            }

            AppGlobals.MainViewModel.ProjectViewModels.Add(this);
            RecalcCommand.IsEnabled = TableDefinition.HasRight(RightTypes.AllowEdit);

            base.Initialize();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.ProjectUsers)
            {
                var userRow =
                    AppGlobals.LookupContext.ProjectUsers.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);
                if (userRow != null)
                {
                    _userFocusId = userRow.UserId;
                }
            }
            var result = base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
            return result;
        }


        protected override void PopulatePrimaryKeyControls(Project newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            CalculateDeadlineCommand.IsEnabled = true;
        }

        protected override void LoadFromEntity(Project entity)
        {
            ManagerAutoFillValue = entity.Manager.GetAutoFillValue();
            StartDate = entity.StartDateTime;
            if (entity.ContractCost != null) ContractCost = entity.ContractCost.Value;
            Deadline = entity.Deadline;
            OriginalDeadline = entity.OriginalDeadline;
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            IsBillable = entity.IsBillable;
            ProjectDaysGridManager.LoadFromEntity(entity);
            Notes = entity.Notes;
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.Cost;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            CalcTotals();

            if (_userFocusId >= 0)
            {
                View.SetExistRecordFocus(_userFocusId);
                _userFocusId = -1;
            }

        }

        protected override Project GetEntityData()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Project>();
            var result = new Project();
            if (Id != 0)
            {
                result = table.FirstOrDefault(p => p.Id == Id);
            }

            result.Id = Id;
            result.Name = KeyAutoFillValue.Text;
            result.StartDateTime = StartDate;
            result.ManagerId = ManagerAutoFillValue.GetEntity<User>().Id;
            result.ContractCost = ContractCost;
            result.Deadline = Deadline;
            result.OriginalDeadline = OriginalDeadline;
            result.ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
            result.IsBillable = IsBillable;
            result.Notes = Notes;

            if (result.ProductId == 0)
            {
                result.ProductId = null;
            }
            ProjectDaysGridManager.SaveToEntity(result);
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ManagerAutoFillValue = null;
            ContractCost = 0;
            KeyAutoFillValue = null;
            StartDate = null;
            ManagerAutoFillValue = null;
            OriginalDeadline = Deadline = GblMethods.NowDate();
            ProductAutoFillValue = null;
            IsBillable = false;
            Notes = null;
            UsersGridManager.SetupForNewRecord();
            MinutesSpent = 0;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            TotalCost = 0;
            ProjectTotalsManager.ClearTotals();
            ProjectDaysGridManager.Clear();
            CalculateDeadlineCommand.IsEnabled = false;
        }

        private void Recalc()
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
            var currentProject = 1;
            var context = SystemGlobals.DataRepository.GetDataContext();
            var projectTable = context.GetTable<Project>();
            var timeClocksTable = context.GetTable<TimeClock>();
            lookupData.PrintOutput += (sender, args) =>
            {
                foreach (var primaryKeyValue in args.Result)
                {
                    var projectPrimaryKey = primaryKeyValue;
                    if (projectPrimaryKey.IsValid())
                    {
                        var project = TableDefinition.GetEntityFromPrimaryKeyValue(projectPrimaryKey);
                        project = projectTable
                            .Include(p => p.ProjectUsers)
                            .ThenInclude(p => p.User)
                            .FirstOrDefault(p => p.Id == project.Id);

                        View.UpdateRecalcProcedure(currentProject, recordCount, project.Name);
                        project.MinutesSpent = 0;
                        project.Cost = 0;
                        foreach (var user in project.ProjectUsers)
                        {
                            double? totalMinutesSpent = null;
                            totalMinutesSpent = timeClocksTable
                                .Include(p => p.ProjectTask)
                                .ThenInclude(p => p.Project)
                                .Where(p => p.ProjectTask.ProjectId == project.Id
                                            && p.MinutesSpent.HasValue
                                            && p.UserId == user.UserId).ToList()
                                .Sum(p => p.MinutesSpent);

                            var cost = user.User.HourlyRate * (totalMinutesSpent / 60);
                            user.MinutesSpent = totalMinutesSpent.Value;
                            user.Cost = Math.Round(cost.Value, 2);

                            project.MinutesSpent += user.MinutesSpent;
                            project.Cost += user.Cost;
                        }

                        var materialsTable = context.GetTable<ProjectMaterial>();
                        var materials = materialsTable.Where(p => p.ProjectId == project.Id).ToList();
                        project.Cost += materials.Sum(p => p.ActualCost);

                        if (!context.SaveNoCommitEntity(project, "Saving Project"))
                        {
                            args.Abort = true;
                            result = DbDataProcessor.LastException;
                            return;
                        }

                        if (project.Id == Id)
                        {
                            var usersRows = UsersGridManager.Rows.OfType<ProjectUsersGridRow>()
                                .Where(p => p.IsNew == false);
                            foreach (var userRow in usersRows)
                            {
                                var projectUser = project.ProjectUsers.FirstOrDefault(p => p.UserId
                                    == userRow.UserId);
                                if (projectUser != null)
                                    userRow.LoadFromEntity(projectUser);
                            }
                            var originalMinutesSpent = MinutesSpent;
                            var originalCost = TotalCost;
                            MinutesSpent = project.MinutesSpent;
                            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
                            TotalCost = project.Cost;
                        }
                    }
                    currentProject++;
                }
            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (context.Commit("Saving Projects"))
                {
                    UsersGridManager.Grid?.RefreshGridView();
                    CalcTotals();
                }
                else
                {
                    result = DbDataProcessor.LastException;
                }
            }

            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        public void RefreshCostGrid(Project project)
        {
            var oldRecordDirty = RecordDirty;
            MinutesSpent = project.MinutesSpent;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            TotalCost = project.Cost;
            CalcTotals();

            UsersGridManager.LoadGrid(project.ProjectUsers);
            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(Entity);
            TimeClockLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh, primaryKey));
            HistoryLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh, primaryKey));
            RecordDirty = oldRecordDirty;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.ProjectViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        private void OnTasksAddModify()
        {
            if (!UsersGridManager.ValidateCalc())
            {
                return;
            }

            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                TaskLookup.SetCommand(GetLookupCommand(LookupCommands.AddModify));
        }

        private void OnMaterialsAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                MaterialLookup.SetCommand(GetLookupCommand(LookupCommands.AddModify));

        }

        protected override void OnRecordDirtyChanged(bool newValue)
        {
            if (newValue)
            {
                
            }
            base.OnRecordDirtyChanged(newValue);
        }

        public void CalcTotals()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectTask>();
            var tasks = table.Where(p => p.ProjectId == Id);
            var estimatedMinutes = (double)0;
            var estimatedCost = (double)0;
            var remainingMinutes = (double)0;
            var remainingCost = (double)0;
            foreach (var task in tasks)
            {
                estimatedMinutes += task.MinutesCost;
                estimatedCost += task.EstimatedCost;
                var taskRemainingMinutes = task.MinutesCost * (1 - task.PercentComplete);
                remainingMinutes += taskRemainingMinutes;
                remainingCost += task.EstimatedCost * (1 - task.PercentComplete);
            }

            var materialsTable = context.GetTable<ProjectMaterial>();
            var materials = materialsTable.Where(p => p.ProjectId == Id)
                .ToList();
            var materialsCost = materials.Sum(p => p.Cost);
            var actualMaterialCost = materials.Sum(p => p.ActualCost);
            var remainingMaterialsCost = materialsCost - actualMaterialCost;
            if (remainingMaterialsCost < 0)
            {
                remainingMaterialsCost = 0;
            }

            ProjectTotalsManager.EstimatedRow.Minutes = estimatedMinutes;
            ProjectTotalsManager.EstimatedRow.Cost = estimatedCost + materialsCost;
            ProjectTotalsManager.RemainingRow.Minutes = remainingMinutes;
            ProjectTotalsManager.RemainingRow.Cost = remainingCost + remainingMaterialsCost;
            ActualRow.Minutes = MinutesSpent;
            ActualRow.Cost = TotalCost;
            StatusRow.Minutes = estimatedMinutes - (ActualRow.Minutes + remainingMinutes);
            StatusRow.Cost = estimatedCost - (ActualRow.Cost + remainingCost);
            ProjectTotalsManager.RefreshGrid();
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh));
        }
    }
}
