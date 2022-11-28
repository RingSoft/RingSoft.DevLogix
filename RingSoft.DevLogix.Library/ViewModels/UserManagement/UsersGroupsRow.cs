using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DbMaintenance;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UsersGroupsRow : DbMaintenanceDataEntryGridRow<UsersGroup>
    {
        public new UsersGroupsManager Manager { get; set; }

        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public UsersGroupsRow(UsersGroupsManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.GroupLookup)
            {
                AllowLookupAdd = AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowAdd),
                AllowLookupView = AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowView)
            };
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                AutoFillValue = autoFillCellProps.AutoFillValue;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(UsersGroup entity)
        {
            AutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Groups,
                    entity.GroupId.ToString());
        }

        public override bool ValidateRow()
        {
            if (!AutoFillValue.IsValid())
            {
                Manager.ViewModel.View.OnValGridFail();

                var message = "Group contains an invalid value.";
                var caption = "Validation Fail";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, 0);
                return false;
            }
            return true;
        }

        public override void SaveToEntity(UsersGroup entity, int rowIndex)
        {
            entity.GroupId = AppGlobals.LookupContext.Groups.GetEntityFromPrimaryKeyValue(AutoFillValue.PrimaryKeyValue)
                .Id;
            entity.UserId = Manager.ViewModel.Id;

        }

    }
}
