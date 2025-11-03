using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskDependencyManager : DbMaintenanceDataEntryGridManager<ProjectTaskDependency>
    {
        public new ProjectTaskViewModel ViewModel { get; private set; }

        public int ProjectFilter { get; set; }

        public ProjectTaskDependencyManager(ProjectTaskViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectTaskDependencyRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectTaskDependency> ConstructNewRowFromEntity(ProjectTaskDependency entity)
        {
            return new ProjectTaskDependencyRow(this);
        }

        public bool ValidateCircular()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectTask>();
            var rows = Rows.OfType<ProjectTaskDependencyRow>().ToList();
            if (rows.Any())
            {
                foreach (var projectTaskDependencyRow in rows)
                {
                    var dependencies = new List<int>();
                    dependencies.Add(ViewModel.Id);
                    var dependencyId = projectTaskDependencyRow.DependencyTaskId;
                    if (!ValidateCircular(dependencies, dependencyId, table))
                        return false;
                }
            }

            return true;
        }
        private bool ValidateCircular(List<int> circularList, int dependencyId, IQueryable<ProjectTask> table)
        {
            if (dependencyId <= 0)
                return true;

            circularList.Add(dependencyId);
            var dependency = table
                .Include(p => p.SourceDependencies)
                .FirstOrDefault(p => p.Id == dependencyId);

            if (dependency != null)
                foreach (var dependencyTask in dependency.SourceDependencies)
                {
                    if (circularList.IndexOf(dependencyTask.DependsOnProjectTaskId) != -1)
                    {
                        var message = "Task Dependency circular reference found.";
                        var caption = "Circular Reference Found";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return false;
                    }

                    if (!ValidateCircular(circularList, dependencyTask.DependsOnProjectTaskId, table))
                        return false;
                }

            return true;
        }

        public async Task<bool> ValidateProjectChange()
        {
            var rowsExist = Rows.Any(p => !p.IsNew);
            if (rowsExist)
            {
                var message =
                    "Changing the project will cause the dependencies grid to be cleared. Are you sure this is what you want to do?";
                var caption = "Change Validation";
                var msgResult = await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                if (msgResult == MessageBoxButtonsResult.Yes)
                {
                    ClearRows();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ClearRows();
            }
            return true;
        }
    }
}
