using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTotalsRow : DataEntryGridRow
    {
        public new ProjectTotalsManager Manager { get; private set; }

        public string RowTitle { get; set; }

        private double _minutes;

        public double Minutes {
            get => _minutes;
            set
            {
                _minutes = Math.Round(value, 2);
            }
        }

        private double _cost;

        public double Cost
        {
            get => _cost;
            set
            {
                _cost = Math.Round(value, 2);
            }
    }

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
                    }, (decimal)Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var displayStyleId = 0;
            var column = (TotalsColumns)columnId;
            switch (column)
            {
                case TotalsColumns.Type:
                    break;
                case TotalsColumns.Time:
                    displayStyleId = GetDisplayStyleId(Minutes);
                    break;
                case TotalsColumns.Cost:
                    displayStyleId = GetDisplayStyleId(Cost);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridCellStyle
            {
                State = DataEntryGridCellStates.ReadOnly,
                DisplayStyleId = displayStyleId,
            };
        }

        public int GetDisplayStyleId(double value)
        {
            var result = 0;

            if (value < 0)
            {
                if (NegativeDisplayStyleId > 0)
                {
                    result = NegativeDisplayStyleId;
                }
            }
            else if (value > 0)
            {
                if (PositiveDisplayStyleId > 0)
                {
                    result = PositiveDisplayStyleId;
                }
            }

            return result;
        }

        public void ClearData()
        {
            Minutes = 0;
            Cost = 0;
        }
    }
}
