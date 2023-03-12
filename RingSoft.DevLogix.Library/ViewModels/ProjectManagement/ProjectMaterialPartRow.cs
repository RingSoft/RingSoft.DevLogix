using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public abstract class ProjectMaterialPartRow : DbMaintenanceDataEntryGridRow<ProjectMaterialPart>
    {
        public new ProjectMaterialPartManager Manager { get; private set; }

        protected ProjectMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
            Manager = manager;
        }
    }
}
