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

    public enum DayType
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
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
            Sunday = new ProjectDaysRow(this, "Sunday", DayType.Sunday);
            Monday = new ProjectDaysRow(this, "Monday", DayType.Monday);
            Tuesday = new ProjectDaysRow(this, "Tuesday", DayType.Tuesday);
            Wednesday = new ProjectDaysRow(this, "Wednesday", DayType.Wednesday);
            Thursday = new ProjectDaysRow(this, "Thursday", DayType.Thursday);
            Friday = new ProjectDaysRow(this, "Friday", DayType.Friday);
            Saturday = new ProjectDaysRow(this, "Saturday", DayType.Saturday);
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

        public void SetStandardUserTime(ProjectDaysRow row)
        {
            ViewModel.UsersGridManager.SetUserMinutes(row.WorkMinutes, row.DayType);
        }

        public decimal GetStandardMinutes(DayType dayType)
        {
            var rows = Rows.OfType<ProjectDaysRow>();
            var rowFound = rows.FirstOrDefault(p => p.DayType == dayType);
            if (rowFound != null)
            {
                return rowFound.WorkMinutes;
            }
            return 0;
        }
    }
}
