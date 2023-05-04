using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public interface IUserTrackerView : IDbMaintenanceView
    {
        
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

        private decimal? _redAlertMinutes;

        public decimal? RedAlertMinutes
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

        private decimal? _yellowAlertMinutes;

        public decimal? YellowAlertMinutes
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

        public new IUserTrackerView View { get; private set; }

        public RelayCommand RefreshNowCommand { get; set; }

        public UserTrackerViewModel()
        {
            RefreshRateSetup = new TextComboBoxControlSetup();
            RefreshRateSetup.LoadFromEnum<RefreshRate>();
            RefreshNowCommand = new RelayCommand(Refresh);
            UserManager = new UserTrackerUserManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.UserTrackerUsers);
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
            RefreshNowCommand.IsEnabled = false;
            UserManager.SetupForNewRecord();
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
                if (users != null)
                {
                    foreach (var userTrackerUser in users)
                    {
                        userTrackerUser.UserTrackerId = entity.Id;
                    }
                    context.AddRange(users);
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
            UserManager.RefreshGrid();
        }
    }
}
