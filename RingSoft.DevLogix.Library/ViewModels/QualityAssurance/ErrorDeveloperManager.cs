using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class ErrorDeveloperManager : DbMaintenanceDataEntryGridManager<ErrorDeveloper>
    {
        public new ErrorViewModel ViewModel { get; private set; }

        public ErrorDeveloperManager(ErrorViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ErrorDeveloperRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ErrorDeveloper> ConstructNewRowFromEntity(ErrorDeveloper entity)
        {
            return new ErrorDeveloperRow(this);
        }
    }
}
