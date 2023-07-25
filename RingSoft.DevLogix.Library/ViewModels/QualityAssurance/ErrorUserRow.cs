using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorUserRow : DbMaintenanceDataEntryGridRow<ErrorUser>
    {
        public new ErrorUserGridManager Manager { get; private set; }

        public AutoFillSetup UserAutoFillSetup { get; private set; }

        public AutoFillValue UserAutoFillValue { get; private set; }

        public int UserId { get; private set; }

        public double MinutesSpent { get; private set; }

        public string TimeSpent { get; private set;  }

        public double Cost { get; private set; }

        public ErrorUserRow(ErrorUserGridManager manager) : base(manager)
        {
            Manager = manager;
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ErrorUserColumns)columnId;
            switch (column)
            {
                case ErrorUserColumns.User:
                    return new DataEntryGridAutoFillCellProps(this, columnId, UserAutoFillSetup, UserAutoFillValue);
                case ErrorUserColumns.TimeSpent:
                    return new DataEntryGridTextCellProps(this, columnId, TimeSpent);
                case ErrorUserColumns.Cost:
                    return new DataEntryGridDecimalCellProps(this, columnId, new DecimalEditControlSetup
                    {
                        FormatType = DecimalEditFormatTypes.Currency,
                        Precision = 2,
                    }, Cost);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle()
            {
                State = DataEntryGridCellStates.Disabled
            };
        }

        public override void LoadFromEntity(ErrorUser entity)
        {
            UserId = entity.UserId;
            UserAutoFillValue = entity.User.GetAutoFillValue();
            MinutesSpent = entity.MinutesSpent;
            Cost = entity.Cost;
            TimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorUser entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
