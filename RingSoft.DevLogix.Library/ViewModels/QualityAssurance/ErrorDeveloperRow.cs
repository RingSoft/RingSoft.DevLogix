using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorDeveloperRow : DbMaintenanceDataEntryGridRow<ErrorDeveloper>
    {
        public new ErrorDeveloperManager Manager { get; private set; }

        public AutoFillSetup DeveloperAutoFillSetup { get; private set; }

        public AutoFillValue DeveloperAutoFillValue { get; private set; }

        public DateTime FixedDate { get; private set; }

        public new int RowId { get; private set; }

        public ErrorDeveloperRow(ErrorDeveloperManager manager) : base(manager)
        {
            Manager = manager;
            DeveloperAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ErrorDeveloperGridColumns)columnId;

            switch (column)
            {
                case ErrorDeveloperGridColumns.Developer:
                    return new DataEntryGridAutoFillCellProps(this, columnId, DeveloperAutoFillSetup, DeveloperAutoFillValue);
                case ErrorDeveloperGridColumns.DateFixed:
                    return new DataEntryGridDateCellProps(this, columnId, new DateEditControlSetup
                    {
                        DateFormatType = DateFormatTypes.DateTime
                    }, FixedDate);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
        }

        public override void LoadFromEntity(ErrorDeveloper entity)
        {
            DeveloperAutoFillValue = entity.Developer.GetAutoFillValue();
            FixedDate = entity.DateFixed.ToLocalTime();
            RowId = entity.Id;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorDeveloper entity, int rowIndex)
        {
            entity.ErrorId = Manager.ViewModel.Id;
            entity.DeveloperId = DeveloperAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
            entity.DateFixed = FixedDate.ToUniversalTime();
            entity.Id = RowId;
        }

        public void SetDeveloperProperties()
        {
            DeveloperAutoFillValue = DeveloperAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            FixedDate = GblMethods.NowDate();
        }
    }
}
