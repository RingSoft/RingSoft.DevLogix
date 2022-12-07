using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UsersGroupsManager : DbMaintenanceDataEntryGridManager<UsersGroup>
    {
        public new UserMaintenanceViewModel ViewModel { get; set; }

        public UsersGroupsManager(UserMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new UsersGroupsRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<UsersGroup> ConstructNewRowFromEntity(UsersGroup entity)
        {
            return new UsersGroupsRow(this);
        }

        public override bool ValidateGrid()
        {
            var rows = Rows.OfType<GroupsUsersGridRow>();
            foreach (var row in rows)
            {
                if (!row.IsNew)
                {
                    if (!row.ValidateRow())
                    {
                        return false;
                    }
                }
            }
            return base.ValidateGrid();
        }

        public List<UsersGroup> GetList()
        {
            var result = new List<UsersGroup>();

            var rows = Rows.OfType<UsersGroupsRow>().Where(p => p.IsNew == false);
            foreach (var row in rows)
            {
                var item = new UsersGroup();
                row.SaveToEntity(item, 0);
                result.Add(item);
            }
            return result;
        }
    }
}
