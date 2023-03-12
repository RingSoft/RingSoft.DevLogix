using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialPartNewRow : ProjectMaterialPartRow
    {
        public override MaterialPartLineTypes LineType => MaterialPartLineTypes.NewRow;

        public AutoFillSetup MaterialPartAutoFillSetup { get; private set; }

        public AutoFillValue MaterialPartAutoFillValue { get; private set; }

        public ProjectMaterialPartNewRow(ProjectMaterialPartManager manager) : base(manager)
        {
            MaterialPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.MaterialPartLookup);
        }

        public override decimal GetExtendedCost()
        {
            return 0;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    return new DataEntryGridAutoFillCellProps(this, columnId, MaterialPartAutoFillSetup,
                        MaterialPartAutoFillValue);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectMaterialPartColumns)columnId;
            switch (column)
            {
                case ProjectMaterialPartColumns.MaterialPart:
                    break;
                default:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
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
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        if (autoFillCellProps.AutoFillValue.IsValid())
                        {
                            var newRow = Manager.ConstructRowFromLineType(MaterialPartLineTypes.MaterialPart);
                            Manager.ReplaceRow(this, newRow);
                            if (newRow is ProjectMaterialPartMaterialPartRow newMaterialPartRow)
                            {
                                newMaterialPartRow.SetAutoFillValue(autoFillCellProps.AutoFillValue);
                                Manager.Grid?.UpdateRow(newRow);
                            }
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
