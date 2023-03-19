using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectDaysRow : DataEntryGridRow
    {
        public new ProjectDaysGridManager Manager { get; private set; }

        public string Day { get; private set; }

        public decimal WorkMinutes { get; private set; }

        public DayType DayType { get; private set; }

        public ProjectDaysRow(ProjectDaysGridManager manager, string day, DayType dayType) : base(manager)
        {
             Manager = manager;
             Day = day;
             DayType = dayType;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectDaysColumns)columnId;
            switch (column)
            {
                case ProjectDaysColumns.Day:
                    return new DataEntryGridTextCellProps(this, columnId, Day);
                case ProjectDaysColumns.Time:
                    return new TimeCostCellProps(this, columnId, WorkMinutes);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public void Clear()
        {
            WorkMinutes = 0;
        }

        public void SetWorkMinutes(decimal? workMinutes)
        {
            if (workMinutes != null)
            {
                WorkMinutes = workMinutes.Value;
            }
            Manager.SetStandardUserTime(this);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectDaysColumns)columnId;
            switch (column)
            {
                case ProjectDaysColumns.Day:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectDaysColumns)value.ColumnId;
            switch (column)
            {
                case ProjectDaysColumns.Time:
                    if (value is TimeCostCellProps timeCostCellProps)
                    {
                        SetWorkMinutes(timeCostCellProps.Minutes);
                        Manager.ViewModel.RecordDirty = true;
                    }
                    break;
            }
            base.SetCellValue(value);
        }
    }
}
