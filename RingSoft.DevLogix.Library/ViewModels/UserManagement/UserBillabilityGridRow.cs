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
            TimeSpent = AppGlobals.MakeTimeSpent(minutesSpent);
            Billability = Math.Round(billability, 4);
        }
    }
}
