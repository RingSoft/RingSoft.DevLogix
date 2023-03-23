using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleGridRow : DataEntryGridRow
    {
        public new ProjectScheduleGridManager Manager { get; private set; }

        public DateTime Date { get; private set; }

        public string Description { get; private set; }

        public decimal HoursWorked { get; private set; }

        public decimal HoursRemaining { get; private set; }

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
                    break;
                case ProjectScheduleColumns.HoursRemaining:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
