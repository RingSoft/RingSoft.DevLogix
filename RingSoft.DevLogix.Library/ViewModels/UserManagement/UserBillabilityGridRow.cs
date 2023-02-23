using System;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserBillabilityGridRow : DataEntryGridRow
    {
        public new UserBillabilityGridManager Manager { get; private set; }

        public UserBillabilityRows RowType { get; private set; }

        public string Name { get; private set; }

        public decimal MinutesSpent { get; private set; }
        public string TimeSpent { get; private set; }

        public decimal Billability { get; private set; }

        public UserBillabilityGridRow(UserBillabilityGridManager manager, UserBillabilityRows rowType) : base(manager)
        {
            Manager = manager;
            RowType = rowType;
            switch (rowType)
            {
                case UserBillabilityRows.BillableProjects:
                    Name = "Billable Projects";
                    break;
                case UserBillabilityRows.NonBillableProjects:
                    Name = "Non-Billable Projects";
                    break;
                case UserBillabilityRows.Errors:
                    Name = "Errors";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rowType), rowType, null);
            }
            TimeSpent = "0 Minutes";
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (UserBillabilityColumns)columnId;
            switch (column)
            {
                case UserBillabilityColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, Name);
                case UserBillabilityColumns.TimeSpent:
                    return new DataEntryGridTextCellProps(this, columnId, TimeSpent);
                case UserBillabilityColumns.Billability:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup()
                    {
                        FormatType = DecimalEditFormatTypes.Percent,
                    }, Billability);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId, string.Empty);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle
            {
                State = DataEntryGridCellStates.ReadOnly,
            };
        }

        public void SetRowValues(decimal minutesSpent, decimal billability)
        {
            MinutesSpent = minutesSpent;
            var timeSpent = "0 Minutes";
            var hoursSpent = (decimal)0;
            var daysSpent = (decimal)0;
            var numFormatString = GblMethods.GetNumFormat(2, false);

            if (minutesSpent >= 0)
            {
                if (minutesSpent > 60)
                {
                    hoursSpent = Math.Round(minutesSpent / 60, 2);
                    TimeSpent = $"{FormatValue(hoursSpent, DecimalFieldTypes.Decimal)} Hours";
                }
                else
                {
                    minutesSpent = Math.Round(minutesSpent, 2);
                    TimeSpent = $"{FormatValue(minutesSpent, DecimalFieldTypes.Decimal)} Minutes";
                }
            }

            if (hoursSpent > 24)
            {
                daysSpent = Math.Round(hoursSpent / 24, 2);
                TimeSpent = $"{FormatValue(daysSpent, DecimalFieldTypes.Decimal)} Days";
            }
            Billability = Math.Round(billability, 4);
        }

        private string FormatValue(decimal value, DecimalFieldTypes decimalFieldType)
        {
            var result = string.Empty;
            var numFormat = GblMethods.GetNumFormat(2, false);
            result = GblMethods.FormatValue(FieldDataTypes.Decimal, value.ToString()
                , numFormat);

            return result;
        }
    }
}
