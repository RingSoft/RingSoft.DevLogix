using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectMaterialPostColumns
    {
        Date = 1,
        Material = 2,
        Quantity = 3,
        Cost = 4,
        Extended = 5,
    }
    public class ProjectMaterialPostManager : DataEntryGridManager
    {
        public const int DateColumnId = (int)ProjectMaterialPostColumns.Date;
        public const int MaterialColumnId = (int)ProjectMaterialPostColumns.Material;
        public const int QuantityColumnId = (int)ProjectMaterialPostColumns.Quantity;
        public const int CostColumnId = (int)ProjectMaterialPostColumns.Cost;
        public const int ExtendedColumnId = (int)ProjectMaterialPostColumns.Extended;

        public ProjectMaterialPostViewModel ViewModel { get; private set; }

        public ProjectMaterialPostManager(ProjectMaterialPostViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectMaterialPostRow(this);
        }
    }
}
