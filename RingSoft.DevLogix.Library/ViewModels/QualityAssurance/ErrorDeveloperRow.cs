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

        public ErrorDeveloperRow(ErrorDeveloperManager manager) : base(manager)
        {
            Manager = manager;
            DeveloperAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridAutoFillCellProps(this, columnId, DeveloperAutoFillSetup, DeveloperAutoFillValue);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                DeveloperAutoFillValue = autoFillCellProps.AutoFillValue;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ErrorDeveloper entity)
        {
            DeveloperAutoFillValue = DeveloperAutoFillSetup.GetAutoFillValueForIdValue(entity.DeveloperId);
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ErrorDeveloper entity, int rowIndex)
        {
            entity.ErrorId = Manager.ViewModel.Id;
            entity.DeveloperId = DeveloperAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id;
        }
    }
}
