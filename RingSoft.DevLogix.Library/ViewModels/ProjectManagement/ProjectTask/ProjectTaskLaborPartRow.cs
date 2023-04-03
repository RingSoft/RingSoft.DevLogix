using System;
using System.Collections.Generic;
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

        //public decimal MinutesCost { get; private set; }

        public bool IsComplete { get; private set; }

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
                case ProjectTaskLaborPartColumns.Complete:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, IsComplete);
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var cellStyle = new DataEntryGridCellStyle();
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    cellStyle.ColumnHeader = "Labor Part";
                    switch (Manager.DisplayMode)
                    {
                        case DisplayModes.All:
                            cellStyle.State = DataEntryGridCellStates.Enabled;
                            break;
                        case DisplayModes.User:
                        case DisplayModes.Disabled:
                            cellStyle.State = DataEntryGridCellStates.Disabled;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return cellStyle;
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                case ProjectTaskLaborPartColumns.LineType:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
                case ProjectTaskLaborPartColumns.Complete:
                    cellStyle = new DataEntryGridControlCellStyle();
                    switch (Manager.DisplayMode)
                    {
                        case DisplayModes.User:
                        case DisplayModes.All:
                            cellStyle.State = DataEntryGridCellStates.Enabled;
                            break;
                        case DisplayModes.Disabled:
                            cellStyle.State = DataEntryGridCellStates.Disabled;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return cellStyle;
                default:
                    switch (Manager.DisplayMode)
                    {
                        case DisplayModes.All:
                            cellStyle.State = DataEntryGridCellStates.Enabled;
                            break;
                        case DisplayModes.User:
                        case DisplayModes.Disabled:
                            cellStyle.State = DataEntryGridCellStates.Disabled;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return cellStyle;
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
                //    if (value is TimeCostCellProps timeCostCellProps)
                //    {
                //        MinutesCost = timeCostCellProps.Minutes;
                //    }
                    break;
                case ProjectTaskLaborPartColumns.Complete:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        IsComplete = checkBoxCellProps.Value;
                    }
                    Manager.CalcPercentComplete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            IsComplete = entity.Complete;
        }

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            entity.DetailId = rowIndex;
            entity.RowId = RowId;
            entity.LineType = (byte)LaborPartLineType;
            entity.ParentRowId = ParentRowId;
            entity.Complete = IsComplete;
        }

        protected IEnumerable<ProjectTaskLaborPart> GetDetailChildren(ProjectTaskLaborPart entity)
        {
            var result = Manager.Details.Where(w =>
                w.ParentRowId != null && w.ParentRowId == entity.RowId).OrderBy(p => p.DetailId);
            return result;
        }

        public virtual void LoadChildren(ProjectTaskLaborPart entity)
        {

        }
    }
}
