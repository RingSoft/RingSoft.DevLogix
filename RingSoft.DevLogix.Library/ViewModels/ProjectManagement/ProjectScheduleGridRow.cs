using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleGridRow : DataEntryGridRow
    {
        public new ProjectScheduleGridManager Manager { get; private set; }

        public ProjectScheduleGridRow(ProjectScheduleGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
