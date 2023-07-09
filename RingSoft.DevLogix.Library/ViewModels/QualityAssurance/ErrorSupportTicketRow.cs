using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorSupportTicketRow : DbMaintenanceDataEntryGridRow<SupportTicketError>
    {
        public new ErrorSupportTicketManager Manager { get; }

        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public ErrorSupportTicketRow(ErrorSupportTicketManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupportTicketId))
            {
                AllowLookupAdd = AppGlobals.LookupContext.SupportTicket.HasRight(RightTypes.AllowAdd),
                AllowLookupView = AppGlobals.LookupContext.SupportTicket.HasRight(RightTypes.AllowView)
            };
            AutoFillSetup.LookupDefinition.InitialOrderByField
                = AppGlobals.LookupContext.SupportTicket.GetFieldDefinition(p => p.Id);
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

        public override void LoadFromEntity(SupportTicketError entity)
        {
            AutoFillValue = entity.SupportTicket.GetAutoFillValue();
        }

        public override void SaveToEntity(SupportTicketError entity, int rowIndex)
        {
            entity.SupportTicketId = AutoFillValue.GetEntity<SupportTicket>().Id;

        }
    }
}
