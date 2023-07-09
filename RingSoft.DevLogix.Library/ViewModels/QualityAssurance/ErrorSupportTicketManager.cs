using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorSupportTicketManager : DbMaintenanceDataEntryGridManager<SupportTicketError>
    {
        public new ErrorViewModel ViewModel { get; }
        public ErrorSupportTicketManager(ErrorViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ErrorSupportTicketRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<SupportTicketError> ConstructNewRowFromEntity(SupportTicketError entity)
        {
            return new ErrorSupportTicketRow(this);
        }
    }
}
