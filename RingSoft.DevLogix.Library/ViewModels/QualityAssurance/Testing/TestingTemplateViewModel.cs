using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingTemplateViewModel : DevLogixDbMaintenanceViewModel<TestingTemplate>
    {
        public override TableDefinition<TestingTemplate> TableDefinition => AppGlobals.LookupContext.TestingTemplates;

        protected override TestingTemplate PopulatePrimaryKeyControls(TestingTemplate newEntity, PrimaryKeyValue primaryKeyValue)
        {
            return new TestingTemplate();
        }

        protected override void LoadFromEntity(TestingTemplate entity)
        {
            
        }

        protected override TestingTemplate GetEntityData()
        {
            return new TestingTemplate();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(TestingTemplate entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
