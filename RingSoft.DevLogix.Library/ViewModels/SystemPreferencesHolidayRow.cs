using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class SystemPreferencesHolidayRow : DbMaintenanceDataEntryGridRow<SystemPreferencesHolidays>
    {
        public new SystemPreferencesHolidaysManager Manager { get; private set; }

        public DateTime? Date { get; private set; }

        public string Holiday { get; private set; }

        public SystemPreferencesHolidayRow(SystemPreferencesHolidaysManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (HolidaysColumns)columnId;
            switch (column)
            {
                case HolidaysColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateOnly,
                    }, Date);
                case HolidaysColumns.Holiday:
                    return new DataEntryGridTextCellProps(this, columnId, Holiday);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (HolidaysColumns)value.ColumnId;
            switch (column)
            {
                case HolidaysColumns.Date:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                    {
                        Date = dateCellProps.Value;
                    }
                    break;
                case HolidaysColumns.Holiday:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Holiday = textCellProps.Text;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(SystemPreferencesHolidays entity)
        {
            Date = entity.Date;
            Holiday = entity.Name;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(SystemPreferencesHolidays entity, int rowIndex)
        {
            entity.SystemPreferencesId = Manager.ViewModel.Id;
            entity.Date = Date.Value;
            entity.Name = Holiday;
        }
    }
}
