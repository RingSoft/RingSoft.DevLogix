using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class SupportTicketErrorManager : DbMaintenanceDataEntryGridManager<SupportTicketError>
    {
        public new SupportTicketViewModel ViewModel { get; }

        public SupportTicketErrorManager(SupportTicketViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new SupportTicketErrorRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<SupportTicketError> ConstructNewRowFromEntity(SupportTicketError entity)
        {
            return new SupportTicketErrorRow(this);
        }
    }
}
