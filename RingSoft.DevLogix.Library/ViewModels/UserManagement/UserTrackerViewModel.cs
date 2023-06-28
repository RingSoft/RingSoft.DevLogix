using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using Timer = System.Timers.Timer;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IUserTrackerView : IDbMaintenanceView
    {
        void SetAlertLevel(AlertLevels level, string message);

        void RefreshGrid();
    }
    public class UserTrackerViewModel  : DevLogixDbMaintenanceViewModel<UserTracker>
    {
        public override TableDefinition<UserTracker> TableDefinition => AppGlobals.LookupContext.UserTracker;

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

        private int _refreshValue;

        public int RefreshValue
        {
            get => _refreshValue;
            set
            {
                if (_refreshValue == value)
                {
                    return;
                }
                _refreshValue = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _refreshRateSetup;

        public TextComboBoxControlSetup RefreshRateSetup
        {
            get => _refreshRateSetup;
            set
            {
                if (_refreshRateSetup == value)
                {
                    return;
                }

                _refreshRateSetup = value;
                OnPropertyChanged();
            }

        }

        private TextComboBoxItem _refreshRateItem;

        public TextComboBoxItem RefreshRateItem
        {
            get => _refreshRateItem;
            set
            {
                if (_refreshRateItem == value)
                {
                    return;
                }
                _refreshRateItem = value;
                OnPropertyChanged();
            }
        }

        public RefreshRate RefreshRate
        {
            get => (RefreshRate)RefreshRateItem.NumericValue;
            set => RefreshRateItem = RefreshRateSetup.GetItem((int)value);
        }

        private double? _redAlertMinutes;

        public double? RedAlertMinutes
        {
            get => _redAlertMinutes;
            set
            {
                if (_redAlertMinutes == value)
                    return;

                _redAlertMinutes = value;
                if (_redAlertMinutes == 0)
                {
                    _redAlertMinutes = null;
                }
                OnPropertyChanged();
            }
        }

        private double? _yellowAlertMinutes;

        public double? YellowAlertMinutes
        {
            get => _yellowAlertMinutes;
            set
            {
                if (_yellowAlertMinutes == value)
                    return;

                _yellowAlertMinutes = value;
                if (_yellowAlertMinutes == 0)
                {
                    _yellowAlertMinutes = null;
                }

                OnPropertyChanged();
            }
        }

        private UserTrackerUserManager _userManager;

        public UserTrackerUserManager UserManager
        {
            get => _userManager;
            set
            {
                if (_userManager == value)
                    return;

                _userManager = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _lastRefresh;

        public DateTime? LastRefresh
        {
            get => _lastRefresh;
            set
            {
                if (_lastRefresh == value)
                {
                    return;
                }
                _lastRefresh = value;
                OnPropertyChanged(null, false);
            }
        }

        private string _lastRefreshText;

        public string LastRefreshText
        {
            get => _lastRefreshText;
            set
            {
                if (_lastRefreshText == value)
                    return;

                _lastRefreshText = value;
                OnPropertyChanged(null, false);
            }
        }



        public new IUserTrackerView View { get; private set; }

        public RelayCommand RefreshNowCommand { get; set; }

        private System.Timers.Timer _timer = new Timer();

        public UserTrackerViewModel()
        {
            RefreshRateSetup = new TextComboBoxControlSetup();
            RefreshRateSetup.LoadFromEnum<RefreshRate>();
            RefreshNowCommand = new RelayCommand(() =>
            {
                LastRefresh = DateTime.Now;
                Refresh();
            });
            UserManager = new UserTrackerUserManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.UserTrackerUsers);
            _timer.Interval = 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        protected override void Initialize()
        {
            if (base.View is IUserTrackerView view)
            {
                View = view;
            }
            base.Initialize();
        }

        protected override UserTracker PopulatePrimaryKeyControls(UserTracker newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.GetTable<UserTracker>()
                .Include(p => p.Users)
                .ThenInclude(p => p.User)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = result.Id;
            RefreshNowCommand.IsEnabled = true;
            return result;
        }

        protected override void LoadFromEntity(UserTracker entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            RefreshRate = (DbLookup.AdvancedFind.RefreshRate)entity.RefreshType;
            RefreshValue = entity.RefreshInterval;
            RedAlertMinutes = entity.RedMinutes;
            YellowAlertMinutes = entity.YellowMinutes;
            UserManager.LoadGrid(entity.Users.OrderBy(p => p.User.Name));
            Refresh();
        }

        protected override UserTracker GetEntityData()
        {
            return new UserTracker
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                RefreshType = (byte)RefreshRate,
                RefreshInterval = RefreshValue,
                RedMinutes = RedAlertMinutes,
                YellowMinutes = YellowAlertMinutes,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            RefreshRate = RefreshRate.None;
            RefreshValue = 0;
            RedAlertMinutes = null;
            YellowAlertMinutes = null;
            UserManager.SetupForNewRecord();
            LastRefresh = null;
            UpdateRefreshText();
        }

        protected override bool SaveEntity(UserTracker entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, "Saving User Tracker"))
            {
                var existUsers = context.GetTable<UserTrackerUser>()
                    .Where(p => p.UserTrackerId == Id).ToList();
                if (existUsers.Any())
                {
                    context.RemoveRange(existUsers);
                }
                var users = UserManager.GetEntityList();
                var newUsers = new List<UserTrackerUser>();
                if (users != null)
                {
                    foreach (var userTrackerUser in users)
                    {
                        if (userTrackerUser.UserId == 0)
                        {
                            continue;
                        }
                        userTrackerUser.UserTrackerId = entity.Id;
                        newUsers.Add(userTrackerUser);
                    }
                    context.AddRange(newUsers);
                }
                return context.Commit("Saving User Tracker Users");
            }

            return false;
        }

        protected override bool DeleteEntity()
        {
            var result = false;
            var context = AppGlobals.DataRepository.GetDataContext();

            var existUsers = context.GetTable<UserTrackerUser>()
                .Where(p => p.UserTrackerId == Id).ToList();
            if (existUsers.Any())
            {
                context.RemoveRange(existUsers);
            }

            var table = context.GetTable<UserTracker>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting User Tracker");
            }
            return result;
        }

        private void Refresh()
        {
            if (LastRefresh == null)
            {
                LastRefresh = DateTime.Now;
            }
            UpdateRefreshText();
            UserManager.RefreshGrid();
        }
        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (LastRefresh == null)
            {
                LastRefresh = DateTime.Now;
                return;
            }

            if (RefreshValue == 0)
            {
                LastRefresh = DateTime.Now;
                return;
            }
            var nextRefresh = DateTime.Now;
            switch (RefreshRate)
            {
                case RefreshRate.None:
                    nextRefresh = DateTime.Now;
                    return;
                case RefreshRate.Hours:
                    nextRefresh = LastRefresh.Value.AddHours(RefreshValue);
                    break;
                case RefreshRate.Minutes:
                    nextRefresh = LastRefresh.Value.AddMinutes(RefreshValue);
                    break;
                case RefreshRate.Seconds:
                    nextRefresh = LastRefresh.Value.AddSeconds(RefreshValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (DateTime.Now > nextRefresh)
            {
                LastRefresh = DateTime.Now;
                Refresh();
            }
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            _timer.Stop();
            _timer.Enabled = false;
            _timer.Dispose();
            base.OnWindowClosing(e);
        }

        private void UpdateRefreshText()
        {
            if (_lastRefresh != null)
                LastRefreshText = GblMethods.FormatDateValue(_lastRefresh.Value, DbDateTypes.DateTime);
            else
            {
                LastRefreshText = string.Empty;
            }
        }

    }
}
