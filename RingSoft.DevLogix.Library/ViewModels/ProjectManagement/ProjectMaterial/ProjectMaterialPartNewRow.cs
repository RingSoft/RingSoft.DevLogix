using System;
using System.Linq;
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

        public override double GetExtendedCost()
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
                        else
                        {
                            var item = string.Empty;
                            if (autoFillCellProps.AutoFillValue!= null)
                            {
                                item = autoFillCellProps.AutoFillValue.Text;
                            }
                            Manager.ViewModel.View.GetNewLineType(item, 
                                out var materialPartPkValue, out var lineType);
                            var newRow = Manager.ConstructRowFromLineType(lineType);
                            Manager.ReplaceRow(this, newRow);
                            switch (lineType)
                            {
                                case MaterialPartLineTypes.NewRow:
                                    autoFillCellProps.OverrideCellMovement = true;
                                    break;
                                case MaterialPartLineTypes.MaterialPart:
                                    var materialPart =
                                        AppGlobals.LookupContext.MaterialParts.GetEntityFromPrimaryKeyValue(
                                            materialPartPkValue);

                                    if (materialPart != null && newRow is ProjectMaterialPartMaterialPartRow materialPartRow)
                                    {
                                        var context = AppGlobals.DataRepository.GetDataContext();
                                        materialPart = context.GetTable<MaterialPart>()
                                            .FirstOrDefault(p => p.Id == materialPart.Id);
                                        var autoFillValue = materialPart.GetAutoFillValue();
                                        materialPartRow.SetAutoFillValue(autoFillValue);
                                    }
                                    break;
                                case MaterialPartLineTypes.Overhead:
                                    if (newRow is ProjectMaterialPartOverheadRow newOverheadRow)
                                    {
                                        newOverheadRow.SetDescription(autoFillCellProps.AutoFillValue.Text);
                                    }
                                    break;

                                case MaterialPartLineTypes.Miscellaneous:
                                    if (newRow is ProjectMaterialPartMiscRow newMiscRow)
                                    {
                                        newMiscRow.SetDescription(autoFillCellProps.AutoFillValue.Text);
                                    }
                                    break;
                                case MaterialPartLineTypes.Comment:
                                    if (newRow is ProjectMaterialPartCommentRow commentRow)
                                    {
                                        commentRow.SetComment(autoFillCellProps.AutoFillValue.Text);
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            Manager.Grid?.UpdateRow(newRow);

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

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
