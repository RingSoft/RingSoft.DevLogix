using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectUserColumns
    {
        User = 0,
        MinutesSpent = 1,
        Cost = 2,
    }

    public class ProjectUsersGridManager : DbMaintenanceDataEntryGridManager<ProjectUser>
    {
        public const int UserColumnId = (int)ProjectUserColumns.User;
        public const int MinutesSpentColumnId = (int)ProjectUserColumns.MinutesSpent;
        public const int CostColumnId = (int)ProjectUserColumns.Cost;

        public new ProjectMaintenanceViewModel ViewModel { get; private set; }

        public ProjectUsersGridManager(ProjectMaintenanceViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ProjectUsersGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<ProjectUser> ConstructNewRowFromEntity(ProjectUser entity)
        {
            return new ProjectUsersGridRow(this);
        }

        public bool ProcessPunchIn()
        {
            var result = true;

            var userRows = Rows.OfType<ProjectUsersGridRow>();
            if (!userRows.Any(p => p.UserId == AppGlobals.LoggedInUser.Id))
            {
                if (!ViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                {
                    var message =
                        "You're not listed as a user to this project and you do not have the right to edit projects. Please contact someone who has the right to edit projects to add you as a user to this project in order to punch in.";
                    var caption = "Punch In Validation";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                var userRow = new ProjectUsersGridRow(this);
                userRow.SetUser(AppGlobals.LoggedInUser.Id);
                AddRow(userRow, Rows.Count - 1);
                Grid?.RefreshGridView();
                ViewModel.DoSave();
            }

            return result;
        }
    }
}
