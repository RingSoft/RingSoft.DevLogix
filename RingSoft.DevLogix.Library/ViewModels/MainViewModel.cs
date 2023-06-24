using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Security.Certificates;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMainView
    {
        bool ChangeOrganization();

        bool LoginUser(int userId = 0);

        void CloseWindow();

        void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition);

        void ShowDbMaintenanceDialog(TableDefinitionBase tableDefinition);

        void ShowAdvancedFindWindow();

        void MakeMenu();

        void PunchIn(Error error);

        void PunchIn(ProjectTask projectTask);

        void PunchIn(TestingOutline testingOutline);

        void ShowMainChart(bool show = true);

        object GetOwnerWindow();

        void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack);

        void SetElapsedTime();

        void ShowTimeClock(bool show = true);

        void LaunchTimeClock(TimeClock activeTimeCard);

        UserClockReasonViewModel GetUserClockReason();
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _organization;

        public string Organization
        {
            get => _organization;
            set
            {
                if (_organization == value)
                {
                    return;
                }

                _organization = value;
                OnPropertyChanged();
            }
        }

        private string _dbPlatform;

        public string DbPlatform
        {
            get => _dbPlatform;
            set
            {
                if (_dbPlatform == value)
                    return;

                _dbPlatform = value;
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
                {
                    return;
                }

                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _elapsedTime;

        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime == value)
                {
                    return;
                }

                _elapsedTime = value;
            }
        }

        public IMainView MainView { get; set; }
        public int ActiveTimeClockId { get; set; }

        public List<UserMaintenanceViewModel> UserViewModels { get; } = new List<UserMaintenanceViewModel>();
        public List<ProjectMaintenanceViewModel> ProjectViewModels { get; } = new List<ProjectMaintenanceViewModel>();
        public List<ProjectTaskViewModel> ProjectTaskViewModels { get; } = new List<ProjectTaskViewModel>();
        public List<ProjectMaterialViewModel> MaterialViewModels { get; } = new List<ProjectMaterialViewModel>();
        public List<ErrorViewModel> ErrorViewModels { get; } = new List<ErrorViewModel>();
        public List<TestingOutlineViewModel> TestingOutlineViewModels { get; } = new List<TestingOutlineViewModel>();
        public TimeClockMaintenanceViewModel TimeClockMaintenanceViewModel { get; set; }

        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceWindowCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        public RelayCommand ChartCommand { get; }
        public RelayCommand RefreshChartCommand { get; }
        public RelayCommand EditChartCommand { get; }
        public RelayCommand ChangeOrgCommand { get; }
        public RelayCommand TimeClockCommand { get; }
        public RelayCommand UsersCommand { get; }
        public RelayCommand UserTrackerCommand { get; }
        public RelayCommand ProductsCommand { get; }
        public RelayCommand ErrorsCommand { get; }
        public RelayCommand OutlinesCommand { get; }
        public RelayCommand ProjectsCommand { get; }
        public ChartBarsViewModel ChartViewModel { get; private set; }

        private Timer _timer = new Timer();
        private DateTime? _startDate;

        public MainViewModel()
        {
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, args) =>
            {
                if (_startDate != null)
                {
                    SetElapsedTime(GetElapsedTime());
                }
            };
            _timer.Enabled = true;
            ExitCommand = new RelayCommand(Exit);
            LogoutCommand = new RelayCommand(Logout);

            UsersCommand =
                new RelayCommand((() => { MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Users); }));

            UserTrackerCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.UserTracker);
            }));

            ProductsCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Products);
            }));

            ErrorsCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Errors);
            }));

            OutlinesCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.TestingOutlines);
            }));

            ProjectsCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.Projects);
            }));

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);
            ChartCommand = new RelayCommand((() =>
            {
                MainView.ShowDbMaintenanceWindow(AppGlobals.LookupContext.DevLogixCharts);
            }));
            ShowMaintenanceWindowCommand = new RelayCommand<TableDefinitionBase>(ShowMaintenanceWindow);
            RefreshChartCommand = new RelayCommand(RefreshChart);
            EditChartCommand = new RelayCommand(EditChart);
            ChangeOrgCommand = new RelayCommand((() =>
            {
                if (AppGlobals.LoggedInUser.ClockOutReason == (byte)ClockOutReasons.ClockedIn)
                {
                    var message = "Do you wish to clock out?";
                    var caption = "Clock Out";
                    if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        MessageBoxButtonsResult.Yes)
                    {
                        if (!PunchOut(true, AppGlobals.LoggedInUser))
                        {
                            return;
                        }
                    }
                }

                SetChartId(0);
                AppGlobals.LoggedInOrganization = null;
                SetupTimer(null);

                Initialize(MainView);
            }));
            TimeClockCommand = new RelayCommand((() =>
            {
                var timeClock = new TimeClock()
                {
                    Id = ActiveTimeClockId,
                };
                var primaryKey = AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(timeClock);
                if (primaryKey.IsValid)
                {
                    AppGlobals.LookupContext.TimeClockLookup.ShowAddOnTheFlyWindow(primaryKey);
                }
            }));
        }

        private string GetElapsedTime()
        {
            var result = string.Empty;

            var duration = DateTime.Now.Subtract(_startDate.Value.ToLocalTime());
            result = $"{duration.Days.ToString("00")} {duration.ToString("hh\\:mm\\:ss")}";

            return result;
        }


        private void SetElapsedTime(string elapsedTime)
        {
            ElapsedTime = elapsedTime;
            MainView.SetElapsedTime();
        }

        public void Initialize(IMainView view)
        {
            MainView = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInOrganization == null)
                loadVm = view.ChangeOrganization();
            else
            {
                SetOrganizationProps();
            }

            if (loadVm)
            {
                if (UserAutoFillSetup == null)
                {
                    UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
                }

                var query = AppGlobals.DataRepository.GetDataContext().GetTable<User>();
                if (!query.Any())
                {
                    var message =
                        "You must first create a master user.  Make sure this user has full User table maintenance rights and don't forget the password.";
                    var caption = "Create User";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                    MainView.ShowDbMaintenanceDialog(AppGlobals.LookupContext.Users);
                    if (!query.Any())
                    {
                        AppGlobals.LoggedInOrganization = null;
                        Initialize(view);
                    }
                }

                if (query.Any())
                {
                    loadVm = view.LoginUser();
                    if (loadVm)
                    {
                        UserAutoFillValue = DbLookup.ExtensionMethods.GetAutoFillValue(AppGlobals.LoggedInUser);
                        SetUserTimer();
                    }
                    else
                    {
                        AppGlobals.LoggedInOrganization = null;
                        Initialize(view);
                    }
                }
                //else
                //{
                //    MainView.MakeMenu();
                //}
            }
        }

        private void SetUserTimer()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TimeClock>();
            var timeClock = table
                .FirstOrDefault(p => p.UserId == AppGlobals.LoggedInUser.Id
                                     && p.PunchOutDate == null);
            if (timeClock != null)
            {
                SetupTimer(timeClock);
            }
        }

        private void Exit()
        {
            MainView.CloseWindow();
        }

        private void ShowMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            MainView.ShowDbMaintenanceWindow(tableDefinition);
        }

        private void ShowAdvancedFind()
        {
            MainView.ShowAdvancedFindWindow();
        }

        private void Logout()
        {
            var cont = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var user = context.GetTable<User>().FirstOrDefault(p => p.Id == AppGlobals.LoggedInUser.Id);
                if (user != null)
                {
                    var clockReason = (ClockOutReasons)user.ClockOutReason;
                    switch (clockReason)
                    {
                        case ClockOutReasons.ClockedIn:
                            var message = "Do you want to clock out before you log off?";
                            var caption = "Clock Out?";
                            var clockOut = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                            if (clockOut == MessageBoxButtonsResult.Yes)
                            {
                                cont = PunchOut(true, user);
                            }

                            break;
                    }
                }
            }

            if (cont)
            {
                SetupTimer(null);

                if (MainView.LoginUser())
                {
                    UserAutoFillValue = DbLookup.ExtensionMethods.GetAutoFillValue(AppGlobals.LoggedInUser);
                    SetUserTimer();
                    MainView.MakeMenu();
                }
            }
        }

        public void InitChart(ChartBarsViewModel chartBarsViewModel)
        {
            ChartViewModel = chartBarsViewModel;
        }

        public void SetChartId(int chartId)
        {
            MainView.ShowMainChart(chartId != 0);
            if (chartId == 0)
            {
                ChartViewModel.Clear(true);
            }
            else
            {
                ChartViewModel.SetChartBars(chartId);
            }
        }

        private void RefreshChart()
        {
            ChartViewModel.SetChartBars(ChartViewModel.ChartId);
            var message = "Chart Refreshed.";
            ControlsGlobals.UserInterface.ShowMessageBox(message, message, RsMessageBoxIcons.Information);
        }

        private void EditChart()
        {
            var devLogixChart = new DevLogixChart
            {
                Id = ChartViewModel.ChartId
            };

            var primaryKey = AppGlobals.LookupContext.DevLogixCharts.GetPrimaryKeyValueFromEntity(devLogixChart);
            var lookup = AppGlobals.LookupContext.DevLogixChartLookup.Clone();
            lookup.ShowAddOnTheFlyWindow(primaryKey, null, MainView.GetOwnerWindow());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void SetupTimer(int timeClockId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TimeClock>();
            var timeClock = table.FirstOrDefault(p => p.Id == timeClockId);
            SetupTimer(timeClock);
        }

        public void SetupTimer(TimeClock timeClock)
        {
            int userId = 0;
            if (timeClock != null)
            {
                userId = timeClock.UserId;
                ActiveTimeClockId = timeClock.Id;
            }

            if (userId == 0)
            {
                _startDate = null;
                MainView.ShowTimeClock(false);
                ActiveTimeClockId = 0;
            }
            else
            {
                _startDate = timeClock.PunchInDate;
                MainView.ShowTimeClock();
            }
        }

        public void SetOrganizationProps()
        {
            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<DbPlatforms>();
            Organization = AppGlobals.LoggedInOrganization.Name;
            var platform = AppGlobals.LoggedInOrganization.Platform;
            var description = enumTranslation.TypeTranslations
                .FirstOrDefault(p => p.NumericValue == platform).TextValue;
            DbPlatform = description;
            UserAutoFillValue = null;
        }

        private bool ValidateNewTimeClock()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var tableQuery = context.GetTable<TimeClock>();
            var activeTimeCard =
                tableQuery.FirstOrDefault(p => p.PunchOutDate == null && p.UserId == AppGlobals.LoggedInUser.Id);
            if (activeTimeCard != null)
            {
                var message =
                    "You currently are punched into an active time card. Do you wish to load that time card instead?";
                var caption = "Already Punched in";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) ==
                    MessageBoxButtonsResult.Yes)
                {
                    MainView.LaunchTimeClock(activeTimeCard);
                }

                return false;
            }

            return true;
        }

        public void PunchIn(Error error)
        {
            if (ValidateNewTimeClock())
            {
                MainView.PunchIn(error);
            }
        }

        public void PunchIn(TestingOutline outline)
        {
            if (ValidateNewTimeClock())
            {
                MainView.PunchIn(outline);
            }
        }

        public void PunchIn(ProjectTask task)
        {
            if (ValidateNewTimeClock())
            {
                MainView.PunchIn(task);
            }
        }

        public bool PunchOut(bool clockOut, User user, IDbContext context = null)
        {
            if (context == null)
            {
                context = AppGlobals.DataRepository.GetDataContext();
            }

            var tableQuery = context.GetTable<TimeClock>();

            var activeTimeCard =
                tableQuery.FirstOrDefault(p => p.PunchOutDate == null && p.UserId == user.Id && p.PunchOutDate == null);

            var result = true;

            var lookupDefinition = AppGlobals.LookupContext.TimeClockLookup.Clone();

            var dialogInput = new DialogInput();

            if (activeTimeCard != null)
            {
                lookupDefinition.ShowAddOnTheFlyWindow(
                    AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(activeTimeCard), dialogInput);
            }
            else
            {
                dialogInput.DialogResult = true;
            }

            if (!clockOut)
            {
                return true;
            }

            if (dialogInput.DialogResult == false)
            {
                return false;
            }

            var clockReasonViewModel = MainView.GetUserClockReason();
            if (!clockReasonViewModel.DialogResult)
            {
                return false;
            }

            if (user != null)
            {
                user.ClockDate = DateTime.Now.ToUniversalTime();
                user.ClockOutReason = (byte)clockReasonViewModel.ClockOutReason;
                if (clockReasonViewModel.ClockOutReason == ClockOutReasons.Other)
                {
                    user.OtherClockOutReason = clockReasonViewModel.OtherReason;
                }
                else
                {
                    user.OtherClockOutReason = null;
                }

                var table = AppGlobals.LookupContext.Users;
                var clockDateField = table.GetFieldDefinition(p => p.ClockDate);
                var clockReasonField = table.GetFieldDefinition(p => p.ClockOutReason);
                var otherReasonField = table.GetFieldDefinition(p => p.OtherClockOutReason);

                var sqlData = new SqlData(clockDateField.FieldName
                    , clockReasonField.FormatValue(user.ClockDate.ToString())
                    , ValueTypes.DateTime
                    , DbDateTypes.DateTime);
                var updateStatement = new UpdateDataStatement(table.GetPrimaryKeyValueFromEntity(user));
                updateStatement.AddSqlData(sqlData);

                sqlData = new SqlData(clockReasonField.FieldName
                    , user.ClockOutReason.ToString()
                    , ValueTypes.Numeric);
                updateStatement.AddSqlData(sqlData);

                if (user.OtherClockOutReason.IsNullOrEmpty())
                {
                    sqlData = new SqlData(otherReasonField.FieldName
                        , ""
                        , ValueTypes.String);
                }
                else
                {
                    sqlData = new SqlData(otherReasonField.FieldName
                        , user.OtherClockOutReason
                        , ValueTypes.String);
                }

                updateStatement.AddSqlData(sqlData);

                var sql = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateUpdateSql(updateStatement);
                var dataResult = AppGlobals.LookupContext.DataProcessor.ExecuteSql(sql);
                return dataResult.ResultCode == GetDataResultCodes.Success;

                //return context.SaveEntity(user, "Clocking Out");

            }

            return false;
        }

        public bool PunchOut(bool clockOut, int userId)
        {
            if (userId == 0)
            {
                userId = AppGlobals.LoggedInUser.Id;
            }

            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = table.FirstOrDefault(p => p.Id == userId);

            return PunchOut(clockOut, user, context);
        }

        public bool ValidateWindowClose()
        {
            if (AppGlobals.LoggedInUser == null)
            {
                return true;
            }
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var userQuery = context.GetTable<User>();
                if (userQuery != null)
                {
                    var user = userQuery.FirstOrDefault(p => p.Id == AppGlobals.LoggedInUser.Id);
                    if (user != null && (ClockOutReasons)user.ClockOutReason == ClockOutReasons.ClockedIn)
                    {
                        var message = "Do you want to clock out before you exit the application?";
                        var caption = "Clock Out?";
                        var clockOut = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                        if (clockOut == MessageBoxButtonsResult.Yes)
                        {
                            if (!PunchOut(true, user.Id))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
