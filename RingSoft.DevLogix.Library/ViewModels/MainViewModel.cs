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
using RingSoft.DbLookup.ModelDefinition;
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

        bool LoginUser();

        void CloseWindow();

        void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition);

        void ShowDbMaintenanceDialog(TableDefinitionBase tableDefinition);

        void ShowAdvancedFindWindow();

        void MakeMenu();

        void PunchIn(Error error);

        void PunchIn(ProjectTask projectTask);

        void PunchIn(TestingOutline testingOutline);

        bool PunchOut(bool clockOut, int userId = 0);

        bool PunchOut(bool clockOut, User user, IDbContext context = null);

        void ShowMainChart(bool show = true);

        object GetOwnerWindow();

        void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack);

        void SetElapsedTime();

        void ShowTimeClock(bool show = true);
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

        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceWindowCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        public RelayCommand RefreshChartCommand { get; }
        public RelayCommand EditChartCommand { get; }
        public RelayCommand ChangeOrgCommand { get; }
        public RelayCommand TimeClockCommand { get; }
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

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);
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
                        if (!MainView.PunchOut(true, AppGlobals.LoggedInUser))
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
                                cont = MainView.PunchOut(true, user);
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
    }
}
