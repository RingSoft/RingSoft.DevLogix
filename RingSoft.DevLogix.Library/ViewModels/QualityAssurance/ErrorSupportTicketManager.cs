using System.Linq;
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

        protected override void SelectRowForEntity(SupportTicketError entity)
        {
            var selRow = Rows.OfType<ErrorSupportTicketRow>()
                .FirstOrDefault(p => p.SupportTicketId == entity.SupportTicketId);

            if (selRow != null)
            {
                ViewModel.View.GotoGrid(ErrorGrids.SupportTicket);
                GotoCell(selRow, 0);
            }

            base.SelectRowForEntity(entity);
        }
    }
}
