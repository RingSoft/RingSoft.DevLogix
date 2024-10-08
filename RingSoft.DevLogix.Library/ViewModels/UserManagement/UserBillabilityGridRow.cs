﻿using System;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserBillabilityGridRow : DataEntryGridRow
    {
        private double _minutesSpent;
        private double _billability;
        public new UserBillabilityGridManager Manager { get; private set; }

        public UserBillabilityRows RowType { get; private set; }

        public string Name { get; private set; }

        public double MinutesSpent
        {
            get => _minutesSpent;
            private set { _minutesSpent = Math.Round(value, 2); }
        }

        public string TimeSpent { get; private set; }

        public double Billability
        {
            get => _billability;
            private set => _billability = Math.Round(value, 4);
        }

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
                case UserBillabilityRows.TestingOutlines:
                    Name = "Testing Outlines";
                    break;
                case UserBillabilityRows.Customers:
                    Name = "Customers";
                    break;

                case UserBillabilityRows.Support:
                    Name = "Support Tickets";
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

        public void SetRowValues(double minutesSpent, double billability)
        {
            MinutesSpent = minutesSpent;
            TimeSpent = AppGlobals.MakeTimeSpent(minutesSpent);
            Billability = Math.Round(billability, 4);
        }
    }
}
