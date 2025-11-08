using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public enum ErrorQaColumns
    {
        Tester = 1,
        NewStatus = 2,
        DateChanged = 3,
    }
    public class ErrorQaManager : DbMaintenanceDataEntryGridManager<ErrorQa>
    {
        public const int TesterColumnId = (int)ErrorQaColumns.Tester;
        public const int NewStatusColumnId = (int)ErrorQaColumns.NewStatus;
        public const int DateChangedColumnId = (int)ErrorQaColumns.DateChanged;

        public new ErrorViewModel ViewModel { get; private set; }

        public ErrorQaManager(ErrorViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ErrorQaRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ErrorQa> ConstructNewRowFromEntity(ErrorQa entity)
        {
            return new ErrorQaRow(this);
        }

        public void AddNewRow(int newStatusId)
        {
            if (AppGlobals.LoggedInUser != null)
            {
                var row = new ErrorQaRow(this);
                row.SetTesterProperties(newStatusId);
                var entity = new ErrorQa();
                row.SaveToEntity(entity, 0);

                var context = AppGlobals.DataRepository.GetDataContext();
                if (context.SaveEntity(entity, "Saving Error Tester"))
                {
                    AddRow(row);
                    Grid?.RefreshGridView();
                }
            }
        }

        protected override void SelectRowForEntity(ErrorQa entity)
        {
            var selRow = Rows.OfType<ErrorQaRow>()
                .FirstOrDefault(p => p.RowId == entity.Id);

            if (selRow != null)
            {
                ViewModel.View.GotoGrid(ErrorGrids.Qa);
                GotoCell(selRow, TesterColumnId);
            }
            base.SelectRowForEntity(entity);
        }
    }
}
