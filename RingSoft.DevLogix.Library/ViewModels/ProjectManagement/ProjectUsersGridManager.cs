using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectUserColumns
    {
        User = 0,
        MinutesSpent = 1,
        Cost = 2,
    }
    public class ProjectUsersGridManager : DbMaintenanceDataEntryGridManager<ProjectUser>
    {
        public const int UserColumnId = (int)ProjectUserColumns.User;
        public const int MinutesSpentColumnId = (int)ProjectUserColumns.MinutesSpent;
        public const int CostColumnId = (int)ProjectUserColumns.Cost;

        public new ProjectMaintenanceViewModel ViewModel { get; private set; }

        public ProjectUsersGridManager(ProjectMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectUsersGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectUser> ConstructNewRowFromEntity(ProjectUser entity)
        {
            return new ProjectUsersGridRow(this);
        }
    }
}
