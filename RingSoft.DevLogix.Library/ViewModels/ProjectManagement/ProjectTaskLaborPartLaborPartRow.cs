﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

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
            SetMinutesCost();
        }

        private void SetMinutesCost()
        {
            if (LaborPartAutoFillValue.IsValid())
            {
                var laborPart = LaborPartAutoFillValue.GetEntity<LaborPart>();
                if (laborPart != null)
                {
                    var context = AppGlobals.DataRepository.GetDataContext();
                    laborPart = context.GetTable<LaborPart>()
                        .FirstOrDefault(p => p.Id == laborPart.Id);
                }

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
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    return new DataEntryGridCellStyle
                    {
                        ColumnHeader = "Labor Part",
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
                        LaborPartAutoFillValue = autoFillCellProps.AutoFillValue;
                        SetMinutesCost();
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
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            LaborPartAutoFillValue = entity.LaborPart.GetAutoFillValue();
            MinutesCost = entity.MinutesCost;
            Quantity = entity.Quantity.Value;
            ExtendedMinutesCost = GetExtendedMinutesCost();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
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
