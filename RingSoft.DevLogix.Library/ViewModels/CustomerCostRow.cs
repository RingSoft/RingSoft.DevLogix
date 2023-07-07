using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using System;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class CustomerCostRow : DbMaintenanceDataEntryGridRow<CustomerUser>
    {
        public new CustomerCostManager Manager { get; }

        public AutoFillSetup UserAutoFillSetup { get; private set; }

        public AutoFillValue UserAutoFillValue { get; private set; }

        public int UserId { get; private set; }

        public double MinutesSpent { get; private set; }

        public string TimeSpent { get; private set; }

        public double Cost { get; private set; }

        public CustomerCostRow(CustomerCostManager manager) : base(manager)
        {
            Manager = manager;
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);

        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (CustomerUserColumns)columnId;

            switch (column)
            {
                case CustomerUserColumns.User:
                    return new DataEntryGridAutoFillCellProps(this, columnId, UserAutoFillSetup, UserAutoFillValue);
                case CustomerUserColumns.TimeSpent:
                    return new DataEntryGridTextCellProps(this, columnId, TimeSpent);
                case CustomerUserColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                        Precision = 2,
                    }, (decimal)Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle()
            {
                State = DataEntryGridCellStates.Disabled
            };
        }

        public override void LoadFromEntity(CustomerUser entity)
        {
            UserId = entity.UserId;
            UserAutoFillValue = entity.User.GetAutoFillValue();
            MinutesSpent = entity.MinutesSpent;
            Cost = entity.Cost;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }


        public override void SaveToEntity(CustomerUser entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
