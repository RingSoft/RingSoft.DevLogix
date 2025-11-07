using System.Linq;
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

        protected override void SelectRowForEntity(SupportTicketError entity)
        {
            var errorRow = Rows.OfType<SupportTicketErrorRow>()
                .FirstOrDefault(p => p.ErrorId == entity.ErrorId);
            if (errorRow != null)
            {
                ViewModel.View.GotoTab(SupportTicketsTab.Errors);
                GotoCell(errorRow, 0);
            }

            base.SelectRowForEntity(entity);
        }
    }
}
