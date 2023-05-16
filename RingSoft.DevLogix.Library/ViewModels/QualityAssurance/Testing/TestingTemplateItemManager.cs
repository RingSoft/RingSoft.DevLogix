using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingTemplateItemManager : DbMaintenanceDataEntryGridManager<TestingTemplateItem>
    {
        public new TestingTemplateViewModel ViewModel { get; }
        public TestingTemplateItemManager(TestingTemplateViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new TestingTemplateItemRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<TestingTemplateItem> ConstructNewRowFromEntity(TestingTemplateItem entity)
        {
            return new TestingTemplateItemRow(this);
        }
    }
}
