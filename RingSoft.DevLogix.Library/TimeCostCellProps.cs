﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library
{
    public class TimeCostCellProps : DataEntryGridEditingCellProps
    {
        public decimal Minutes { get; set; }

        public const int MinutesControlId = 100;

        public override int EditingControlId => MinutesControlId;

        public TimeCostCellProps(DataEntryGridRow row, int columnId, decimal minutes)
            : base(row, columnId)
        {
            Minutes = minutes;
            
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return AppGlobals.MakeTimeSpent(Minutes);
        }
    }
}