using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartLaborPartRow : ProjectTaskLaborPartRow
    {
        public override LaborPartLineTypes LaborPartLineType => LaborPartLineTypes.LaborPart;
        public override decimal GetExtendedMinutesCost()
        {
            return MinutesCost * Quantity;
        }

        public AutoFillSetup LaborPartAutoFillSetup { get; private set; }

        public AutoFillValue LaborPartAutoFillValue { get; private set; }

        public decimal MinutesCost { get; private set; }

        public decimal Quantity { get; private set; } = 1;

        public decimal ExtendedMinutesCost { get; private set; }

        public ProjectTaskLaborPartLaborPartRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
            LaborPartAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.LaborPartLookup);
        }

        public void SetAutoFillValue(AutoFillValue autoFillValue)
        {
            LaborPartAutoFillValue = autoFillValue;
            var laborPart = LaborPartAutoFillValue.GetEntity<LaborPart>();
            var context = AppGlobals.DataRepository.GetDataContext();
            laborPart = context.GetTable<LaborPart>()
                .FirstOrDefault(p => p.Id == laborPart.Id);
            if (laborPart != null)
            {
                SetMinutesCost(laborPart);

                if (!laborPart.Comment.IsNullOrEmpty())
                {
                    var commentRow = new ProjectTaskLaborPartCommentRow(Manager);
                    AddChildRow(commentRow);
                    commentRow.SetValue(laborPart.Comment);
                }
            }
        }


        private void SetMinutesCost(LaborPart laborPart)
        {
            if (LaborPartAutoFillValue.IsValid())
            {
                if (laborPart != null)
                {
                    MinutesCost = laborPart.MinutesCost;
                    CalculateRow();
                }
            }
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
                    return new TimeCostCellProps(this, columnId, MinutesCost);
                case ProjectTaskLaborPartColumns.Quantity:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup()
                    {
                        AllowNullValue = false,
                        MinimumValue = 1,
                    }, Quantity);
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                    return new TimeCostCellProps(this, columnId, ExtendedMinutesCost);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            var cellStyle = new DataEntryGridCellStyle();
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    cellStyle.ColumnHeader = "Labor Part";
                    switch (Manager.DisplayMode)
                    {
                        case DisplayModes.All:
                            cellStyle.State = DataEntryGridCellStates.Enabled;
                            break;
                        case DisplayModes.User:
                        case DisplayModes.Disabled:
                            cellStyle.State = DataEntryGridCellStates.Disabled;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return cellStyle;

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
                        SetAutoFillValue(autoFillCellProps.AutoFillValue);
                    }
                    break;
                case ProjectTaskLaborPartColumns.Quantity:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Quantity = decimalCellProps.Value.Value;
                        CalculateRow();
                    }
                    break;
                case ProjectTaskLaborPartColumns.MinutesCost:
                    if (value is TimeCostCellProps timeCostCellProps)
                    {
                        MinutesCost = timeCostCellProps.Minutes;
                        CalculateRow();
                    }
                    break;
                case ProjectTaskLaborPartColumns.ExtendedMinutes:
                case ProjectTaskLaborPartColumns.Complete:
                case ProjectTaskLaborPartColumns.PercentComplete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        private void CalculateRow()
        {
            ExtendedMinutesCost = Quantity * MinutesCost;
            Manager.CalculateTotalMinutesCost();
            Manager.CalcPercentComplete();
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            LaborPartAutoFillValue = entity.LaborPart.GetAutoFillValue();
            MinutesCost = entity.MinutesCost;
            Quantity = entity.Quantity.Value;
            ExtendedMinutesCost = GetExtendedMinutesCost();

            var children = GetDetailChildren(entity);
            foreach (var child in children)
            {
                var childRow =
                    Manager.ConstructRowFromLineType((LaborPartLineTypes)child.LineType);
                AddChildRow(childRow);
                childRow.LoadChildren(child);
                Manager.Grid?.UpdateRow(childRow);
            }
            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            if (!LaborPartAutoFillValue.IsValid())
            {
                Manager.ViewModel.View.SetFocusToGrid(ProjectTaskGrids.LaborPart);
                var message = "Invalid Labor Part";
                ControlsGlobals.UserInterface.ShowMessageBox(message, message, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, ProjectTaskLaborPartsManager.LaborPartColumnId);
                return false;
            }
            return true;
        }
        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            entity.LaborPartId = LaborPartAutoFillValue.GetEntity<LaborPart>().Id;
            entity.MinutesCost = MinutesCost;
            entity.Quantity = Quantity;

            base.SaveToEntity(entity, rowIndex);
        }
    }
}
