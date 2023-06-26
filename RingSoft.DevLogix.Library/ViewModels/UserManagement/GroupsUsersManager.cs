using MySqlX.XDevAPI.Relational;
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

        public List<UsersGroup> GetList()
        {
            var result = new List<UsersGroup>();

            var rows = Rows.OfType<GroupsUsersGridRow>();
            foreach (var row in rows)
            {
                if (!row.IsNew)
                {
                    var item = new UsersGroup();
                    row.SaveToEntity(item, 0);
                    result.Add(item);
                }
            }
            return result;
        }

    }
}
