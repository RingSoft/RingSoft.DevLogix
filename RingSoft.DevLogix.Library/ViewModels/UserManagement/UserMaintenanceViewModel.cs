using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IUserView : IDbMaintenanceView
    {
        public string GetRights();

        public void LoadRights(string rightsString);

        public void ResetRights();

        public void RefreshView();

        public void OnValGridFail();
    }
    public class UserMaintenanceViewModel : DevLogixDbMaintenanceViewModel<User>
    {
        public override TableDefinition<User> TableDefinition => AppGlobals.LookupContext.Users;

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

        private AutoFillSetup _departmentAutoFillSetup;
        
        public AutoFillSetup DepartmentAutoFillSetup
        {
            get => _departmentAutoFillSetup;
            set
            {
                if (_departmentAutoFillSetup == value)
                {
                    return;
                }
                _departmentAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _departmentAutoFillValue;
        public AutoFillValue DepartmentAutoFillValue
        {
            get => _departmentAutoFillValue;
            set
            {
                if (_departmentAutoFillValue == value)
                {
                    return;
                }
                _departmentAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _supervisorAutoFillSetup;

        public AutoFillSetup SupervisorAutoFillSetup
        {
            get => _supervisorAutoFillSetup;
            set
            {
                if (_supervisorAutoFillSetup == value)
                {
                    return;
                }
                _supervisorAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _supervisorAutoFillValue;
        public AutoFillValue SupervisorAutoFillValue
        {
            get => _supervisorAutoFillValue;
            set
            {
                if (_supervisorAutoFillValue == value)
                {
                    return;
                }
                _supervisorAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _clockDateTime;

        public DateTime? ClockDateTime
        {
            get => _clockDateTime;
            set
            {
                if (_clockDateTime == value)
                    return;

                _clockDateTime = value;
                OnPropertyChanged();
            }
        }


        private AutoFillSetup _defaultChartAutoFillSetup;

        public AutoFillSetup DefaultChartAutoFillSetup
        {
            get => _defaultChartAutoFillSetup;
            set
            {
                if (_defaultChartAutoFillSetup == value)
                {
                    return;
                }
                _defaultChartAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _defaultChartAutoFillValue;
        public AutoFillValue DefaultChartAutoFillValue
        {
            get => _defaultChartAutoFillValue;
            set
            {
                if (_defaultChartAutoFillValue == value)
                {
                    return;
                }
                _defaultChartAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string? _emailAddress;

        public string? EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (_emailAddress == value)
                {
                    return;
                }
                _emailAddress = value;
                OnPropertyChanged();
                View.RefreshView();
            }
        }

        private string? _phoneNumber;

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber == value)
                {
                    return;
                }
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<TimeClockLookup, TimeClock> _timeClockLookup;

        public LookupDefinition<TimeClockLookup, TimeClock > TimeClockLookup
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


        private UsersGroupsManager _groupsManager;

        public UsersGroupsManager GroupsManager
        {
            get => _groupsManager;
            set
            {
                if (_groupsManager == value)
                {
                    return;
                }
                _groupsManager = value;
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

        public new IUserView View { get; private set; }

        public RelayCommand ChangeChartCommand { get; private set; }

        public RelayCommand ClockOutCommand { get; private set; }

        public UserMaintenanceViewModel()
        {
            ChangeChartCommand = new RelayCommand(() =>
            {
                PrimaryKeyValue primaryKey = null;
                var chart = DefaultChartAutoFillValue.GetEntity(AppGlobals.LookupContext.DevLogixCharts);
                if (chart.Id > 0)
                {
                    primaryKey = AppGlobals.LookupContext.DevLogixCharts.GetPrimaryKeyValueFromEntity(chart);
                }

                var lookup = AppGlobals.LookupContext.DevLogixChartLookup.Clone();
                lookup.WindowClosed += (sender, args) =>
                {
                    if (!args.LookupData.SelectedPrimaryKeyValue.IsValid)
                    {
                        return;
                    }
                    var newChart =
                        AppGlobals.LookupContext.DevLogixCharts.GetEntityFromPrimaryKeyValue(args.LookupData
                            .SelectedPrimaryKeyValue);

                    var recordDirty = RecordDirty;
                    DefaultChartAutoFillValue = DefaultChartAutoFillSetup.GetAutoFillValueForIdValue(newChart.Id);

                    var user = new User { Id = Id };
                    var userPrimaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(user);
                    var updateStatement = new UpdateDataStatement(userPrimaryKey);
                    var field = TableDefinition.GetFieldDefinition(p => p.DefaultChartId);
                    var sqlData = new SqlData(field.FieldName, newChart.Id.ToString(), ValueTypes.Numeric);
                    updateStatement.AddSqlData(sqlData);
                    var sqls = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateUpdateSql(updateStatement);
                    AppGlobals.LookupContext.DataProcessor.ExecuteSqls(sqls);

                    if (AppGlobals.LoggedInUser.Id == user.Id)
                    {
                        AppGlobals.MainViewModel.SetChartId(newChart.Id);
                    }

                    RecordDirty = recordDirty;
                };
                lookup.ShowAddOnTheFlyWindow(primaryKey);
            });

            ClockOutCommand = new RelayCommand(() =>
            {
                var recordDirty = RecordDirty;
                if (AppGlobals.MainViewModel.MainView.PunchOut(true, Id))
                {
                    ClockDateTime = null;
                    ClockOutCommand.IsEnabled = false;
                    RecordDirty = recordDirty;
                }
            });

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
        }

        protected override void Initialize()
        {
            View = base.View as IUserView;
            if (View == null)
                throw new Exception($"User View interface must be of type '{nameof(IUserView)}'.");

            DepartmentAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.DepartmentId));
            DefaultChartAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.DefaultChartId));
            SupervisorAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupervisorId));

            GroupsManager = new UsersGroupsManager( this);

            base.Initialize();
        }

        protected override User PopulatePrimaryKeyControls(User newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<User> query = AppGlobals.DataRepository.GetDataContext().GetTable<User>();
            query = query.Include(p => p.UserGroups);

            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            View.RefreshView();

            if (result.Id == AppGlobals.LoggedInUser.Id || result.IsSupervisor())
            {
                if (result.ClockDate == null)
                {
                    ClockOutCommand.IsEnabled = false;
                }
                else
                {
                    ClockOutCommand.IsEnabled = true;
                }
            }
            else
            {
                ClockOutCommand.IsEnabled = false;
            }

            ChangeChartCommand.IsEnabled = true;

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.UserId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);
            return result;
        }

        protected override void LoadFromEntity(User entity)
        {
            DepartmentAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(
                AppGlobals.LookupContext.Departments, entity.DepartmentId.ToString());
            View.LoadRights(entity.Rights.Decrypt());
            EmailAddress = entity.Email;
            PhoneNumber = entity.PhoneNumber;
            GroupsManager.LoadGrid(entity.UserGroups);
            Notes = entity.Notes;
            DefaultChartAutoFillValue = DefaultChartAutoFillSetup.GetAutoFillValueForIdValue(entity.DefaultChartId);
            SupervisorAutoFillValue = SupervisorAutoFillSetup.GetAutoFillValueForIdValue(entity.SupervisorId);
            if (entity.ClockDate.HasValue)
            {
                ClockDateTime = entity.ClockDate.Value.ToLocalTime();
            }
            else
            {
                ClockDateTime = null;
            }
        }

        protected override User GetEntityData()
        {
            var user = new User
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Email = EmailAddress,
                PhoneNumber = PhoneNumber,
                Rights = View.GetRights().Encrypt(),
                Notes = Notes,
                SupervisorId = SupervisorAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
            };
            if (DepartmentAutoFillValue.IsValid())
            {
                user.DepartmentId = AppGlobals.LookupContext.Departments
                    .GetEntityFromPrimaryKeyValue(DepartmentAutoFillValue.PrimaryKeyValue).Id;
            }
            user.DefaultChartId = DefaultChartAutoFillValue.GetEntity(AppGlobals.LookupContext.DevLogixCharts).Id;
            if (user.DefaultChartId == 0)
            {
                user.DefaultChartId = null;
            }

            if (user.SupervisorId == 0)
            {
                user.SupervisorId = null;
            }

            return user;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            DepartmentAutoFillValue = null;
            EmailAddress = null;
            PhoneNumber = null;
            DefaultChartAutoFillValue = null;
            SupervisorAutoFillValue = null;
            View.ResetRights();
            GroupsManager.SetupForNewRecord();
            ClockOutCommand.IsEnabled = false;
            ChangeChartCommand.IsEnabled = false;
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Notes = null;
        }

        protected override bool ValidateEntity(User entity)
        {
            if (!GroupsManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override bool SaveEntity(User entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, $"Saving User '{entity.Name}.'"))
            {
                var ugQuery = context.GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userGroups);
                userGroups = GroupsManager.GetList();
                if (userGroups != null)
                {
                    foreach (var userGroup in userGroups)
                    {
                        userGroup.UserId = entity.Id;
                    }

                    context.AddRange(userGroups);
                }

                var result = context.Commit("Saving UsersGroups");

                if (result)
                {
                    if (AppGlobals.LoggedInUser.Id == Id)
                    {
                        if (entity.DefaultChartId.HasValue)
                        {
                            AppGlobals.MainViewModel.SetChartId(entity.DefaultChartId.Value);
                        }
                        else
                        {
                            AppGlobals.MainViewModel.SetChartId(0);
                        }
                    }
                }
                return result;
            }

            return false;


        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<User>();
            var user = query.FirstOrDefault(f => f.Id == Id);
            if (user != null)
            {
                var ugQuery = context.GetTable<UsersGroup>();
                var userGroups = ugQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userGroups);
                if (context.DeleteNoCommitEntity(user, $"Deleting User '{user.Name}'"))
                {
                    return context.Commit($"Deleting User '{user.Name}'");
                }
            }
            return false;

        }
    }
}
