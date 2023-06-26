using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingOutlineTemplatesGridRow : DbMaintenanceDataEntryGridRow<TestingOutlineTemplate>
    {
        public new TestingOutlineTemplatesGridManager Manager { get; }

        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; set; }

        public TestingOutlineTemplatesGridRow(TestingOutlineTemplatesGridManager manager) : base(manager)
        {
            Manager = manager;
            AutoFillSetup = new AutoFillSetup(TableDefinition
                .GetFieldDefinition(p => p.TestingTemplateId));
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

        public override void LoadFromEntity(TestingOutlineTemplate entity)
        {
            AutoFillValue = entity.TestingTemplate.GetAutoFillValue();
        }

        public override void SaveToEntity(TestingOutlineTemplate entity, int rowIndex)
        {
            entity.TestingTemplateId = AutoFillValue.GetEntity<TestingTemplate>().Id;
        }
    }
}
