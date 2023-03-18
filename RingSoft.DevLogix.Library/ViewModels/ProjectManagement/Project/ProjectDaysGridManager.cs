using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

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

        public ProjectMaintenanceViewModel ViewModel { get; private set; }

        public ProjectDaysRow Sunday { get; private set; }
        public ProjectDaysRow Monday { get; private set; }
        public ProjectDaysRow Tuesday { get; private set; }
        public ProjectDaysRow Wednesday { get; private set; }
        public ProjectDaysRow Thursday { get; private set; }
        public ProjectDaysRow Friday { get; private set; }
        public ProjectDaysRow Saturday { get; private set; }

        public ProjectDaysGridManager(ProjectMaintenanceViewModel viewModel)
        {
            ViewModel = viewModel;
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

        public void Clear()
        {
            foreach (var projectDaysRow in Rows.OfType<ProjectDaysRow>())
            {
                projectDaysRow.Clear();
            }
            Grid?.RefreshGridView();
        }
        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void SaveToEntity(Project project)
        {
            project.SundayMinutes = Sunday.WorkMinutes;
            project.MondayMinutes = Monday.WorkMinutes;
            project.TuesdayMinutes = Tuesday.WorkMinutes;
            project.WednesdayMinutes = Wednesday.WorkMinutes;
            project.ThursdayMinutes = Thursday.WorkMinutes;
            project.FridayMinutes = Friday.WorkMinutes;
            project.SaturdayMinutes = Saturday.WorkMinutes;
        }

        public void LoadFromEntity(Project project)
        {
            Sunday.SetWorkMinutes(project.SundayMinutes);
            Monday.SetWorkMinutes(project.MondayMinutes);
            Tuesday.SetWorkMinutes(project.TuesdayMinutes);
            Wednesday.SetWorkMinutes(project.WednesdayMinutes);
            Thursday.SetWorkMinutes(project.ThursdayMinutes);
            Friday.SetWorkMinutes(project.FridayMinutes);
            Saturday.SetWorkMinutes(project.SaturdayMinutes);
            Grid?.RefreshGridView();
        }
    }
}
