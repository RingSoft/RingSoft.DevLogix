using Microsoft.EntityFrameworkCore;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum UserGrids
    {
        Groups = 0,
        TimeOff = 1,
    }
    public interface IUserView : IDbMaintenanceView
    {

        UserMaintenanceViewModel LocalViewModel { get; set; }

        public string GetRights();

        public void LoadRights(string rightsString);

        public void ResetRights();

        public void RefreshView();

        bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition);

        string StartRecalcProcedure(LookupDefinitionBase lookupDefinition);

        void UpdateRecalcProcedure(int currentProject, int totalProjects, string currentProjectText);

        void SetUserReadOnlyMode(bool value);

        void SetExistRecordFocus(UserGrids userGrid, int rowId);

        string GetPassword();

        void SetPassword(string password);
    }

    public class BillabilityData
    {
        public User User { get; private set; }

        public double BillableProjects { get; private set; }

        public double NonBillableProjects { get; private set; }

        public double Errors { get; private set; }

        public double TestingOutlines { get; private set; }

        public double Customers { get; private set; }

        public double Support { get; private set; }
        public BillabilityData(User user)
        {
            User = user;
            Calculate();
        }

        public void Calculate()
        {
            var totalMinutes = User.BillableProjectsMinutesSpent
                               + User.NonBillableProjectsMinutesSpent
                               + User.ErrorsMinutesSpent
                               + User.TestingOutlinesMinutesSpent
                               + User.CustomerMinutesSpent
                               + User.SupportTicketsMinutesSpent;

            var billableProjectsBillability = (double)0;
            var nonBillableProjectsBillability = (double)0;
            var errorsBillability = (double)0;
            var testingBillability = (double)0;

            if (totalMinutes > 0)
            {
                BillableProjects = User.BillableProjectsMinutesSpent / totalMinutes;
                NonBillableProjects = User.NonBillableProjectsMinutesSpent / totalMinutes;
                Errors = User.ErrorsMinutesSpent / totalMinutes;
                TestingOutlines = User.TestingOutlinesMinutesSpent / totalMinutes;
                Customers = User.CustomerMinutesSpent / totalMinutes;
                Support = User.SupportTicketsMinutesSpent / totalMinutes;
            }

        }
    }
    public class UserMaintenanceViewModel : DevLogixDbMaintenanceViewModel<User>
    {
        public override TableDefinition<User> TableDefinition => AppGlobals.LookupContext.Users;

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
                OnPropertyChanged(null, false);
            }
        }

        private string _clockReason;

        public string ClockReason
        {
            get => _clockReason;
            set
            {
                if (_clockReason == value)
                    return;

                _clockReason = value;
                OnPropertyChanged(null, false);
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

        private double _hourlyRate;

        public double HourlyRate
        {
            get => _hourlyRate;
            set
            {
                if (_hourlyRate == value)
                    return;

                _hourlyRate = value;
                OnPropertyChanged();
            }
        }

        private UserBillabilityGridManager _billabilityGridManager;

        public UserBillabilityGridManager BillabilityGridManager
        {
            get => _billabilityGridManager;
            set
            {
                if (_billabilityGridManager == value)
                    return;

                _billabilityGridManager = value;
                OnPropertyChanged();
            }
        }

        private bool _rightsChanged;

        public bool RightsChanged
        {
            get => _rightsChanged;
            set
            {
                if (_rightsChanged == value)
                    return;

                _rightsChanged = value;
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
                OnPropertyChanged(null, false);
            }
        }

        private LookupDefinition<UserMonthlySalesLookup, UserMonthlySales> _userMonthlySalesLookup;

        public LookupDefinition<UserMonthlySalesLookup, UserMonthlySales> UserMonthlySalesLookup
        {
            get => _userMonthlySalesLookup;
            set
            {
                if (_userMonthlySalesLookup == value)
                    return;

                _userMonthlySalesLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _userMonthlySalesLookupCommand;

        public LookupCommand UserMonthlySalesLookupCommand
        {
            get => _userMonthlySalesLookupCommand;
            set
            {
                if (_userMonthlySalesLookupCommand == value)
                    return;

                _userMonthlySalesLookupCommand = value;
                OnPropertyChanged(null, false);
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

        private UserTimeOffGridManager _timeOffGridManager;

        public UserTimeOffGridManager TimeOffGridManager
        {
            get => _timeOffGridManager;
            set
            {
                if (_timeOffGridManager == value)
                    return;

                _timeOffGridManager = value;
                OnPropertyChanged();
            }
        }

        private double _monthlySalesQuota;

        public double MonthlySalesQuota
        {
            get => _monthlySalesQuota;
            set
            {
                if (_monthlySalesQuota == value)
                    return;

                _monthlySalesQuota = value;
                OnPropertyChanged();
            }
        }

        private double _totalSales;

        public double TotalSales
        {
            get => _totalSales;
            set
            {
                if (_totalSales == value)
                    return;

                _totalSales = value;
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

        public new IUserView View { get; set; }

        public RelayCommand ChangeChartCommand { get; private set; }

        public RelayCommand ClockOutCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public EnumFieldTranslation ClockReasonFieldTranslation;

        public int MasterUserId { get; private set; }

        public bool MasterMode { get; private set; }

        public AutoFillValue DefaultDepartmentAutoFillValue { get; private set; }

        private int _rowFocusId = -1;
        private string? _oldPassword;
        private const string _dummyPassword = "{1D56EF31}";

        public UserMaintenanceViewModel()
        {
            TablesToDelete.Add(AppGlobals.LookupContext.UseerMonthlySales);
            TablesToDelete.Add(AppGlobals.LookupContext.UsersGroups);
            TablesToDelete.Add(AppGlobals.LookupContext.UsersTimeOff);
            
            SetMasterUserId();
            ClockReasonFieldTranslation = new EnumFieldTranslation();
            ClockReasonFieldTranslation.LoadFromEnum<ClockOutReasons>();

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
                    if (!args.LookupData.SelectedPrimaryKeyValue.IsValid())
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
                    var sql = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateUpdateSql(updateStatement);
                    AppGlobals.LookupContext.DataProcessor.ExecuteSql(sql);

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
                if (AppGlobals.MainViewModel.PunchOut(true, Id))
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    var user = context.GetTable<User>().FirstOrDefault(p => p.Id == Id);
                    ClockDateTime = user.ClockDate.Value.ToLocalTime();
                    PopulateClockReason(user);
                    RecordDirty = recordDirty;
                }
            });

            RecalcCommand = new RelayCommand(() =>
            {
                Recalculate();
            });

            BillabilityGridManager = new UserBillabilityGridManager(this);

            TimeOffGridManager = new UserTimeOffGridManager(this);

            GroupsManager = new UsersGroupsManager(this);

            BillabilityGridManager.MakeGrid();

            //var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);

            TimeClockLookup = AppGlobals.LookupContext.TimeClockTabLookup.Clone();
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            var salesLookup = AppGlobals.LookupContext.UserMonthlySalesLookup.Clone();
            salesLookup.AddVisibleColumnDefinition(
                p => p.SalesQuota, p => p.Quota);

            salesLookup.AddVisibleColumnDefinition(
                p => p.Difference
                , p => p.Difference)
                .DoShowNegativeValuesInRed()
                .DoShowPositiveValuesInGreen();
            UserMonthlySalesLookup = salesLookup;

            PrintProcessingHeader += UserMaintenanceViewModel_PrintProcessingHeader;
        }

        private void SetMasterUserId()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var masterUser = context.GetTable<User>().FirstOrDefault();
            if (masterUser != null)
            {
                MasterUserId = masterUser.Id;
            }

        }

        protected override void Initialize()
        {
            View = base.View as IUserView;
            if (View == null)
                throw new Exception($"User View interface must be of type '{nameof(IUserView)}'.");
            View.LocalViewModel = this;

            AppGlobals.MainViewModel.UserViewModels.Add(this);

            DepartmentAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.DepartmentId));
            DefaultChartAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.DefaultChartId));
            SupervisorAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupervisorId));

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Departments)
                {
                    var department =
                        AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    DefaultDepartmentAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Departments,
                            department.Id.ToString());

                }
            }


            base.Initialize();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.UsersTimeOff)
            {
                var userTimeOffRow =
                    AppGlobals.LookupContext.UsersTimeOff.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);
                if (userTimeOffRow != null)
                {
                    _rowFocusId = userTimeOffRow.RowId;
                }
            }
            var result = base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
            return result;
        }

        protected override void PopulatePrimaryKeyControls(User newEntity, PrimaryKeyValue primaryKeyValue)
        {
            SetMasterMode(newEntity);
            Id = newEntity.Id;
            View.RefreshView();

            ChangeChartCommand.IsEnabled = true;

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.UserId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            UserMonthlySalesLookup.FilterDefinition.ClearFixedFilters();
            UserMonthlySalesLookup.FilterDefinition.AddFixedFilter(
                p => p.UserId
                , Conditions.Equals
                , newEntity.Id);
            UserMonthlySalesLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            if (!TableDefinition.HasRight(RightTypes.AllowEdit))
            {
                var readOnlyMode = AppGlobals.LoggedInUser.Id != Id;
                View.SetUserReadOnlyMode(readOnlyMode);
                SaveButtonEnabled = !readOnlyMode;
            }
        }

        protected override User GetEntityFromDb(User newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<User> query = AppGlobals.DataRepository.GetDataContext().GetTable<User>();

            var result = query
                .Include(p => p.UserTimeOff)
                .Include(p => p.UserGroups)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            if (AppGlobals.LoggedInUser == null)
            {
                ClockOutCommand.IsEnabled = false;
            }
            else
            {
                if (result.Id == AppGlobals.LoggedInUser.Id || result.IsSupervisor())
                {
                    ClockOutCommand.IsEnabled = true;
                }
                else
                {
                    ClockOutCommand.IsEnabled = false;
                }
            }

            PopulateClockReason(result);

            _oldPassword = result.Password;

            return result;
        }

        private void SetMasterMode(User newEntity)
        {
            if (MasterUserId == newEntity.Id || MasterUserId == 0)
            {
                MasterMode = true;
            }
            else
            {
                MasterMode = false;
            }
        }

        private void PopulateClockReason(User user)
        {
            var clockReason =
                ClockReasonFieldTranslation.TypeTranslations.FirstOrDefault(p => p.NumericValue == user.ClockOutReason);
            if (clockReason != null)
            {
                var userClockReason = (ClockOutReasons)user.ClockOutReason;
                switch (userClockReason)
                {
                    case ClockOutReasons.Other:
                        ClockReason = user.OtherClockOutReason;
                        break;
                    default:
                        ClockReason = clockReason.TextValue;
                        break;
                }
            }

            if (AppGlobals.LoggedInUser != null && user.Id == AppGlobals.LoggedInUser.Id)
            {
                AppGlobals.LoggedInUser.ClockOutReason = user.ClockOutReason;
                AppGlobals.LoggedInUser.ClockDate = user.ClockDate;
                AppGlobals.LoggedInUser.OtherClockOutReason = user.OtherClockOutReason;
            }
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
            TimeOffGridManager.LoadGrid(entity.UserTimeOff);
            if (_rowFocusId >= 0)
            {
                View.SetExistRecordFocus(UserGrids.TimeOff, _rowFocusId);
                _rowFocusId = -1;
            }
            SetBillability(entity);
            HourlyRate = entity.HourlyRate;
            if (entity.Password.IsNullOrEmpty())
            {
                View.SetPassword(string.Empty);
            }
            else
            {
                View.SetPassword(_dummyPassword);
            }
            MonthlySalesQuota = entity.MonthlySalesQuota;
            TotalSales = entity.TotalSales;
        }

        private void SetBillability(User entity)
        {
            var billabilityData = new BillabilityData(entity);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.BillableProjects,
                entity.BillableProjectsMinutesSpent, billabilityData.BillableProjects);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.NonBillableProjects,
                entity.NonBillableProjectsMinutesSpent, billabilityData.NonBillableProjects);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.Errors,
                entity.ErrorsMinutesSpent, billabilityData.Errors);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.TestingOutlines,
                entity.TestingOutlinesMinutesSpent, billabilityData.TestingOutlines);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.Customers,
                entity.CustomerMinutesSpent, billabilityData.Customers);

            BillabilityGridManager.SetRowValues(UserBillabilityRows.Support,
                entity.SupportTicketsMinutesSpent, billabilityData.Support);

        }

        protected override User GetEntityData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = new User
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Email = EmailAddress,
                PhoneNumber = PhoneNumber,
                Rights = View.GetRights().Encrypt(),
                Notes = Notes,
                SupervisorId = SupervisorAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                HourlyRate = HourlyRate,
                MonthlySalesQuota = MonthlySalesQuota
            };
            if (View.GetPassword().IsNullOrEmpty())
            {
                user.Password = string.Empty;
            }
            else
            {
                var password = View.GetPassword();
                if (password == _dummyPassword)
                {
                    user.Password = _oldPassword;
                }
                else
                {
                    user.Password = password.EncryptPassword();
                    var len = user.Password.Length;
                }
            }
            if (Id != 0)

                if (!TableDefinition.HasRight(RightTypes.AllowEdit))
                {
                    if (Entity != null)
                    {
                        user.Name= Entity.Name;
                    }
                }

            {
                var existUser = table.FirstOrDefault(p => p.Id == Id);
                if (existUser != null)
                {
                    user.BillableProjectsMinutesSpent = existUser.BillableProjectsMinutesSpent;
                    user.NonBillableProjectsMinutesSpent = existUser.NonBillableProjectsMinutesSpent;
                    user.ErrorsMinutesSpent = existUser.ErrorsMinutesSpent;
                    user.TestingOutlinesMinutesSpent = existUser.TestingOutlinesMinutesSpent;
                    user.ClockDate = existUser.ClockDate;
                    user.ClockOutReason = existUser.ClockOutReason;
                    user.OtherClockOutReason = existUser.OtherClockOutReason;
                    user.TotalSales = existUser.TotalSales;
                }
            }
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
            DepartmentAutoFillValue = DefaultDepartmentAutoFillValue;
            EmailAddress = null;
            PhoneNumber = null;
            DefaultChartAutoFillValue = null;
            SupervisorAutoFillValue = null;
            View.ResetRights();
            GroupsManager.SetupForNewRecord();
            ClockOutCommand.IsEnabled = false;
            ChangeChartCommand.IsEnabled = false;
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            UserMonthlySalesLookupCommand = GetLookupCommand(LookupCommands.Clear);
            Notes = null;
            HourlyRate = 0;
            ClockDateTime = null;
            ClockReason = string.Empty;
            TimeOffGridManager.SetupForNewRecord();
            SetMasterUserId();
            MasterMode = MasterUserId == 0;
            View.RefreshView();
            SetBillability(new User());
            View.SetPassword(string.Empty);
            _oldPassword = string.Empty;
            MonthlySalesQuota = 0;
            TotalSales = 0;
        }

        protected override bool ValidateEntity(User entity)
        {
            var password = View.GetPassword();
            if (!_oldPassword.IsNullOrEmpty() && password != _dummyPassword)
            {
                if (!password.IsValidPassword(_oldPassword))
                {
                    var message = "You must login with your old Password in order to continue";
                    var caption = "Login Needed.";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                    if (!AppGlobals.MainViewModel.MainView.LoginUser(Id))
                    {
                        return false;
                    }
                }
            }

            _oldPassword = entity.Password;
            if (!GroupsManager.ValidateGrid())
            {
                return false;
            }

            if (!TimeOffGridManager.ValidateGrid())
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

                var timeOffQuery = context.GetTable<UserTimeOff>();
                var userTimeOff = timeOffQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userTimeOff);
                userTimeOff = TimeOffGridManager.GetEntityList();
                if (userTimeOff != null)
                {
                    foreach (var timeOff in userTimeOff)
                    {
                        timeOff.UserId = entity.Id;
                    }
                    context.AddRange(userTimeOff);
                }

                var result = context.Commit("Saving User");

                if (result)
                {
                    if (AppGlobals.LoggedInUser != null && AppGlobals.LoggedInUser.Id == Id)
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

        protected override DbMaintenanceResults DoDelete()
        {
            var caption = "Delete Not Allowed";
            //if (AppGlobals.LoggedInUser != null && AppGlobals.LoggedInUser.Id == Id)
            //{
            //    var message = "You cannot delete the logged in user.";
            //    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            //    return DbMaintenanceResults.ValidationError;
            //}

            if (MasterMode)
            {
                var message = "You are not allowed to delete the master user.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return DbMaintenanceResults.ValidationError;
            }

            return base.DoDelete();
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

                var userTimeOffQuery = context.GetTable<UserTimeOff>();
                var userTimeOffs = userTimeOffQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userTimeOffs);

                var userSalesQuery = context.GetTable<UserMonthlySales>();
                var userMonthlySales = userSalesQuery.Where(p => p.UserId == Id).ToList();
                context.RemoveRange(userMonthlySales);

                if (context.DeleteNoCommitEntity(user, $"Deleting User '{user.Name}'"))
                {
                    return context.Commit($"Deleting User '{user.Name}'");
                }
            }
            return false;

        }

        public string StartRecalculateProcedure(
            LookupDefinitionBase lookupToFilter
            , AppProcedure procedure)
        {
            var result = string.Empty;
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<User>();
            var lookupUi = new LookupUserInterface { PageSize = 10 };
            var lookupData = SystemGlobals.DataRepository.GetDataContext()
                .GetLookupDataBase<User>(lookupToFilter, TableDefinition);
            var usersToProcess = lookupData.GetRecordCount();
            var userIndex = 1;
            DbDataProcessor.DontDisplayExceptions = true;
            lookupData.PrintOutput += (sender, args) =>
            {
                foreach (var primaryKeyValue in args.Result)
                {
                    var userPrimaryKey = primaryKeyValue;
                    var user = TableDefinition.GetEntityFromPrimaryKeyValue(userPrimaryKey);
                    if (user != null)
                    {
                        user = query
                            .Include(p => p.TimeClocks)
                            .ThenInclude(p => p.ProjectTask)
                            .ThenInclude(p => p.Project)
                            .Include(p => p.TimeClocks)
                            .Include(p => p.Orders)
                            .Include(p => p.UserMonthlySales)
                            .FirstOrDefault(p => p.Id == user.Id);
                    }

                    if (user != null)
                    {
                        View.UpdateRecalcProcedure(userIndex, usersToProcess, user.Name);
                        var billableProjects = user.TimeClocks.Where(p =>
                            p.ProjectTaskId.HasValue
                            && p.ProjectTask.Project.IsBillable
                            && p.MinutesSpent.HasValue);
                        user.BillableProjectsMinutesSpent = billableProjects.Sum(p => p.MinutesSpent.Value);

                        var nonBillableProjects = user.TimeClocks.Where(p =>
                            p.ProjectTaskId.HasValue
                            && !p.ProjectTask.Project.IsBillable
                            && p.MinutesSpent.HasValue);
                        user.NonBillableProjectsMinutesSpent = nonBillableProjects.Sum(p => p.MinutesSpent.Value);

                        var errors = user.TimeClocks.Where(p =>
                            p.ErrorId.HasValue
                            && p.MinutesSpent.HasValue);

                        user.ErrorsMinutesSpent = errors.Sum(p => p.MinutesSpent.Value);
                        if (!context.SaveNoCommitEntity(user, "Saving User"))
                        {
                            result = DbDataProcessor.LastException;
                            args.Abort = true;
                            return;
                        }
                        var testOutlines = user.TimeClocks.Where(p =>
                            p.TestingOutlineId.HasValue
                            && p.MinutesSpent.HasValue);

                        user.TestingOutlinesMinutesSpent = testOutlines.Sum(p => p.MinutesSpent.Value);
                        if (!context.SaveNoCommitEntity(user, "Saving User"))
                        {
                            result = DbDataProcessor.LastException;
                            args.Abort = true;
                            return;
                        }

                        var customers = user.TimeClocks.Where(p =>
                            p.CustomerId.HasValue
                            && p.MinutesSpent.HasValue);

                        user.CustomerMinutesSpent = customers.Sum(p => p.MinutesSpent.Value);
                        if (!context.SaveNoCommitEntity(user, "Saving User"))
                        {
                            result = DbDataProcessor.LastException;
                            args.Abort = true;
                            return;
                        }

                        var support = user.TimeClocks.Where(p =>
                            p.SupportTicketId.HasValue
                            && p.MinutesSpent.HasValue);

                        user.SupportTicketsMinutesSpent = support.Sum(p => p.MinutesSpent.Value);
                        if (!context.SaveNoCommitEntity(user, "Saving User"))
                        {
                            result = DbDataProcessor.LastException;
                            args.Abort = true;
                            return;
                        }

                        result = CalcUserMonthlySalesFromOrders(user, procedure);
                        if (!result.IsNullOrEmpty())
                        {
                            args.Abort = true;
                            return;
                        }

                        if (Id == user.Id)
                        {
                            SetBillability(user);
                        }
                    }
                    userIndex++;
                }
            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (!context.Commit("Recalculating"))
                {
                    result = DbDataProcessor.LastException;
                }
            }
            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        private string CalcUserMonthlySalesFromOrders(User user, AppProcedure procedure)
        {
            var result = string.Empty;
            var context = AppGlobals.DataRepository.GetDataContext();

            var userMonthlySales = user.UserMonthlySales;

            var listSales = new List<UserMonthlySales>();
            listSales.AddRange(userMonthlySales);
            foreach (var item in listSales)
            {
                item.TotalSales = 0;
            }
            context.RemoveRange(userMonthlySales);
            if (!context.Commit("Removing Old Sales", true))
            {
                result = GblMethods.LastError;
                return result;
            }

            var index = 1;
            var total = user.Orders.Count;
            var intFormatter = new IntegerEditControlSetup();
            var totalFormat = intFormatter.FormatValue(total);
            foreach (var order in user.Orders)
            {
                procedure.SplashWindow.SetProgress(
                    $"Recalculating Orders {intFormatter.FormatValue(index)}/{totalFormat}");
                var monthEndDate = new DateTime(order.OrderDate.Year, order.OrderDate.Month,
                    DateTime.DaysInMonth(order.OrderDate.Year, order.OrderDate.Month))
                    .ToUniversalTime();
                
                var monthEndSales = listSales.FirstOrDefault(
                    p => p.MonthEnding == monthEndDate );

                if (monthEndSales == null)
                {
                    monthEndSales = new UserMonthlySales()
                    {
                        UserId = user.Id,
                        MonthEnding = monthEndDate,
                        Quota = user.MonthlySalesQuota,
                    };
                    listSales.Add(monthEndSales);
                }
                monthEndSales.TotalSales += order.Total.GetValueOrDefault();
                monthEndSales.Difference = monthEndSales.TotalSales - monthEndSales.Quota;
                index++;
            }
            user.TotalSales = listSales.Sum( p => p.TotalSales );
            if (user.Id == Id)
            {
                TotalSales = user.TotalSales;
            }
            context.AddRange(listSales);
            if (!context.Commit("Committing User Sales", true))
            {
                result = GblMethods.LastError;
            }
            return result;
        }

        private void Recalculate()
        {
            var lookupToFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(lookupToFilter))
            {
                return;
            }

            var result = View.StartRecalcProcedure(lookupToFilter);
            if (result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox("Recalculation Complete.", "Recalculation Complete", RsMessageBoxIcons.Information);
                UserMonthlySalesLookupCommand = GetLookupCommand(LookupCommands.Refresh);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Recalculation Failed", RsMessageBoxIcons.Error);
            }
        }

        public void RefreshBillability(User user)
        {
            SetBillability(user);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.UserViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            printerSetupArgs.PrintingProperties.ReportType = ReportTypes.Custom;
            printerSetupArgs.PrintingProperties.CustomReportPathFileName =
                $"{RingSoftAppGlobals.AssemblyDirectory}\\UserManagement\\User.rpt";

            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }

        public override void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            base.ProcessPrintOutputData(printerSetupArgs);
            var customProperties = new List<PrintingCustomProperty>();
            customProperties.Add(new PrintingCustomProperty
            {
                Name = "intRecordCount",
                Value = printerSetupArgs.TotalRecords.ToString(),
            });
            PrintingInteropGlobals.PropertiesProcessor.CustomProperties = customProperties;
        }


        private void UserMaintenanceViewModel_PrintProcessingHeader(object? sender, PrinterDataProcessedEventArgs e)
        {
            var primaryKey = e.PrimaryKey;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var percentSetup = new DecimalEditControlSetup
            {
                FormatType = DecimalEditFormatTypes.Percent,
            };

            var dateSetup = new DateEditControlSetup
            {
                DateFormatType = DateFormatTypes.DateTime,
            };
            if (primaryKey.IsValid())
            {
                var user = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKey);
                if (user != null)
                {
                    user = table
                        .Include(p => p.UserTimeOff)
                        .FirstOrDefault(p => p.Id == user.Id);
                }

                if (user != null)
                {
                    var detailsChunk = new List<PrintingInputDetailsRow>();
                    var billabilityData = new BillabilityData(user);
                    e.HeaderRow.StringField15 = AppGlobals.MakeTimeSpent(user.BillableProjectsMinutesSpent);
                    e.HeaderRow.StringField16 = AppGlobals.MakeTimeSpent(user.NonBillableProjectsMinutesSpent);
                    e.HeaderRow.StringField17 = AppGlobals.MakeTimeSpent(user.ErrorsMinutesSpent);
                    e.HeaderRow.StringField18 = AppGlobals.MakeTimeSpent(user.TestingOutlinesMinutesSpent);
                    e.HeaderRow.NumberField15 = percentSetup.FormatValue(billabilityData.BillableProjects);
                    e.HeaderRow.NumberField16 = percentSetup.FormatValue(billabilityData.NonBillableProjects);
                    e.HeaderRow.NumberField17 = percentSetup.FormatValue(billabilityData.Errors);
                    e.HeaderRow.NumberField18 = percentSetup.FormatValue(billabilityData.TestingOutlines);

                    foreach (var userTimeOff in user.UserTimeOff)
                    {
                        var detailRow = new PrintingInputDetailsRow();
                        detailRow.HeaderRowKey = e.HeaderRow.RowKey;
                        detailRow.TablelId = 1;
                        detailRow.StringField01 = dateSetup.FormatValueForDisplay(userTimeOff.StartDate.ToLocalTime());
                        detailRow.StringField02 = dateSetup.FormatValueForDisplay(userTimeOff.EndDate.ToLocalTime());
                        detailRow.StringField03 = userTimeOff.Description;
                        detailsChunk.Add(detailRow);
                    }
                    PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);

                    detailsChunk.Clear();
                }
            }
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }
    }
}
