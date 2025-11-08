using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public enum ErrorDeveloperGridColumns
    {
        Developer = 1,
        DateFixed = 2,
    }
    public class ErrorDeveloperManager : DbMaintenanceDataEntryGridManager<ErrorDeveloper>
    {
        public const int DeveloperColumnId = (int)ErrorDeveloperGridColumns.Developer;
        public const int DateFixedColumnId = (int)ErrorDeveloperGridColumns.DateFixed;

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

        public void AddNewRow()
        {
            if (AppGlobals.LoggedInUser != null)
            {
                var row = new ErrorDeveloperRow(this);
                row.SetDeveloperProperties();
                var entity = new ErrorDeveloper();
                row.SaveToEntity(entity, 0);

                var context = AppGlobals.DataRepository.GetDataContext();
                if (context.SaveEntity(entity, "Saving Error Developer"))
                {
                    AddRow(row);
                    Grid?.RefreshGridView();
                }
            }
        }

        protected override void SelectRowForEntity(ErrorDeveloper entity)
        {
            var selRow = Rows.OfType<ErrorDeveloperRow>()
                .FirstOrDefault(p => p.RowId == entity.Id);

            if (selRow != null)
            {
                ViewModel.View.GotoGrid(ErrorGrids.Developers);
                GotoCell(selRow, DeveloperColumnId);
            }
            base.SelectRowForEntity(entity);
        }
    }
}
