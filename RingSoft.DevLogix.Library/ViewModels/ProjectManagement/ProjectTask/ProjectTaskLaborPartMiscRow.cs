using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartMiscRow : ProjectTaskLaborPartRow
    {
        public override LaborPartLineTypes LaborPartLineType => LaborPartLineTypes.Miscellaneous;
        public override double GetExtendedMinutesCost()
        {
            return Minutes * Quantity;
        }

        public string Description { get; private set; }

        public double Quantity { get; private set; } = 1;

        public double Minutes { get; private set; }

        public double ExtendedMinutes { get; private set; }

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
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                        FormatType = DecimalEditFormatTypes.Number,
                    }, Quantity);
                case ProjectTaskLaborPartColumns.MinutesCost:
                    return new TimeCostCellProps(this, columnId, Minutes);
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                    return new TimeCostCellProps(this, columnId, ExtendedMinutes);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var displayStyle = new DataEntryGridCellStyle();
            switch (Manager.DisplayMode)
            {
                case DisplayModes.All:
                    displayStyle.State = DataEntryGridCellStates.Enabled;
                    break;
                case DisplayModes.User:
                case DisplayModes.Disabled:
                    displayStyle.State = DataEntryGridCellStates.Disabled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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
                case ProjectTaskLaborPartColumns.Complete:
                case ProjectTaskLaborPartColumns.PercentComplete:
                    displayStyle = base.GetCellStyle(columnId);
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
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Quantity = decimalCellProps.Value.Value;
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
            Manager.CalcPercentComplete();
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            Description = entity.Description;
            Minutes = entity.MinutesCost;
            Quantity = entity.Quantity.Value;
            ExtendedMinutes = GetExtendedMinutesCost();

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            entity.Description = Description;
            entity.MinutesCost = Minutes;
            entity.Quantity = Quantity;

            base.SaveToEntity(entity, rowIndex);
        }
    }
}
