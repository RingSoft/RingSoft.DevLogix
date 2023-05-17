using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingOutlineViewModel : DevLogixDbMaintenanceViewModel<TestingOutline>
    {
        public override TableDefinition<TestingOutline> TableDefinition => AppGlobals.LookupContext.TestingOutlines;

        public RelayCommand GenerateDetailsCommand { get; private set; }

        public RelayCommand RetestCommand { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

        public TestingOutlineViewModel()
        {
            GenerateDetailsCommand = new RelayCommand(GenerateDetails);
            RetestCommand = new RelayCommand(Retest);
            PunchInCommand = new RelayCommand(PunchIn);
        }

        protected override TestingOutline PopulatePrimaryKeyControls(TestingOutline newEntity, PrimaryKeyValue primaryKeyValue)
        {
            return new TestingOutline();
        }

        protected override void LoadFromEntity(TestingOutline entity)
        {
            
        }

        protected override TestingOutline GetEntityData()
        {
            return new TestingOutline();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(TestingOutline entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        private void GenerateDetails()
        {

        }

        private void Retest()
        {

        }

        private void PunchIn()
        {

        }
    }
}
