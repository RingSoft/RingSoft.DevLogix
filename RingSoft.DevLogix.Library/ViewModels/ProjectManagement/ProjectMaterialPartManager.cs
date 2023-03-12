using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectMaterialPartColumns
    {
        LineType = 1,
        MaterialPart = 2,
        Quantity = 3,
        Cost = 4,
        ExtendedCost = 5,
    }

    public class ProjectMaterialPartManager : DbMaintenanceDataEntryGridManager<ProjectMaterialPart>
    {
        public const int LineTypeColumnId = (int)ProjectMaterialPartColumns.LineType;
        public const int MaterialPartColumnId = (int)ProjectMaterialPartColumns.MaterialPart;
        public const int QuantityColumnId = (int)ProjectMaterialPartColumns.Quantity;
        public const int CostColumnId = (int)ProjectMaterialPartColumns.Cost;
        public const int ExtendedColumnId = (int)ProjectMaterialPartColumns.ExtendedCost;

        public const int MiscRowDisplayStyleId = 100;
        public const int CommentRowDisplayStyleId = 101;
        public const int OverheadRowDisplayStyleId = 102;

        public new ProjectMaterialViewModel ViewModel { get; set; }
        public ProjectMaterialPartManager(ProjectMaterialViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectMaterialPartNewRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectMaterialPart> ConstructNewRowFromEntity(ProjectMaterialPart entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
