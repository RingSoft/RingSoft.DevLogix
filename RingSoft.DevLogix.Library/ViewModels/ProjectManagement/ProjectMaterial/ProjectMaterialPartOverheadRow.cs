using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialPartOverheadRow: ProjectMaterialPartRow
    {
        public override MaterialPartLineTypes LineType => MaterialPartLineTypes.Overhead;

        public string Description { get; private set; }

        public double Cost { get; private set; }

        public ProjectMaterialPartOverheadRow(ProjectMaterialPartManager manager) : base(manager)
        {
            DisplayStyleId = ProjectMaterialPartManager.OverheadRowDisplayStyleId;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ProjectMaterialPartColumns.Cost:
                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                        AllowNullValue = false,
                    }, (decimal)Cost);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    return new DataEntryGridCellStyle
                    {
                        ColumnHeader = "Description",
                    };
                case ProjectMaterialPartColumns.Quantity:
                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectMaterialPartColumns)value.ColumnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    if (value is DataEntryGridTextCellProps textCellProps)
                    {
                        Description = textCellProps.Text;
                    }
                    break;
                case ProjectMaterialPartColumns.Cost:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Cost = decimalCellProps.Value.Value.ToDouble();
                        Manager.CalculateTotalCost();
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override double GetExtendedCost()
        {
            return Cost;
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            entity.Description = Description;
            entity.Cost = Cost;

            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            Description = entity.Description;
            Cost = entity.Cost.Value;

            base.LoadFromEntity(entity);
        }
    }
}
