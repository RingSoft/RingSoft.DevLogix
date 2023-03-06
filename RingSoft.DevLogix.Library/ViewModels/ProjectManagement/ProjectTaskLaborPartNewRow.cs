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
                            Manager.ViewModel.View.GetNewLineType(autoFillCellProps.AutoFillValue.Text,
                                out var laborPartPkValue, out var lineType);
                            switch (lineType)
                            {
                                case LaborPartLineTypes.NewRow:
                                    autoFillCellProps.OverrideCellMovement = true;
                                    break;
                                case LaborPartLineTypes.LaborPart:
                                    var laborPart =
                                        AppGlobals.LookupContext.LaborParts.GetEntityFromPrimaryKeyValue(
                                            laborPartPkValue);

                                    if (laborPart != null)
                                    {
                                        var context = AppGlobals.DataRepository.GetDataContext();
                                        laborPart = context.GetTable<LaborPart>()
                                            .FirstOrDefault(p => p.Id == laborPart.Id);
                                        var autoFillValue = laborPart.GetAutoFillValue();
                                        var newRow = new ProjectTaskLaborPartLaborPartRow(Manager);
                                        newRow.SetAutoFillValue(autoFillValue);
                                        Manager.ReplaceRow(this, newRow);
                                    }
                                    break;
                                case LaborPartLineTypes.Miscellaneous:
                                    var newMiscRow = new ProjectTaskLaborPartMiscRow(Manager);
                                    newMiscRow.SetDescription(autoFillCellProps.AutoFillValue.Text);
                                    Manager.ReplaceRow(this, newMiscRow);
                                    break;
                                case LaborPartLineTypes.Comment:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            var newRow = new ProjectTaskLaborPartLaborPartRow(Manager);
                            newRow.SetAutoFillValue(autoFillCellProps.AutoFillValue);
                            Manager.ReplaceRow(this, newRow);
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
