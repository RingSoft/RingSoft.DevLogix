using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectView
    {
        void PunchIn(Project project);
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

        public ProjectMaintenanceViewModel()
        {
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));

            PunchInCommand = new RelayCommand(() =>
            {
                View.PunchIn(GetProject(Id));
            });

            RecalcCommand = new RelayCommand(() =>
            {
                Recalc();
            });

            UsersGridManager = new ProjectUsersGridManager(this);

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);

            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
        }

        protected override void Initialize()
        {
            if (base.View is IProjectView projectView)
            {
                View = projectView;
            }

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
            RecalcCommand.IsEnabled = true;

            return project;
        }

        private static Project GetProject(int projectId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectTable = context.GetTable<Project>();
            var result = projectTable
                .Include(p => p.ProjectUsers)
                .FirstOrDefault(p => p.Id == projectId);
            return result;
        }


        protected override void LoadFromEntity(Project entity)
        {
            Deadline = entity.Deadline.ToLocalTime();
            OriginalDeadline = entity.OriginalDeadline.ToLocalTime();
            ProductAutoFillValue = ProductAutoFillSetup.GetAutoFillValueForIdValue(entity.ProductId);
            IsBillable = entity.IsBillable;
            UsersGridManager.LoadGrid(entity.ProjectUsers);
            Notes = entity.Notes;
        }

        protected override Project GetEntityData()
        {
            var result = new Project
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Deadline = Deadline.ToUniversalTime(),
                OriginalDeadline = OriginalDeadline.ToUniversalTime(),
                ProductId = ProductAutoFillValue.GetEntity(AppGlobals.LookupContext.Products).Id,
                IsBillable = IsBillable,
                Notes = Notes,
            };

            if (result.ProductId == 0)
            {
                result.ProductId = null;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            OriginalDeadline = Deadline = DateTime.Now;
            ProductAutoFillValue = null;
            IsBillable = false;
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Notes = null;
            UsersGridManager.SetupForNewRecord();
            RecalcCommand.IsEnabled = false;
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
                result = context.DeleteEntity(project, "Deleting Project");
            }

            return result;
        }

        private void Recalc()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var usersTable = context.GetTable<ProjectUser>();
            var timeClocksTable = context.GetTable<TimeClock>();
            var users = usersTable.Where(p => p.ProjectId == Id)
                .Include(p => p.User);

            var usersRows = UsersGridManager.Rows.OfType<ProjectUsersGridRow>();

            foreach (var user in users)
            {
                var projectUser = new ProjectUser();

                var totalMinutesSpent = timeClocksTable.Where(
                        p => p.ProjectId == Id
                             && p.MinutesSpent.HasValue
                             && p.UserId == user.UserId).ToList()
                    .Sum(p => p.MinutesSpent);

                var cost = user.User.HourlyRate * (totalMinutesSpent / 60);
                user.MinutesSpent = totalMinutesSpent.Value;
                user.Cost = cost.Value;
                var userRow = usersRows.FirstOrDefault(p => p.UserId == user.UserId);
                if (userRow != null) userRow.LoadFromEntity(user);
            }

            UsersGridManager.Grid?.RefreshGridView();
            RecordDirty = true;
        }
    }
}
