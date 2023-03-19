using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectTaskDependencyManager : DbMaintenanceDataEntryGridManager<ProjectTaskDependency>
    {
        public new ProjectTaskViewModel ViewModel { get; private set; }

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

            foreach (var dependencyTask in dependency.Dependencies)
            {
                if (de.IndexOf(advancedFindFilter.SearchForAdvancedFindId.Value) != -1)
                {
                    var message = "Advanced find circular reference found. Aborting query.";
                    var caption = "Circular Reference Found";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                if (!ValidateAdvancedFind(advancedFindList, advancedFindFilter.SearchForAdvancedFindId.Value))
                    return false;
            }

            return true;
        }

    }
}
