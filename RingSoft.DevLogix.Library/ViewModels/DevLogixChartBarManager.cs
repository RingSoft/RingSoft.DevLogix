using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public enum ChartBarColumns
    {
        AdvFind = 1,
        Name = 2,
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

        public List<DevLogixChartBar> GetRowsList()
        {
            var result = new List<DevLogixChartBar>();
            var chartBarRows = Rows.OfType<DevLogixChartBarRow>().Where(p => p.IsNew == false);
            var rowIndex = 1;

            foreach (var devLogixChartBarRow in chartBarRows)
            {
                var newResult = new DevLogixChartBar();
                devLogixChartBarRow.SaveToEntity(newResult, rowIndex);
                result.Add(newResult);
                rowIndex++;
            }
            return result;
        }
    }
}
