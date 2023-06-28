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

        public double Quantity { get; private set; } = 1;

        public double Cost { get; private set; }

        public double ExtendedCost { get; private set; }

        public ProjectMaterialPartMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
            MaterialPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.MaterialPartLookup);
        }

        public void SetAutoFillValue(AutoFillValue autoFillValue)
        {
            MaterialPartAutoFillValue = autoFillValue;
            var materialPart = MaterialPartAutoFillValue.GetEntity<MaterialPart>();
            if (materialPart != null)
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                materialPart = context.GetTable<MaterialPart>()
                    .FirstOrDefault(p => p.Id == materialPart.Id);
            }
            if (materialPart != null)
            {
                if (!materialPart.Comment.IsNullOrEmpty())
                {
                    var commentRow = new ProjectMaterialPartCommentRow(Manager);
                    AddChildRow(commentRow);
                    commentRow.SetValue(materialPart.Comment);
                }
            }

            SetCost(materialPart);
        }

        private void SetCost(MaterialPart materialPart)
        {
            if (MaterialPartAutoFillValue.IsValid())
            {
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

        public override double GetExtendedCost()
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
                    }, (decimal)Quantity);
                case ProjectMaterialPartColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, (decimal)Cost);

                case ProjectMaterialPartColumns.ExtendedCost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                    }, (decimal)ExtendedCost);
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
                        Quantity = decimalCellProps.Value.Value.ToDouble();
                        CalculateRow();
                    }
                    break;
                case ProjectMaterialPartColumns.Cost:
                    if (value is DataEntryGridDecimalCellProps costDecimalCellProps)
                    {
                        Cost = costDecimalCellProps.Value.Value.ToDouble();
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

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                var childRow =
                    Manager.ConstructRowFromLineType((MaterialPartLineTypes)child.LineType);
                AddChildRow(childRow);
                childRow.LoadChildren(child);
                Manager.Grid?.UpdateRow(childRow);
            }

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            if (!MaterialPartAutoFillValue.IsValid())
            {
                Manager.ViewModel.View.GotoGrid();
                var message = "Invalid Task Dependency";
                ControlsGlobals.UserInterface.ShowMessageBox(message, message, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, ProjectMaterialPartManager.MaterialPartColumnId);
                return false;
            }
            return true;
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
