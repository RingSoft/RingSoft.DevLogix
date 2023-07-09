using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Text.RegularExpressions;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class SupportTicketErrorRow : DbMaintenanceDataEntryGridRow<SupportTicketError>
    {
        public new SupportTicketErrorManager Manager { get; }

        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public int ErrorId { get; set; }

        public SupportTicketErrorRow(SupportTicketErrorManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorId))
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
                ErrorId = AutoFillValue.GetEntity<Error>().Id;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(SupportTicketError entity)
        {
            ErrorId = entity.ErrorId;
            AutoFillValue = entity.Error.GetAutoFillValue();
        }

        public override void SaveToEntity(SupportTicketError entity, int rowIndex)
        {
            entity.ErrorId = AutoFillValue.GetEntity<Error>().Id;

        }
    }
}
