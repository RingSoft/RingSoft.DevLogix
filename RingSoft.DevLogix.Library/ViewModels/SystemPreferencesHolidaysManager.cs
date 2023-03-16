using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum HolidaysColumns
    {
        Date = 1,
        Holiday = 2,
    }
    public class SystemPreferencesHolidaysManager : DbMaintenanceDataEntryGridManager<SystemPreferencesHolidays>
    {
        public const int DateColumnId = (int)HolidaysColumns.Date;
        public const int HolidayColumnId = (int)HolidaysColumns.Holiday;

        public new SystemPreferencesViewModel ViewModel { get; private set; }

        public SystemPreferencesHolidaysManager(SystemPreferencesViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new SystemPreferencesHolidayRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<SystemPreferencesHolidays> ConstructNewRowFromEntity(SystemPreferencesHolidays entity)
        {
            return new SystemPreferencesHolidayRow(this);
        }
    }
}
