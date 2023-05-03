using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserTrackerUserRow : DbMaintenanceDataEntryGridRow<UserTrackerUser>
    {
        public new UserTrackerUserManager Manager { get; }
        public UserTrackerUserRow(UserTrackerUserManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void LoadFromEntity(UserTrackerUser entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(UserTrackerUser entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
