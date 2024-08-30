using System;
using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class TimeZoneViewModel : DbMaintenanceViewModel<TimeZone>
    {
        #region Properties

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

        private int _hoursFromGMT;

        public int HoursFromGMT
        {
            get => _hoursFromGMT;
            set
            {
                if (_hoursFromGMT == value)
                {
                    return;
                }
                _hoursFromGMT = value;
                OnPropertyChanged();
            }
        }

        private int _currentHoursFromGmt;

        public int CurrentHoursFromGmt
        {
            get { return _currentHoursFromGmt; }
            set
            {
                if (_currentHoursFromGmt == value)
                {
                    return;
                }
                _currentHoursFromGmt = value;
                OnPropertyChanged();
            }
        }


        #endregion

        public TimeZoneViewModel()
        {
            var gmtDate = new DateTime
                (DateTime.UtcNow.Year
                    , DateTime.UtcNow.Month
                    , DateTime.UtcNow.Day
                    , DateTime.UtcNow.Hour
                    , DateTime.UtcNow.Minute
                    , DateTime.UtcNow.Second);
            var currDate = GblMethods.NowDate();
            var timeSpanDiff = currDate - gmtDate;
            var gmtDiff = timeSpanDiff.Hours;
            CurrentHoursFromGmt += gmtDiff;
        }

        protected override void PopulatePrimaryKeyControls(TimeZone newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(TimeZone entity)
        {
            HoursFromGMT = entity.HourToGMT;
        }

        protected override TimeZone GetEntityData()
        {
            var result = new TimeZone()
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                HourToGMT = HoursFromGMT,
            };
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            HoursFromGMT = 0;
        }
    }
}
