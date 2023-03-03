using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskViewModel : AppDbMaintenanceViewModel<ProjectTask>
    {
        public override TableDefinition<ProjectTask> TableDefinition => AppGlobals.LookupContext.ProjectTasks;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }


        protected override ProjectTask PopulatePrimaryKeyControls(ProjectTask newEntity, PrimaryKeyValue primaryKeyValue)
        {
            return new ProjectTask();
        }

        protected override void LoadFromEntity(ProjectTask entity)
        {
            
        }

        protected override ProjectTask GetEntityData()
        {
            return new ProjectTask();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(ProjectTask entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
