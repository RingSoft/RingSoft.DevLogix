using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartNewRow : ProjectTaskLaborPartRow
    {
        public AutoFillSetup LaborPartAutoFillSetup { get; private set; }

        public AutoFillValue LaborPartAutoFillValue { get; private set; }

        public ProjectTaskLaborPartNewRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            LaborPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.LaborPartLookup);
        }

        public override LaborPartLineTypes LaborPartLineType => LaborPartLineTypes.NewRow;
        public override double GetExtendedMinutesCost()
        {
            return 0;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;

            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    return new DataEntryGridAutoFillCellProps(this, columnId, LaborPartAutoFillSetup,
                        LaborPartAutoFillValue);
                case ProjectTaskLaborPartColumns.MinutesCost:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
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
                case ProjectTaskLaborPartColumns.Complete:
                    return new DataEntryGridControlCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                        IsVisible = false,
                    };
                default:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled,
                    };
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ProjectTaskLaborPartColumns)value.ColumnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        if (!autoFillCellProps.AutoFillValue.IsValid())
                        {
                            var item = string.Empty;
                            if (autoFillCellProps.AutoFillValue != null)
                            {
                                item = autoFillCellProps.AutoFillValue.Text;
                            }

                            Manager.ViewModel.View.GetNewLineType(item,
                                out var laborPartPkValue, out var lineType);
                            var newRow = Manager.ConstructRowFromLineType(lineType);
                            Manager.ReplaceRow(this, newRow);
                            switch (lineType)
                            {
                                case LaborPartLineTypes.NewRow:
                                    autoFillCellProps.OverrideCellMovement = true;
                                    break;
                                case LaborPartLineTypes.LaborPart:
                                    var laborPart =
                                        AppGlobals.LookupContext.LaborParts.GetEntityFromPrimaryKeyValue(
                                            laborPartPkValue);

                                    if (laborPart != null && newRow is ProjectTaskLaborPartLaborPartRow laborPartRow)
                                    {
                                        var context = AppGlobals.DataRepository.GetDataContext();
                                        laborPart = context.GetTable<LaborPart>()
                                            .FirstOrDefault(p => p.Id == laborPart.Id);
                                        var autoFillValue = laborPart.GetAutoFillValue();
                                        laborPartRow.SetAutoFillValue(autoFillValue);
                                    }
                                    break;
                                case LaborPartLineTypes.Miscellaneous:
                                    if (newRow is ProjectTaskLaborPartMiscRow newMiscRow)
                                    {
                                        newMiscRow.SetDescription(autoFillCellProps.AutoFillValue.Text);
                                    }
                                    break;
                                case LaborPartLineTypes.Comment:
                                    if (newRow is ProjectTaskLaborPartCommentRow commentRow)
                                    {
                                        commentRow.SetComment(autoFillCellProps.AutoFillValue.Text);
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            Manager.Grid?.UpdateRow(newRow);
                        }
                        else
                        {
                            var newRow = Manager.ConstructRowFromLineType(LaborPartLineTypes.LaborPart);
                            Manager.ReplaceRow(this, newRow);
                            if (newRow is ProjectTaskLaborPartLaborPartRow newLaborPartRow)
                            {
                                newLaborPartRow.SetAutoFillValue(autoFillCellProps.AutoFillValue);
                                Manager.Grid?.UpdateRow(newLaborPartRow);
                            }
                        }
                    }

                    break;
            }
            base.SetCellValue(value);
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
