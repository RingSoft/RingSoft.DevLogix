using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartMiscRow : ProjectTaskLaborPartRow
    {
        public override LaborPartLineTypes LaborPartLineType => LaborPartLineTypes.Miscellaneous;
        public override decimal GetExtendedMinutesCost()
        {
            return Minutes * Quantity;
        }

        public string Description { get; private set; }

        public int Quantity { get; private set; } = 1;

        public decimal Minutes { get; private set; }

        public decimal ExtendedMinutes { get; private set; }

        public ProjectTaskLaborPartMiscRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            DisplayStyleId = ProjectTaskLaborPartsManager.MiscRowDisplayStyleId;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    break;
                case ProjectTaskLaborPartColumns.LaborPart:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ProjectTaskLaborPartColumns.Quantity:
                    return new DataEntryGridIntegerCellProps(this, columnId, new IntegerEditControlSetup
                    {
                        AllowNullValue = false,
                        MinimumValue = 1,
                    }, Quantity);
                case ProjectTaskLaborPartColumns.MinutesCost:
                    return new TimeCostCellProps(this, columnId, Minutes);
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                    return new TimeCostCellProps(this, columnId, ExtendedMinutes);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var displayStyle = new DataEntryGridCellStyle();

            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LineType:
                    displayStyle.State = DataEntryGridCellStates.Disabled;
                    break;
                case ProjectTaskLaborPartColumns.LaborPart:
                    displayStyle.ColumnHeader = "Description";
                    break;
                case ProjectTaskLaborPartColumns.Quantity:
                    break;
                case ProjectTaskLaborPartColumns.MinutesCost:
                    break;
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                    displayStyle.State = DataEntryGridCellStates.Disabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return displayStyle;
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectTaskLaborPartColumns)value.ColumnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Description = textCellProps.Text;
                    }
                    break;
                case ProjectTaskLaborPartColumns.Quantity:
                    if (value is DataEntryGridIntegerCellProps integerCellProps)
                    {
                        Quantity = integerCellProps.Value.Value;
                        if (Quantity == 0)
                        {
                            Quantity = 1;
                        }
                        CalculateRow();
                    }
                    break;
                case ProjectTaskLaborPartColumns.MinutesCost:
                    if (value is TimeCostCellProps timeCostCellProps)
                    {
                        Minutes = timeCostCellProps.Minutes;
                        CalculateRow();
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        private void CalculateRow()
        {
            ExtendedMinutes = Quantity * Minutes;
            Manager.CalculateTotalMinutesCost();
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
