using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectTaskLaborPartColumns
    {
        LineType = 1,
        LaborPart = 2,
        Quantity = 3,
        MinutesCost = 4,
        ExtendedMinutes = 5,
    }
    public class ProjectTaskLaborPartsManager : DbMaintenanceDataEntryGridManager<ProjectTaskLaborPart>
    {
        public const int LineTypeColumnId = (int)ProjectTaskLaborPartColumns.LineType;
        public const int LaborPartColumnId = (int)ProjectTaskLaborPartColumns.LaborPart;
        public const int QuantityColumnId = (int)ProjectTaskLaborPartColumns.Quantity;
        public const int MinutesCostColumnId = (int)ProjectTaskLaborPartColumns.MinutesCost;
        public const int ExtendedMinutesColumnId = (int)ProjectTaskLaborPartColumns.ExtendedMinutes;

        public const int MiscRowDisplayStyleId = 100;
        public const int CommentRowDisplayStyleId = 101;

        public new ProjectTaskViewModel ViewModel { get; private set; }

        public ProjectTaskLaborPartsManager(ProjectTaskViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectTaskLaborPartNewRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectTaskLaborPart> ConstructNewRowFromEntity(ProjectTaskLaborPart entity)
        {
            return new ProjectTaskLaborPartLaborPartRow(this);
        }
    }
}
