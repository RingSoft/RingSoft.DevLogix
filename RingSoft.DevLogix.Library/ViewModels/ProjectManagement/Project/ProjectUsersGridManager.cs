using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectUserColumns
    {
        User = 0,
        MinutesSpent = 1,
        Cost = 2,
        IsStandard = 3,
        Sunday = 4,
        Monday = 5,
        Tuesday = 6,
        Wednesday = 7,
        Thursday = 8,
        Friday = 9,
        Saturday = 10,
    }

    public class ProjectUsersGridManager : DbMaintenanceDataEntryGridManager<ProjectUser>
    {
        public const int UserColumnId = (int)ProjectUserColumns.User;
        public const int MinutesSpentColumnId = (int)ProjectUserColumns.MinutesSpent;
        public const int CostColumnId = (int)ProjectUserColumns.Cost;
        public const int IsStandardColumnId = (int)ProjectUserColumns.IsStandard;
        public const int SundayColumnId = (int)ProjectUserColumns.Sunday;
        public const int MondayColumnId = (int)ProjectUserColumns.Monday;
        public const int TuesdayColumnId = (int)ProjectUserColumns.Tuesday;
        public const int WednesdayColumnId = (int)ProjectUserColumns.Wednesday;
        public const int ThursdayColumnId = (int)ProjectUserColumns.Thursday;
        public const int FridayColumnId = (int)ProjectUserColumns.Friday;
        public const int SaturdayColumnId = (int)ProjectUserColumns.Saturday;

        public new ProjectMaintenanceViewModel ViewModel { get; private set; }

        public bool GotoNewRow { get; set; }

        private bool _gotoNewRow;
        private bool _loading;

        public ProjectUsersGridManager(ProjectMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        internal void IntGotoNewRow()
        {
            _gotoNewRow = true;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            var result = new ProjectUsersGridRow(this);
            return result;
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectUser> ConstructNewRowFromEntity(ProjectUser entity)
        {
            return new ProjectUsersGridRow(this);
        }

        public override void LoadGrid(IEnumerable<ProjectUser> entityList)
        {
            _loading = true;
            base.LoadGrid(entityList);
            _loading = false;
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!_loading && _gotoNewRow)
                {
                    GotoNewRow = true;
                }
            }
            base.OnRowsChanged(e);
        }

        public void SetUserMinutes(double minutes, DayType dayType)
        {
            var users = GetStandardUsers();
            foreach (var projectUsersGridRow in users)
            {
                switch (dayType)
                {
                    case DayType.Sunday:
                        projectUsersGridRow.SundayMinutes = minutes;
                        break;
                    case DayType.Monday:
                        projectUsersGridRow.MondayMinutes = minutes;
                        break;
                    case DayType.Tuesday:
                        projectUsersGridRow.TuesdayMinutes = minutes;
                        break;
                    case DayType.Wednesday:
                        projectUsersGridRow.WednesdayMinutes = minutes;
                        break;
                    case DayType.Thursday:
                        projectUsersGridRow.ThursdayMinutes = minutes;
                        break;
                    case DayType.Friday:
                        projectUsersGridRow.FridayMinutes = minutes;
                        break;
                    case DayType.Saturday:
                        projectUsersGridRow.SaturdayMinutes = minutes;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dayType), dayType, null);
                }
            }
            Grid?.RefreshGridView();
        }

        public IEnumerable<ProjectUsersGridRow> GetStandardUsers()
        {
            return Rows.OfType<ProjectUsersGridRow>().Where(
                p => p.IsStandard);
        }

        public ProjectUsersGridRow GetProjectUsersGridRow(int userId)
        {
            var rows = Rows.OfType<ProjectUsersGridRow>();
            if (userId == 0)
            {
                return rows.FirstOrDefault(p => p.IsNew);
            }
            else
            {
                return rows.FirstOrDefault(p => p.UserId == userId);
            }
        }

        public bool ValidateCalc()
        {
            var rows = Rows.OfType<ProjectUsersGridRow>()
                .Where(p => p.IsNew == false);

            var caption = "Calculation Validation";
            if (!rows.Any())
            {
                ViewModel.View.GotoGrid();
                var message = "No users defined for this project.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Grid?.GotoCell(Rows[0], UserColumnId);
                return false;
            }
            foreach (var projectUsersGridRow in rows)
            {
                if (!projectUsersGridRow.UserAutoFillValue.IsValid())
                {
                    ViewModel.View.GotoGrid();
                    var message = "Invalid User";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    Grid?.GotoCell(projectUsersGridRow, UserColumnId);
                    return false;
                }
                var totalMinutes = projectUsersGridRow.GetTotalMinutes();
                if (totalMinutes <= 0)
                {
                    ViewModel.View.GotoGrid();
                    var message = $"The User '{projectUsersGridRow.UserAutoFillValue.Text}' does not have any working time set up in the users grid.";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    Grid?.GotoCell(projectUsersGridRow, UserColumnId);
                    return false;
                }
            }
            return true;
        }
    }
}
