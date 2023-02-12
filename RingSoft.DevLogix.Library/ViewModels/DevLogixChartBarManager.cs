using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum ChartBarColumns
    {
        AdvFind = 0,
        Name = 1,
    }

    public class DevLogixChartBarManager : DbMaintenanceDataEntryGridManager<DevLogixChartBar>
    {
        public const int AdvFindColumnId = (int)ChartBarColumns.AdvFind;
        public const int NameColumnId = (int)ChartBarColumns.Name;

        public new DevLogixChartViewModel ViewModel { get; set; }

        public DevLogixChartBarManager(DevLogixChartViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new DevLogixChartBarRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<DevLogixChartBar> ConstructNewRowFromEntity(DevLogixChartBar entity)
        {
            return new DevLogixChartBarRow(this);
        }
    }
}
