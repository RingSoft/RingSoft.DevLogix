using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingTemplateItemRow : DbMaintenanceDataEntryGridRow<TestingTemplateItem>
    {
        public new TestingTemplateItemManager Manager { get; }

        public string Description { get; private set; }

        public TestingTemplateItemRow(TestingTemplateItemManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId, Description)
            {
                MaxLength = 50
            };
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            if (value is DataEntryGridTextCellProps textCellProps)
            {
                Description = textCellProps.Text;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(TestingTemplateItem entity)
        {
            Description = entity.Description;
        }

        public override void SaveToEntity(TestingTemplateItem entity, int rowIndex)
        {
            entity.Description = Description;
        }
    }
}
