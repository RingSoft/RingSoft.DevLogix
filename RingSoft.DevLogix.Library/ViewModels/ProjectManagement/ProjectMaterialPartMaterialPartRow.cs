using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    internal class ProjectMaterialPartMaterialPartRow : ProjectMaterialPartRow
    {
        public ProjectMaterialPartMaterialPartRow(ProjectMaterialPartManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override void LoadFromEntity(ProjectMaterialPart entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectMaterialPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
