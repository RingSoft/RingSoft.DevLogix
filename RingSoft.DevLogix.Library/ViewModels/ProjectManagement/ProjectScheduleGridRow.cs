using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleGridRow : DataEntryGridRow
    {
        public new ProjectScheduleGridManager Manager { get; private set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal HoursWorked { get; set; }

        public decimal HoursRemaining { get; set; }

        public ProjectScheduleGridRow(ProjectScheduleGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectScheduleColumns)columnId;

            switch (column)
            {
                case ProjectScheduleColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateOnly,
                    }, Date);
                case ProjectScheduleColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ProjectScheduleColumns.HoursWorked:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Number,
                    }, HoursWorked);
                case ProjectScheduleColumns.HoursRemaining:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Number,
                    }, HoursRemaining);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle
            {
                State = DataEntryGridCellStates.ReadOnly
            };
            //return base.GetCellStyle(columnId);
        }
    }
}
