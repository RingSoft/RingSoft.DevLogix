using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public enum ErrorUserColumns
    {
        User = 1,
        TimeSpent = 2,
        Cost = 3,
    }
    public class ErrorUserGridManager : DbMaintenanceDataEntryGridManager<ErrorUser>
    {
        public const int UserColumnId = (int)ErrorUserColumns.User;
        public const int TimeSpentColumnId = (int)ErrorUserColumns.TimeSpent;
        public const int CostColumnId = (int)ErrorUserColumns.Cost;

        public new ErrorViewModel ViewModel { get; private set; }

        public ErrorUserGridManager(ErrorViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void AddUserRow(ErrorUser user)
        {
            var userRow = new ErrorUserRow(this);
            userRow.LoadFromEntity(user);
            AddRow(userRow, Rows.Count - 1);
            Grid?.RefreshGridView();
        }

        protected override DbMaintenanceDataEntryGridRow<ErrorUser> ConstructNewRowFromEntity(ErrorUser entity)
        {
            return new ErrorUserRow(this);
        }

        public void RefreshCost(List<ErrorUser> users)
        {
            ClearRows();
            LoadGrid(users);
            Grid?.RefreshGridView();
        }
        public void RefreshCost(ErrorUser errorUser)
        {
            var rows = Rows.OfType<ErrorUserRow>();
            var userRow = rows.FirstOrDefault(p => p.UserId == errorUser.UserId);
            if (userRow != null)
            {
                userRow.LoadFromEntity(errorUser);
                Grid?.RefreshGridView();
            }
        }

        public void GetTotals(out double minutesSpent, out double totalCost)
        {
            var rows = Rows.OfType<ErrorUserRow>();
            minutesSpent = rows.Sum(p => p.MinutesSpent);
            totalCost = rows.Sum(p => p.Cost);
        }

        protected override void SelectRowForEntity(ErrorUser entity)
        {
            var selRow = Rows.OfType<ErrorUserRow>()
                .FirstOrDefault(p => p.UserId == entity.UserId);

            if (selRow != null)
            {
                ViewModel.View.GotoGrid(ErrorGrids.User);
                GotoCell(selRow, UserColumnId);
            }
            base.SelectRowForEntity(entity);
        }
    }
}
