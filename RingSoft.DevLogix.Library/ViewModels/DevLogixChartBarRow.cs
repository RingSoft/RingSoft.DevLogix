using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class DevLogixChartBarRow : DbMaintenanceDataEntryGridRow<DevLogixChartBar>
    {
        public new DevLogixChartBarManager Manager { get; set; }

        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public string Name { get; set; }

        public DevLogixChartBarRow(DevLogixChartBarManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.AdvancedFindLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ChartBarColumns)columnId;

            switch (column)
            {
                case ChartBarColumns.AdvFind:
                    return new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue);
                case ChartBarColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ChartBarColumns)value.ColumnId;

            switch (column)
            {
                case ChartBarColumns.AdvFind:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        AutoFillValue = autoFillCellProps.AutoFillValue;
                    }
                    break;
                case ChartBarColumns.Name:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Name = textCellProps.Text;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(DevLogixChartBar entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(DevLogixChartBar entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
