using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingOutlineTemplatesGridManager : DbMaintenanceDataEntryGridManager<TestingOutlineTemplate>
    {
        public new TestingOutlineViewModel ViewModel { get; }

        public TestingOutlineTemplatesGridManager(TestingOutlineViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new TestingOutlineTemplatesGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<TestingOutlineTemplate> ConstructNewRowFromEntity(TestingOutlineTemplate entity)
        {
            return new TestingOutlineTemplatesGridRow(this);
        }
    }
}
