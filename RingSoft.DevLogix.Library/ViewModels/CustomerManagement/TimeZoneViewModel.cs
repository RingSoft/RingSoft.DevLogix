using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

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

        #endregion

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
