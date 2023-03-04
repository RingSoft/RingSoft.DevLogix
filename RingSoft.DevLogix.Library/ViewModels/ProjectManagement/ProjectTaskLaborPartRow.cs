using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public abstract class ProjectTaskLaborPartRow  : DbMaintenanceDataEntryGridRow<ProjectTaskLaborPart>
    {
        public new ProjectTaskLaborPartsManager Manager { get; private set; }

        public decimal MinutesCost { get; private set; }

        protected ProjectTaskLaborPartRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    break;
                case ProjectTaskLaborPartColumns.Quantity:
                    break;
                case ProjectTaskLaborPartColumns.MinutesCost:
                    return new TimeCostCellProps(this, columnId, MinutesCost);
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                    break;
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectTaskLaborPartColumns)value.ColumnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    break;
                case ProjectTaskLaborPartColumns.LaborPart:
                    break;
                case ProjectTaskLaborPartColumns.Quantity:
                    break;
                case ProjectTaskLaborPartColumns.MinutesCost:
                    if (value is TimeCostCellProps timeCostCellProps)
                    {
                        MinutesCost = timeCostCellProps.Minutes;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }
    }
}
