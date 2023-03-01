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

        protected override DbMaintenanceDataEntryGridRow<ErrorUser> ConstructNewRowFromEntity(ErrorUser entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
