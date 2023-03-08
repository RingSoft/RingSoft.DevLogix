using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum LaborPartLineTypes
    {
        [Description("New Row")]
        NewRow = 0,
        [Description("Labor Part")]
        LaborPart = 1,
        [Description("Miscellaneous")]
        Miscellaneous = 2,
        [Description("Comment")]
        Comment = 3,
    }
    public abstract class ProjectTaskLaborPartRow  : DbMaintenanceDataEntryGridRow<ProjectTaskLaborPart>
    {
        public new ProjectTaskLaborPartsManager Manager { get; private set; }

        public abstract LaborPartLineTypes LaborPartLineType { get; }

        public abstract decimal GetExtendedMinutesCost();

        public EnumFieldTranslation EnumTranslation { get; private set; } = new EnumFieldTranslation();

        public decimal MinutesCost { get; private set; }

        protected ProjectTaskLaborPartRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            Manager = manager;
            EnumTranslation.LoadFromEnum<LaborPartLineTypes>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    return new DataEntryGridTextCellProps(this, columnId, EnumTranslation.TypeTranslations
                        .FirstOrDefault(p => p.NumericValue == (int)LaborPartLineType).TextValue);
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    return new DataEntryGridCellStyle
                    {
                        ColumnHeader = "Labor Part",
                    };
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                case ProjectTaskLaborPartColumns.LineType:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
                    };
            }
            return base.GetCellStyle(columnId);
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

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            entity.DetailId = rowIndex;
            entity.RowId = RowId;
        }
    }
}
