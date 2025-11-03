using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public enum SupportTicketUserColumns
    {
        User = 1,
        TimeSpent = 2,
        Cost = 3,
    }

    public class SupportTicketCostManager : DbMaintenanceDataEntryGridManager<SupportTicketUser>
    {
        public const int UserColumnId = (int)SupportTicketUserColumns.User;
        public const int TimeSpentColumnId = (int)SupportTicketUserColumns.TimeSpent;
        public const int CostColumnId = (int)SupportTicketUserColumns.Cost;

        public new SupportTicketViewModel ViewModel { get; }
        public SupportTicketCostManager(SupportTicketViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void AddUserRow(SupportTicketUser user)
        {
            var userRow = new SupportTicketCostRow(this);
            userRow.LoadFromEntity(user);
            AddRow(userRow, Rows.Count - 1);
        }
        protected override DbMaintenanceDataEntryGridRow<SupportTicketUser> ConstructNewRowFromEntity(SupportTicketUser entity)
        {
            return new SupportTicketCostRow( this);
        }

        public void RefreshCost(List<SupportTicketUser> users)
        {
            ClearRows();
            LoadGrid(users);
            Grid?.RefreshGridView();
        }
        public void RefreshCost(SupportTicketUser ticketUser)
        {
            var rows = Rows.OfType<SupportTicketCostRow>();
            var userRow = rows.FirstOrDefault(p => p.UserId == ticketUser.UserId);
            if (userRow != null)
            {
                userRow.LoadFromEntity(ticketUser);
                Grid?.RefreshGridView();
            }
        }

        public void GetTotals(out double minutesSpent, out double totalCost)
        {
            var rows = Rows.OfType<SupportTicketCostRow>();
            minutesSpent = rows.Sum(p => p.MinutesSpent);
            totalCost = rows.Sum(p => p.Cost);
        }

        public override void LoadGrid(IEnumerable<SupportTicketUser> entityList)
        {
            base.LoadGrid(entityList);
            ViewModel.GetTotals();
        }
    }
}
