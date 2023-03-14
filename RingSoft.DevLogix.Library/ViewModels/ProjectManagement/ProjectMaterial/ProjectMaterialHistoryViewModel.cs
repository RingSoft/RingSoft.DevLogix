using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialHistoryViewModel : DevLogixDbMaintenanceViewModel<ProjectMaterialHistory>
    {
        public override TableDefinition<ProjectMaterialHistory> TableDefinition =>
            AppGlobals.LookupContext.ProjectMaterialHistory;

        protected override ProjectMaterialHistory PopulatePrimaryKeyControls(ProjectMaterialHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var table = AppGlobals.DataRepository.GetDataContext().GetTable<ProjectMaterialHistory>();
            var history = table
                .Include(p => p.User)
                .Include(p => p.ProjectMaterial)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            return history;
        }

        protected override void LoadFromEntity(ProjectMaterialHistory entity)
        {
            throw new System.NotImplementedException();
        }

        protected override ProjectMaterialHistory GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            throw new System.NotImplementedException();
        }

        protected override bool SaveEntity(ProjectMaterialHistory entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
