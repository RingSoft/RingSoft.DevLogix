using System;
using RingSoft.DataEntryControls.Engine;
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

        public bool IsDisposed { get; private set; }

        public int BarId { get; private set; }

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
                    var props = new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue);
                    if (!IsNew)
                    {
                        props.AlwaysUpdateOnSelect = true;
                    }
                    return props;
                case ChartBarColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var alwaysUpdate = false;
            var column = (ChartBarColumns)value.ColumnId;

            switch (column)
            {
                case ChartBarColumns.AdvFind:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        if (AutoFillValue == null)
                        {
                            alwaysUpdate = true;
                        }
                        else
                        {
                            if (!autoFillCellProps.AutoFillValue.PrimaryKeyValue
                                    .IsEqualTo(AutoFillValue.PrimaryKeyValue))
                            {
                                alwaysUpdate = true;
                            }
                        }
                        AutoFillValue = autoFillCellProps.AutoFillValue;
                        if (Name.IsNullOrEmpty() && AutoFillValue.IsValid())
                        {
                            Name = AutoFillValue.Text;
                        }
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
            IsNew = false;
            if (alwaysUpdate)
            {
                Manager.ViewModel.RefreshChart();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(DevLogixChartBar entity)
        {
            AutoFillValue = AutoFillSetup.GetAutoFillValueForIdValue(entity.AdvancedFindId);
            Name = entity.Name;
            BarId = entity.BarId;
        }

        public override bool ValidateRow()
        {
            if (IsNew)
            {
                return true;
            }
            if (!AutoFillValue.IsValid())
            {
                Manager.ViewModel.View.GotoGrid();

                var message = "Advanced Find contains an invalid value.";
                var caption = "Validation Fail";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, DevLogixChartBarManager.AdvFindColumnId);
                return false;
            }

            if (Name.IsNullOrEmpty())
            {
                Manager.ViewModel.View.GotoGrid();

                var message = "Name must have a value.";
                var caption = "Validation Fail";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, DevLogixChartBarManager.NameColumnId);
                return false;
            }
            return true;
        }

        public override void SaveToEntity(DevLogixChartBar entity, int rowIndex)
        {
            entity.BarId = rowIndex;
            entity.AdvancedFindId = AutoFillValue.GetEntity(AppGlobals.LookupContext.AdvancedFinds).Id;
            entity.Name = Name;
        }

        public override void Dispose()
        {
            base.Dispose();
            IsDisposed = true;
            Manager.ViewModel.RefreshChart();
        }
    }
}
