using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMainView
    {
        bool ChangeOrganization();

        bool LoginUser(int userId = 0);

        void CloseWindow();

        void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition);

        void ShowMaintenanceTab(TableDefinitionBase tableDefinition);

        void ShowAdvancedFindWindow();

        void MakeMenu();

        void PunchIn(Error error);

        void PunchIn(ProjectTask projectTask);

        void PunchIn(TestingOutline testingOutline);

        void PunchIn(Customer customer);

        void PunchIn(SupportTicket ticket);

        void ShowChart(DevLogixChart chart);

        object GetOwnerWindow();

        void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack);

        void SetElapsedTime();

        void ShowTimeClock(bool show = true);

        void LaunchTimeClock(TimeClock activeTimeCard);

        UserClockReasonViewModel GetUserClockReason();

        bool UpgradeVersion();

        void ShowAbout();

        void RepairDates();

        bool CloseAllTabs();

        void Beep();
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
        public List<CustomerViewModel> CustomerViewModels { get; } = new List<CustomerViewModel>();
        public List<SupportTicketViewModel> SupportTicketViewModels { get; } = new List<SupportTicketViewModel>();
        public TimeClockMaintenanceViewModel TimeClockMaintenanceViewModel { get; set; }
        public double? SupportMinutesPurchased { get; set; }

        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand RepairDatesCommand { get; set; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceWindowCommand { get; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceTabCommand { get; }
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
        public RelayCommand CustomersCommand { get; }
        public RelayCommand OrdersCommand { get; }
        public RelayCommand SupportTicketsCommand { get; }
        public RelayCommand UpgradeCommand { get; }
        public RelayCommand AboutCommand { get; }

        public string? SupportTimeLeft { get; private set; }

        public double? SupportMinutesLeft { get; private set; }

        public double SecondsElapsed { get; private set; }

        public string ActiveCustomerName { get; set; }

        public TimeClockModes? TimeClockMode { get; private set; }

        private Timer _timer = new Timer();
        private DateTime? _startDate;

        public MainViewModel()
        {
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, args) =>
            {
                if (_startDate == null)
                {
                    SecondsElapsed = 0;
                }
                else
                {
                    SecondsElapsed++;
                    if (TimeClockMode.HasValue)
                    {
                        if (TimeClockMode == TimeClockModes.SupportTicket)
                        {
                            SupportTimeLeft = AppGlobals.GetSupportTimeLeftTextFromDate
                            (_startDate.Value.ToLocalTime()
                                , SupportMinutesPurchased
                                , out var supportMinutesLeft);
                            SupportMinutesLeft = supportMinutesLeft;
                        }
                        else
                        {
                            SupportMinutesLeft = null;
                            SupportTimeLeft = null;
                            SupportMinutesPurchased = null;
                        }
                    }
                    else
                    {
                        SupportMinutesLeft = null;
                        SupportTimeLeft = null;
                        SupportMinutesPurchased = null;
                    }

                    SetElapsedTime(GetElapsedTime());
                }
            };
            _timer.Enabled = true;
            ExitCommand = new RelayCommand(Exit);
            LogoutCommand = new RelayCommand(Logout);
            RepairDatesCommand = new RelayCommand((() =>
            {
                MainView.RepairDates();
            }));

            UsersCommand =
                new RelayCommand((() => { MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Users); }));

            UserTrackerCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.UserTracker);
            }));

            ProductsCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Products);
            }));

            ErrorsCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Errors);
            }));

            OutlinesCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.TestingOutlines);
            }));

            ProjectsCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Projects);
            }));

            CustomersCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Customer);
            }));

            OrdersCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.Order);
            }));

            SupportTicketsCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.SupportTicket);
            }));

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);
            ChartCommand = new RelayCommand((() =>
            {
                MainView.ShowMaintenanceTab(AppGlobals.LookupContext.DevLogixCharts);
            }));

            UpgradeCommand = new RelayCommand(() =>
            {
                if (!MainView.UpgradeVersion())
                {
                    var message = "You are already on the latest version.";
                    var caption = "Upgrade Not Necessary";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                }
            });

            AboutCommand = new RelayCommand((() =>
            {
                MainView.ShowAbout();
            }));


            ShowMaintenanceWindowCommand = new RelayCommand<TableDefinitionBase>(ShowMaintenanceWindow);

            ShowMaintenanceTabCommand = new RelayCommand<TableDefinitionBase>(ShowMaintenanceTab);

            ChangeOrgCommand = new RelayCommand(( async () =>
            {
                if (!MainView.CloseAllTabs())
                {
                    return;
                }

                if (AppGlobals.LoggedInUser.ClockOutReason == (byte)ClockOutReasons.ClockedIn)
                {
                    var message = "Do you wish to clock out?";
                    var caption = "Clock Out";
                    if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        MessageBoxButtonsResult.Yes)
                    {
                        if (!PunchOut(true, AppGlobals.LoggedInUser))
                        {
                            return;
                        }
                    }
                }

                ShowChartId(0);
                AppGlobals.LoggedInOrganization = null;
                SetupTimer(null, null);

                Initialize(MainView);
            }));
            TimeClockCommand = new RelayCommand((() =>
            {
                //Peter Ringering - 01/11/2025 03:22:34 PM - E-99
                if (ActiveTimeClockId > 0)
                {
                    var timeClock = new TimeClock()
                    {
                        Id = ActiveTimeClockId,
                    };
                    var primaryKey = AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(timeClock);
                    if (primaryKey.IsValidDb())
                    {
                        AppGlobals.LookupContext.TimeClockLookup.ShowAddOnTheFlyWindow(primaryKey);
                    }
                    else
                    {
                        MainView.Beep();
                        MainView.ShowTimeClock(false);

                    }
                }
                else
                {
                    MainView.Beep();
                }
            }));
        }

        private string GetElapsedTime()
        {
            var result = string.Empty;

            var duration = GblMethods.NowDate().Subtract(_startDate.Value.ToLocalTime());
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
                    SystemGlobals.TableRegistry.ShowDialog(AppGlobals.LookupContext.Users);
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
                .Include(p => p.SupportTicket)
                .ThenInclude(p => p.Customer)
                .FirstOrDefault(p => p.UserId == AppGlobals.LoggedInUser.Id
                                     && p.PunchOutDate == null);
            TimeClockModes? timeClockMode = null;
            if (timeClock != null)
            {
                //double? supportMinutesPurchased = null;
                if (timeClock.SupportTicket != null)
                {
                    SupportMinutesPurchased = timeClock.SupportTicket.Customer.SupportMinutesPurchased;
                    timeClockMode = TimeClockModes.SupportTicket;
                }
                SetupTimer(timeClock, timeClockMode);
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

        private void ShowMaintenanceTab(TableDefinitionBase tableDefinition)
        {
            MainView.ShowMaintenanceTab(tableDefinition);
        }

        private void ShowAdvancedFind()
        {
            MainView.ShowAdvancedFindWindow();
        }

        private async void Logout()
        {
            if (!MainView.CloseAllTabs())
            {
                return;
            }

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
                            var clockOut = await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
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
                SetupTimer(null, null);

                if (MainView.LoginUser())
                {
                    UserAutoFillValue = DbLookup.ExtensionMethods.GetAutoFillValue(AppGlobals.LoggedInUser);
                    SetUserTimer();
                    MainView.MakeMenu();
                }
            }
        }

        public void ShowChartId(int chartId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<DevLogixChart>();
            var chart = table.FirstOrDefault(p => p.Id == chartId);
            if (chart != null) MainView.ShowChart(chart);
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
            var timeClock = table
                .Include(p => p.SupportTicket)
                .ThenInclude(p => p.Customer)
                .FirstOrDefault(p => p.Id == timeClockId);

            double? supportMinutesPurchased = null;
            TimeClockModes? timeClockMode = null;
            if (timeClock != null)
            {
                if (timeClock.SupportTicket != null)
                {
                    supportMinutesPurchased = timeClock.SupportTicket.Customer.SupportMinutesPurchased;
                    timeClockMode = TimeClockModes.SupportTicket;
                }

                SetupTimer(timeClock, timeClockMode);
            }
        }

        public void SetupTimer(TimeClock timeClock, TimeClockModes? timeClockMode)
        {
            TimeClockMode = timeClockMode;
            int userId = 0;
            if (timeClock == null)
            {
                SupportMinutesLeft = null;
                SupportMinutesPurchased = null;
                SupportTimeLeft = null;
            }
            else
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

        private async Task<bool> ValidateNewTimeClock()
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
                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) ==
                    MessageBoxButtonsResult.Yes)
                {
                    MainView.LaunchTimeClock(activeTimeCard);
                }

                return false;
            }

            return true;
        }

        public async void PunchIn(Error error)
        {
            if (await ValidateNewTimeClock())
            {
                MainView.PunchIn(error);
            }
        }

        public async void PunchIn(TestingOutline outline)
        {
            if (await ValidateNewTimeClock())
            {
                MainView.PunchIn(outline);
            }
        }

        public async void PunchIn(ProjectTask task)
        {
            if (await ValidateNewTimeClock())
            {
                MainView.PunchIn(task);
            }
        }

        public async void PunchIn(Customer customer)
        {
            if (await ValidateNewTimeClock())
            {
                MainView.PunchIn(customer);
            }
        }

        public async void PunchIn(SupportTicket ticket)
        {
            if (await ValidateNewTimeClock())
            {
                MainView.PunchIn(ticket);
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
                user.ClockDate = GblMethods.NowDate().ToUniversalTime();
                user.ClockOutReason = (byte)clockReasonViewModel.ClockOutReason;
                if (clockReasonViewModel.ClockOutReason == ClockOutReasons.Other)
                {
                    user.OtherClockOutReason = clockReasonViewModel.OtherReason;
                }
                else
                {
                    user.OtherClockOutReason = null;
                }
                result = context.SaveEntity(user, "Saving Clock Reason");
                if (result)
                {
                    var timeClockContext = AppGlobals.DataRepository.GetDataContext();
                    var timeClockQuery = timeClockContext.GetTable<TimeClock>();
                    activeTimeCard = timeClockQuery
                        .OrderBy(p => p.Id)
                        .Where(p => p.PunchOutDate != null)
                        .LastOrDefault(p => p.UserId == user.Id);

                    if (activeTimeCard != null)
                    {
                        var enumTrans = new EnumFieldTranslation();
                        enumTrans.LoadFromEnum<ClockOutReasons>();
                        var reasonItem = enumTrans.TypeTranslations
                            .FirstOrDefault(p => p.NumericValue == (int)user.ClockOutReason);
                        if (reasonItem != null) activeTimeCard.ClockOutReason = reasonItem.TextValue;
                        if (user.OtherClockOutReason != null)
                        {
                            activeTimeCard.ClockOutReason = user.OtherClockOutReason;
                        }

                        result = timeClockContext.SaveEntity(activeTimeCard, "Saving Time Card");
                    }
                }

                return result;
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

            return user != null && PunchOut(clockOut, user, context);
        }

        public async Task<bool> ValidateWindowClose()
        {
            if (!MainView.CloseAllTabs())
            {
                return false;
            }
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
                        var clockOut = await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
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
