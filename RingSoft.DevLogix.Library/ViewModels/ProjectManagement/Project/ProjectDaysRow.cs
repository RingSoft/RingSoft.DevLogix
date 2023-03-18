using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectDaysRow : DataEntryGridRow
    {
        public new ProjectDaysGridManager Manager { get; private set; }

        public string Day { get; private set; }

        public decimal WorkMinutes { get; private set; }

        public ProjectDaysRow(ProjectDaysGridManager manager, string day) : base(manager)
        {
             Manager = manager;
             Day = day;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
