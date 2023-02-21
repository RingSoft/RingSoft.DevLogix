using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Project>
    {
        public override TableDefinition<Project> TableDefinition => AppGlobals.LookupContext.Projects;

        protected override Project PopulatePrimaryKeyControls(Project newEntity, PrimaryKeyValue primaryKeyValue)
        {
            return new Project();
        }

        protected override void LoadFromEntity(Project entity)
        {
            
        }

        protected override Project GetEntityData()
        {
            return new Project();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(Project entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
