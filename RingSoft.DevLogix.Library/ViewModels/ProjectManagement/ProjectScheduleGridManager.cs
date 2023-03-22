using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleGridManager : DataEntryGridManager
    {
        public ProjectScheduleViewModel ViewModel { get; private set; }

        public ProjectScheduleGridManager(ProjectScheduleViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }
    }
}
