using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialPartMiscRow : ProjectMaterialPartRow
    {
        public override MaterialPartLineTypes LineType => MaterialPartLineTypes.Miscellaneous;

        public string Description { get; private set; }

        public decimal Quantity { get; private set; } = 1;

        public decimal Cost { get; private set; }

        public decimal ExtendedCost { get; private set; }

        public ProjectMaterialPartMiscRow(ProjectMaterialPartManager manager) : base(manager)
        {
            DisplayStyleId = ProjectMaterialPartManager.MiscRowDisplayStyleId;
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
                case ProjectMaterialPartColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                    }, Quantity);
                case ProjectMaterialPartColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                        AllowNullValue = false,
                    }, Cost);
                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                        AllowNullValue = false,
                    }, ExtendedCost);
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
                case ProjectMaterialPartColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps qtyDecimalCellProps)
                    {
                        Quantity = qtyDecimalCellProps.Value.Value;
                        CalculateRow();
                    }
                    break;

                case ProjectMaterialPartColumns.Cost:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Cost = decimalCellProps.Value.Value;
                        CalculateRow();
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        private void CalculateRow()
        {
            ExtendedCost = GetExtendedCost();
            Manager.CalculateTotalCost();
        }

        public override decimal GetExtendedCost()
        {
            return Quantity * Cost;
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            entity.Description = Description;
            entity.Quantity = Quantity;
            entity.Cost = Cost;

            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            Description = entity.Description;
            Quantity = entity.Quantity.Value;
            Cost = entity.Cost.Value;
            ExtendedCost = GetExtendedCost();

            base.LoadFromEntity(entity);
        }

    }
}
