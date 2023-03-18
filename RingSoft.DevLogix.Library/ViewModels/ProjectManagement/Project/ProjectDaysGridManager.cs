using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectDaysColumns
    {
        Day = 0,
        Time = 1,
    }
    public class ProjectDaysGridManager : DataEntryGridManager
    {
        public const int DayColumnId = (int)ProjectDaysColumns.Day;
        public const int TimeColumnId = (int)ProjectDaysColumns.Time;

        public ProjectDaysRow Sunday { get; private set; }
        public ProjectDaysRow Monday { get; private set; }
        public ProjectDaysRow Tuesday { get; private set; }
        public ProjectDaysRow Wednesday { get; private set; }
        public ProjectDaysRow Thursday { get; private set; }
        public ProjectDaysRow Friday { get; private set; }
        public ProjectDaysRow Saturday { get; private set; }

        public ProjectDaysGridManager()
        {
            Sunday = new ProjectDaysRow(this, "Sunday");
            Monday = new ProjectDaysRow(this, "Monday");
            Tuesday = new ProjectDaysRow(this, "Tuesday");
            Wednesday = new ProjectDaysRow(this, "Wednesday");
            Thursday = new ProjectDaysRow(this, "Thursday");
            Friday = new ProjectDaysRow(this, "Friday");
            Saturday = new ProjectDaysRow(this, "Saturday");
        }

        public void Initialize()
        {
            AddRow(Sunday);
            AddRow(Monday);
            AddRow(Tuesday);
            AddRow(Wednesday);
            AddRow(Thursday);
            AddRow(Friday);
            AddRow(Saturday);
        }
        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }
    }
}
