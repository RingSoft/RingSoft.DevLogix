using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class TimeZoneViewModel : DevLogixDbMaintenanceViewModel<TimeZone>
    {
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


        protected override TimeZone PopulatePrimaryKeyControls(TimeZone newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TimeZone>();
            var timeZone = table.FirstOrDefault(p => p.Id == newEntity.Id);
            KeyAutoFillValue = timeZone.GetAutoFillValue();
            return timeZone;
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

        protected override bool SaveEntity(TimeZone entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Time Zone");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TimeZone>();
            var timeZone = table.FirstOrDefault(p => p.Id == Id);
            if (timeZone != null)
            {
                return context.DeleteEntity(timeZone, "Deleting Time Zone");
            }
            return true;
        }
    }
}
