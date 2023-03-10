using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTotalsRow : DataEntryGridRow
    {
        public new ProjectTotalsManager Manager { get; private set; }

        public string RowTitle { get; set; }

        public decimal Minutes { get; set; }

        public decimal Cost { get; set; }

        public int NegativeDisplayStyleId { get; set; }

        public int PositiveDisplayStyleId { get; set; }

        public ProjectTotalsRow(ProjectTotalsManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (TotalsColumns)columnId;

            switch (column)
            {
                case TotalsColumns.Type:
                    return new DataEntryGridTextCellProps(this, columnId, RowTitle);
                case TotalsColumns.Time:
                    return new TimeCostCellProps(this, columnId, Minutes);
                case TotalsColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency
                    }, Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var displayStyleId = 0;
            if (NegativeDisplayStyleId > 0)
            {
                displayStyleId = NegativeDisplayStyleId;
            }

            if (PositiveDisplayStyleId > 0)
            {
                displayStyleId = PositiveDisplayStyleId;
            }
            return new DataEntryGridCellStyle
            {
                State = DataEntryGridCellStates.ReadOnly,
                DisplayStyleId = displayStyleId,
            };
        }
    }
}
