using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectSpecialRights
    {
        AllowMaterialsEdit = 1,
    }

    public interface IProjectView
    {
        void PunchIn(Project project);

        bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition);

        string StartRecalcProcedure(LookupDefinitionBase lookupDefinition);

        void UpdateRecalcProcedure(int currentProject, int totalProjects, string currentProjectText);
    }

    public class ProjectMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Project>
    {
        public override TableDefinition<Project> TableDefinition => AppGlobals.LookupContext.Projects;

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

        private decimal _totalCost;

        public decimal TotalCost
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

        private LookupCommand _taskLookupCommand;

        public LookupCommand TaskLookupCommand
        {
            get => _taskLookupCommand;
            set
            {
                if (_taskLookupCommand == value)
                    return;

                _taskLookupCommand = value;
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
                {
                    return;
                }
                _notes = value;
                OnPropertyChanged();
            }
        }

        public new IProjectView View { get; private set; }

        public RelayCommand PunchInCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        public RelayCommand TasksAddModifyCommand { get; set; }

        public decimal MinutesSpent { get; set; }

        public ProjectTotalsRow ActualRow { get; private set; }

        public ProjectTotalsRow StatusRow { get; private set; }

        public ProjectMaintenanceViewModel()
        {
            ManagerAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ManagerId));
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));

            PunchInCommand = new RelayCommand(() =>
            {
                PunchIn();
            });

            RecalcCommand = new RelayCommand(() =>
            {
                Recalc();
            });

            TasksAddModifyCommand = new RelayCommand(OnTasksAddModify);

            ProjectTotalsManager = new ProjectTotalsManager();

            UsersGridManager = new ProjectUsersGridManager(this);

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            var column = timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            column.HasSearchForHostId(DevLogixLookupContext.TimeSpentHostId);

            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            TaskLookup = AppGlobals.LookupContext.ProjectTaskLookup.Clone();

            TablesToDelete.Add(AppGlobals.LookupContext.ProjectUsers);
        }

        protected override void Initialize()
        {
            if (base.View is IProjectView projectView)
            {
                View = projectView;
            }
            AppGlobals.MainViewModel.ProjectViewModels.Add(this);
            RecalcCommand.IsEnabled = TableDefinition.HasRight(RightTypes.AllowEdit);

            var test = TableDefinition.HasSpecialRight((int)ProjectSpecialRights.AllowMaterialsEdit);

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

        protected override Project PopulatePrimaryKeyControls(Project newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var project = GetProject(newEntity.Id);

            Id = project.Id;
            KeyAutoFillValue = KeyAutoFillSetup.GetAutoFillValueForIdValue(project.Id);

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.ProjectId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);
            PunchInCommand.IsEnabled = true;

            TaskLookup.FilterDefinition.ClearFixedFilters();
            TaskLookup.FilterDefinition.AddFixedFilter(p => p.ProjectId, Conditions.Equals, Id);
            TaskLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return project;
        }

        private static Project GetProject(int projectId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectTable = context.GetTable<Project>();
            var result = projectTable
                .Include(p => p.Manager)
                .Include(p => p.Product)
                .Include(p => p.ProjectUsers)
                .FirstOrDefault(p => p.Id == projectId);
            return result;
        }


        protected override void LoadFromEntity(Project entity)
        {
            ManagerAutoFillValue = entity.Manager.GetAutoFillValue();
            Deadline = entity.Deadline.ToLocalTime();
            OriginalDeadline = entity.OriginalDeadline.ToLocalTime();
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            IsBillable = entity.IsBillable;
            UsersGridManager.LoadGrid(entity.ProjectUsers);
            Notes = entity.Notes;
            AppGlobals.CalculateProject(entity, entity.ProjectUsers.ToList());
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.Cost;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            CalcTotals();
        }

        protected override Project GetEntityData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Project>();
            var result = new Project();
            if (Id != 0)
            {
                result = table.FirstOrDefault(p => p.Id == Id);
            }

            result.Id = Id;
            result.Name = KeyAutoFillValue.Text;
            result.ManagerId = ManagerAutoFillValue.GetEntity<User>().Id;
            result.Deadline = Deadline.ToUniversalTime();
            result.OriginalDeadline = OriginalDeadline.ToUniversalTime();
            result.ProductId = ProductAutoFillValue.GetEntity<Product>().Id;
            result.IsBillable = IsBillable;
            result.Notes = Notes;

            if (result.ProductId == 0)
            {
                result.ProductId = null;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ManagerAutoFillValue = null;
            KeyAutoFillValue = null;
            ManagerAutoFillValue = null;
            OriginalDeadline = Deadline = DateTime.Now;
            ProductAutoFillValue = null;
            IsBillable = false;
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Notes = null;
            UsersGridManager.SetupForNewRecord();
            PunchInCommand.IsEnabled = false;
            MinutesSpent = 0;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            TotalCost = 0;
            TaskLookupCommand = GetLookupCommand(LookupCommands.Clear);
            ProjectTotalsManager.ClearTotals();
        }

        protected override bool SaveEntity(Project entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();

            var result = false;

            result = context.SaveEntity(entity, "Saving Project");

            if (result)
            {
                var userRows = context.GetTable<ProjectUser>().Where(p => p.ProjectId == entity.Id).ToList();
                context.RemoveRange(userRows);
                userRows = UsersGridManager.GetEntityList();

                foreach (var userRow in userRows)
                {
                    userRow.ProjectId = entity.Id;
                }
                context.AddRange(userRows);

                result = context.Commit("Saving Project");
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<Project>();

            var result = false;
            var project = query.FirstOrDefault(p => p.Id == Id);
            if (project != null)
            {
                var users = context.GetTable<ProjectUser>().Where(p => p.ProjectId == project.Id);
                context.RemoveRange(users);
                result = context.DeleteEntity(project, "Deleting Project");
            }

            return result;
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
            var lookupData = new LookupDataBase(recalcFilter, new LookupUserInterface()
            {
                PageSize = 10
            });
            var recordCount = lookupData.GetRecordCountWait();
            var currentProject = 1;
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectTable = context.GetTable<Project>();
            var timeClocksTable = context.GetTable<TimeClock>();
            lookupData.PrintDataChanged += (sender, args) =>
            {
                var table = args.OutputTable;
                foreach (DataRow tableRow in table.Rows)
                {
                    var projectPrimaryKey = new PrimaryKeyValue(TableDefinition);
                    projectPrimaryKey.PopulateFromDataRow(tableRow);
                    if (projectPrimaryKey.IsValid)
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
                            var totalMinutesSpent = timeClocksTable
                                .Where(p => p.ProjectId == project.Id 
                                            && p.MinutesSpent.HasValue
                                            && p.UserId == user.UserId).ToList()
                                .Sum(p => p.MinutesSpent);

                            var cost = user.User.HourlyRate * (totalMinutesSpent / 60);
                            user.MinutesSpent = totalMinutesSpent.Value;
                            user.Cost = Math.Round(cost.Value, 2);

                            project.MinutesSpent += user.MinutesSpent;
                            project.Cost += user.Cost;
                        }

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
                            AppGlobals.CalculateProject(project, project.ProjectUsers.ToList());
                            MinutesSpent = project.MinutesSpent;
                            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
                            TotalCost = project.Cost;
                        }
                    }
                    currentProject++;
                }
            };
            lookupData.GetPrintData();
            if (result.IsNullOrEmpty())
            {
                if (context.Commit("Saving Projects"))
                {
                    UsersGridManager.Grid?.RefreshGridView();
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
            RecordDirty = oldRecordDirty;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.ProjectViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        private void PunchIn()
        {
            if (UsersGridManager.ProcessPunchIn())
            {
                View.PunchIn(GetProject(Id));
            }
        }

        private void OnTasksAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                TaskLookupCommand = GetLookupCommand(LookupCommands.AddModify);

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
            var table = AppGlobals.DataRepository.GetDataContext().GetTable<ProjectTask>();
            var tasks = table.Where(p => p.ProjectId == Id);
            var estimatedMinutes = (decimal)0;
            var estimatedCost = (decimal)0;
            var remainingMinutes = (decimal)0;
            var remainingCost = (decimal)0;
            foreach (var task in tasks)
            {
                estimatedMinutes += task.MinutesCost;
                estimatedCost += task.EstimatedCost;
                var taskRemainingMinutes = task.MinutesCost * (1 - task.PercentComplete);
                remainingMinutes += taskRemainingMinutes;
                remainingCost += task.EstimatedCost * (1 - task.PercentComplete);
            }
            ProjectTotalsManager.EstimatedRow.Minutes = estimatedMinutes;
            ProjectTotalsManager.EstimatedRow.Cost = estimatedCost;
            ProjectTotalsManager.RemainingRow.Minutes = remainingMinutes;
            ProjectTotalsManager.RemainingRow.Cost = remainingCost;
            ActualRow.Minutes = MinutesSpent;
            ActualRow.Cost = TotalCost;
            StatusRow.Minutes = estimatedMinutes - (ActualRow.Minutes + remainingMinutes);
            StatusRow.Cost = estimatedCost - (ActualRow.Cost + remainingCost);
            ProjectTotalsManager.RefreshGrid();
        }
    }
}
