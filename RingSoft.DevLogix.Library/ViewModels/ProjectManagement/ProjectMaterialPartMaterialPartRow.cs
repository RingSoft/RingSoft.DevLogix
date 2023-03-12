using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Linq;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    internal class ProjectMaterialPartMaterialPartRow : ProjectMaterialPartRow
    {
        public override MaterialPartLineTypes LineType => MaterialPartLineTypes.MaterialPart;

        public AutoFillSetup MaterialPartAutoFillSetup { get; private set; }

        public AutoFillValue MaterialPartAutoFillValue { get; private set; }

        public decimal Quantity { get; private set; } = 1;

        public decimal Cost { get; private set; }

        public decimal ExtendedCost { get; private set; }

        public ProjectMaterialPartMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
            MaterialPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.MaterialPartLookup);
        }

        public void SetAutoFillValue(AutoFillValue autoFillValue)
        {
            MaterialPartAutoFillValue = autoFillValue;
            SetCost();
        }

        private void SetCost()
        {
            if (MaterialPartAutoFillValue.IsValid())
            {
                var materialPart = MaterialPartAutoFillValue.GetEntity<MaterialPart>();
                if (materialPart != null)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    materialPart = context.GetTable<MaterialPart>()
                        .FirstOrDefault(p => p.Id == materialPart.Id);
                }

                if (materialPart != null)
                {
                    Cost = materialPart.Cost;
                    CalculateRow();
                }
            }
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

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    return new DataEntryGridAutoFillCellProps(this, columnId, MaterialPartAutoFillSetup,
                        MaterialPartAutoFillValue);
                case ProjectMaterialPartColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        AllowNullValue = false,
                    }, Quantity);
                case ProjectMaterialPartColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, Cost);

                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, ExtendedCost);
            }
            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectMaterialPartColumns)value.ColumnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        SetAutoFillValue(autoFillCellProps.AutoFillValue);
                    }
                    break;
                case ProjectMaterialPartColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Quantity = decimalCellProps.Value.Value;
                        CalculateRow();
                    }
                    break;
                case ProjectMaterialPartColumns.Cost:
                    if (value is DataEntryGridDecimalCellProps costDecimalCellProps)
                    {
                        Cost = costDecimalCellProps.Value.Value;
                        CalculateRow();
                    }
                    break;
                case ProjectMaterialPartColumns.ExtendedCost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            MaterialPartAutoFillValue = entity.MaterialPart.GetAutoFillValue();
            Quantity = entity.Quantity.Value;
            Cost = entity.Cost.Value;
            ExtendedCost = GetExtendedCost();

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            entity.MaterialPartId = MaterialPartAutoFillValue.GetEntity<MaterialPart>().Id;
            entity.Quantity = Quantity;
            entity.Cost = Cost;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
