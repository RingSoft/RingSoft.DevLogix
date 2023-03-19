using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskDependencyRow : DbMaintenanceDataEntryGridRow<ProjectTaskDependency>
    {
        public new ProjectTaskDependencyManager Manager { get; set; }

        public int DependencyTaskId { get; private set; }

        public AutoFillSetup DependencyAutoFillSetup { get; private set; }
        public AutoFillValue DependencyAutoFillValue { get; private set; }

        public ProjectTaskDependencyRow(ProjectTaskDependencyManager manager) : base(manager)
        {
            Manager = manager;
            DependencyAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProjectTaskLookup);
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridAutoFillCellProps(this, columnId, DependencyAutoFillSetup, DependencyAutoFillValue);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
            {
                var dependencyId = autoFillCellProps.AutoFillValue.GetEntity<ProjectTask>().Id;
                if (dependencyId > 0 && dependencyId != DependencyTaskId)
                {
                    var existRow = GetDependencyRowForTask(dependencyId);
                    if (existRow != null)
                    {
                        var message =
                            "The project task you have selected already exists in the grid. Do you wish to go to that row?";
                        var caption = "Entry Validation";
                        var result = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                        if (result == MessageBoxButtonsResult.Yes)
                        {
                            Manager.Grid?.GotoCell(existRow, 1);
                            Manager.RemoveRow(this);
                        }

                        value.OverrideCellMovement = true;
                    }
                    else
                    {
                        DependencyAutoFillValue = autoFillCellProps.AutoFillValue;
                        DependencyTaskId = dependencyId;
                    }
                }
                else
                {
                    DependencyAutoFillValue = autoFillCellProps.AutoFillValue;
                    DependencyTaskId = dependencyId;
                }
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(ProjectTaskDependency entity)
        {
            DependencyAutoFillValue = entity.DependsOnProjectTask.GetAutoFillValue<ProjectTask>();
            DependencyTaskId = entity.DependsOnProjectTask.Id;
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(ProjectTaskDependency entity, int rowIndex)
        {
            entity.DependsOnProjectTaskId = DependencyTaskId;
        }

        public ProjectTaskDependencyRow GetDependencyRowForTask(int taskId)
        {
            var rows = Manager.Rows.OfType<ProjectTaskDependencyRow>();
            return rows.FirstOrDefault(p => p.DependencyTaskId == taskId);
        }
    }
}
