﻿using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
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


        public UserTrackerViewModel()
        {
            RefreshRateSetup = new TextComboBoxControlSetup();
            RefreshRateSetup.LoadFromEnum<RefreshRate>();
        }

        protected override UserTracker PopulatePrimaryKeyControls(UserTracker newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.GetTable<UserTracker>()
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = result.Id;
            return result;
        }

        protected override void LoadFromEntity(UserTracker entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            RefreshRate = (DbLookup.AdvancedFind.RefreshRate)entity.RefreshType;
            RefreshValue = entity.RefreshInterval;
            RedAlertMinutes = entity.RedMinutes;
            YellowAlertMinutes = entity.YellowMinutes;
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
        }

        protected override bool SaveEntity(UserTracker entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, "Saving User Tracker"))
            {
                return true;
            }

            return false;
        }

        protected override bool DeleteEntity()
        {
            var result = false;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<UserTracker>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting User Tracker");
            }
            return result;
        }
    }
}
