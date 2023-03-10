using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum TotalsColumns
    {
        Type = 0,
        Time = 1,
        Cost = 2,
    }
    public class ProjectTotalsManager : DataEntryGridManager
    {
        public const int TypeColumnId = (int)TotalsColumns.Type;
        public const int TotalTimeColumnId = (int)TotalsColumns.Time;
        public const int TotalCostColumnId = (int)TotalsColumns.Cost;

        public ProjectTotalsRow EstimatedRow { get; private set; }
        public ProjectTotalsRow RemainingRow { get; private set; }
        
        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectTotalsRow(this);
        }

        public void Initialize()
        {
            EstimatedRow = new ProjectTotalsRow(this);
            EstimatedRow.RowTitle = "Estimated";
            InsertRow(EstimatedRow);

            RemainingRow = new ProjectTotalsRow(this);
            RemainingRow.RowTitle = "Remaining";
            InsertRow(RemainingRow);
        }

        public void InsertRow(ProjectTotalsRow row, int index = -1)
        {
            AddRow(row, index);
        }

        public void RefreshGrid()
        {
            Grid?.RefreshGridView();
        }
    }
}
