using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix
{
    public class TimeControlCellHost : DataEntryGridEditingControlHost<TimeControl>
    {
        public decimal Minutes { get; set; }

        public TimeControlCellHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new TimeCostCellProps(Row, ColumnId, Control.Minutes);
        }

        public override bool HasDataChanged()
        {
            return Control.Minutes != Minutes;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is TimeCostCellProps timeCostCellProps)
            {
                Minutes = timeCostCellProps.Minutes;
            }
        }

        public override bool IsDropDownOpen => false;

        protected override void OnControlLoaded(TimeControl control, DataEntryGridEditingCellProps cellProps, DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is TimeCostCellProps timeCostCellProps)
            {
                Minutes -= timeCostCellProps.Minutes;
                Control.Minutes = timeCostCellProps.Minutes;
            }
            Control.StringEditControl.BorderThickness = new Thickness(0);
            var displayStyle = GetCellDisplayStyle();
            if (displayStyle.SelectionBrush != null)
                Control.SelectionBrush = displayStyle.SelectionBrush;
            if (displayStyle.BackgroundBrush != null)
            {
                control.ControlBrush = displayStyle.BackgroundBrush;
            }

            if (displayStyle.ForegroundBrush != null)
            {
                control.Foreground = displayStyle.ForegroundBrush;
            }

            control.ControlDirty += (sender, args) =>
            {
                OnControlDirty();
            };

            control.StringEditControl.SelectAll();
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            base.ImportDataGridCellProperties(dataGridCell);
            Control.Height = dataGridCell.ActualHeight - 3;
            Control.Button.Height = dataGridCell.ActualHeight - 5;
            Control.ControlBrush = dataGridCell.Background;
            Control.Foreground = dataGridCell.Foreground;
        }
    }
}
