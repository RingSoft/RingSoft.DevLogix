using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum UserTimeOffColumns
    {
        StartDate = 1,
        EndDate = 2,
        Description = 3,
    }
    public class UserTimeOffGridManager : DbMaintenanceDataEntryGridManager<UserTimeOff>
    {   
        public const int StartDateColumnId = (int)UserTimeOffColumns.StartDate;
        public const int EndDateColumnId = (int)UserTimeOffColumns.EndDate;
        public const int DescriptionColumnId = (int)UserTimeOffColumns.Description;

        public new UserMaintenanceViewModel ViewModel { get; private set; }

        public UserTimeOffGridManager(UserMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new UserTimeOffRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<UserTimeOff> ConstructNewRowFromEntity(UserTimeOff entity)
        {
            return new UserTimeOffRow(this);
        }
    }
}
