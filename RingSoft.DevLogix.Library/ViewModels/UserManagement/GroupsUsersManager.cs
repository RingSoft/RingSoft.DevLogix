using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class GroupsUsersManager : DbMaintenanceDataEntryGridManager<UsersGroup>
    {
        public new GroupMaintenanceViewModel ViewModel { get; set; }

        public GroupsUsersManager(GroupMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new GroupsUsersGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<UsersGroup> ConstructNewRowFromEntity(UsersGroup entity)
        {
            return new GroupsUsersGridRow(this);
        }
    }
}
