using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum UserTrackerColumns
    {
        User = 0,
        PunchedOut = 1,
        PunchedIn = 2,
        TimeClock = 3,
    }
    public class UserTrackerUserManager : DbMaintenanceDataEntryGridManager<UserTrackerUser>
    {
        public const int UserColumnId = (int)UserTrackerColumns.User;
        public const int PunchedOutColumnId = (int)UserTrackerColumns.PunchedOut;
        public const int PunchedInColumnId = (int)UserTrackerColumns.PunchedIn;
        public const int TimeClockColumnId = (int)UserTrackerColumns.TimeClock;

        public new UserTrackerViewModel ViewModel { get; }

        public UserTrackerUserManager(UserTrackerViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new UserTrackerUserRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<UserTrackerUser> ConstructNewRowFromEntity(UserTrackerUser entity)
        {
            return new UserTrackerUserRow(this);
        }
    }
}
