using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix
{
    public class DevLogixGridCellFactory : LookupGridEditHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == TimeCostCellProps.MinutesControlId)
            {
                return new TimeControlCellHost(grid);
            }
            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
