using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorQaRow : DbMaintenanceDataEntryGridRow<ErrorQa>
    {
        public new ErrorQaManager Manager { get; private set; }

        public AutoFillSetup TesterAutoFillSetup { get; private set; }

        public AutoFillValue TesterAutoFillValue { get; private set; }

        public DateTime DateChanged { get; private set; }

        public AutoFillSetup NewStatusAutoFillSetup { get; private set; }

        public AutoFillValue NewStatusAutoFillValue { get; private set; }

        public new int RowId { get; private set; }

        public ErrorQaRow(ErrorQaManager manager) : base(manager)
        {
            Manager = manager;
            TesterAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.Users.LookupDefinition);
            NewStatusAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ErrorStatusLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ErrorQaColumns)columnId;
            switch (column)
            {
                case ErrorQaColumns.Tester:
                    return new DataEntryGridAutoFillCellProps(this, columnId, TesterAutoFillSetup, TesterAutoFillValue);
                case ErrorQaColumns.NewStatus:
                    return new DataEntryGridAutoFillCellProps(this, columnId, NewStatusAutoFillSetup,
                        NewStatusAutoFillValue);
                case ErrorQaColumns.DateChanged:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup() { DateFormatType = DateFormatTypes.DateTime }, DateChanged);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
        }

        public override void LoadFromEntity(ErrorQa entity)
        {
            TesterAutoFillValue = entity.Tester.GetAutoFillValue();
            NewStatusAutoFillValue = entity.NewErrorStatus.GetAutoFillValue();

            DateChanged = entity.DateChanged.ToLocalTime();
            RowId = entity.Id;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorQa entity, int rowIndex)
        {
            entity.ErrorId = Manager.ViewModel.Id;
            entity.TesterId = TesterAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
            entity.NewStatusId = NewStatusAutoFillValue.GetEntity(AppGlobals.LookupContext.ErrorStatuses).Id;
            entity.DateChanged = DateChanged.ToUniversalTime();
            entity.Id = RowId;
        }

        public void SetTesterProperties(int newStatusValue)
        {
            TesterAutoFillValue = TesterAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            NewStatusAutoFillValue = NewStatusAutoFillSetup.GetAutoFillValueForIdValue(newStatusValue);
            DateChanged = GblMethods.NowDate();
        }
    }
}
