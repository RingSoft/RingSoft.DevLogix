using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskLaborPartLaborPartRow : ProjectTaskLaborPartRow
    {
        public ProjectTaskLaborPartLaborPartRow(ProjectTaskLaborPartsManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ProjectTaskLaborPartColumns)columnId;
            switch (column)
            {
                case ProjectTaskLaborPartColumns.LaborPart:
                    return new DataEntryGridTextCellProps(this, columnId);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle
            {
                DisplayStyleId = ProjectTaskLaborPartsManager.LaborPartRowDisplayStyleId
            };
            return base.GetCellStyle(columnId);
        }

        public override void LoadFromEntity(ProjectTaskLaborPart entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectTaskLaborPart entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
