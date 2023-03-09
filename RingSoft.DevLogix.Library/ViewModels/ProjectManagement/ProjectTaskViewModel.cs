using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using MySqlX.XDevAPI.Common;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectTaskView : IDbMaintenanceView
    {
        void GetNewLineType(string text, out PrimaryKeyValue laborPartPkValue, out LaborPartLineTypes lineType);

        bool ShowCommentEditor(DataEntryGridMemoValue comment);

        void SetTaskReadOnlyMode(bool value);
    }
    public class ProjectTaskViewModel : DevLogixDbMaintenanceViewModel<ProjectTask>
    {
        public override TableDefinition<ProjectTask> TableDefinition => AppGlobals.LookupContext.ProjectTasks;

        public override bool SetReadOnlyMode => false;

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

        private AutoFillSetup _projectAutoFillSetup;

        public AutoFillSetup ProjectAutoFillSetup
        {
            get => _projectAutoFillSetup;
            set
            {
                if (_projectAutoFillSetup == value)
                {
                    return;
                }
                _projectAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectAutoFillValue;

        public AutoFillValue ProjectAutoFillValue
        {
            get => _projectAutoFillValue;
            set
            {
                if (_projectAutoFillValue == value)
                    return;

                _projectAutoFillValue = value;
                SetUserFilter();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }
                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                    return;

                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _minutesCost;

        public decimal MinutesCost
        {
            get => _minutesCost;
            set
            {
                if (_minutesCost == value)
                    return;

                _minutesCost = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMinutesCost;

        public decimal TotalMinutesCost
        {
            get => _totalMinutesCost;
            set
            {
                _totalMinutesCost = value;
                TotalMinutesCostText = AppGlobals.MakeTimeSpent(value);
            }
        }

        private decimal _hourlyRate;

        public decimal HourlyRate
        {
            get => _hourlyRate;
            set
            {
                if (_hourlyRate == value)
                    return;

                _hourlyRate = value;
                OnPropertyChanged();
            }
        }

        private string _totalMinutesCostText;

        public string TotalMinutesCostText
        {
            get => _totalMinutesCostText;
            set
            {
                if (_totalMinutesCostText == value)
                    return;

                _totalMinutesCostText = value;
                OnPropertyChanged(null, false);
            }
        }

        private decimal _percentComplete;

        public decimal PercentComplete
        {
            get => _percentComplete;
            set
            {
                if (_percentComplete == value)
                    return;

                _percentComplete = value;
                OnPropertyChanged();
            }
        }

        private ProjectTaskLaborPartsManager _laborPartsManager;

        public ProjectTaskLaborPartsManager LaborPartsManager
        {
            get => _laborPartsManager;
            set
            {
                if (_laborPartsManager == value)
                    return;

                _laborPartsManager = value;
                OnPropertyChanged();
            }
        }

        private string? _notes;

        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        public AutoFillValue DefaultProjectAutoFillValue { get; private set; }

        public new IProjectTaskView View { get; private set; }

        public ProjectTaskViewModel()
        {
            UserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.UserId))
            {
                AllowLookupAdd = false,
            };
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));

            LaborPartsManager = new ProjectTaskLaborPartsManager(this);
            TablesToDelete.Add(AppGlobals.LookupContext.ProjectTaskLaborParts);
        }

        protected override ProjectTask PopulatePrimaryKeyControls(ProjectTask newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.GetTable<ProjectTask>()
                .Include(p => p.User)
                .Include(p => p.Project)
                .Include(p => p.LaborParts)
                .ThenInclude(p => p.LaborPart)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = result.GetAutoFillValue();

            var hasEditRight = TableDefinition.HasRight(RightTypes.AllowEdit);
            if (!hasEditRight)
            {
                if (result.UserId == AppGlobals.LoggedInUser.Id)
                {
                    SaveButtonEnabled = true;
                    View.SetTaskReadOnlyMode(false);
                }
                else
                {
                    SaveButtonEnabled = false;
                    View.SetTaskReadOnlyMode(true);
                }
            }
            return result;

        }

        protected override void Initialize()
        {
            if (base.View is IProjectTaskView projectTaskView)
            {
                View = projectTaskView;
            }
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Projects)
                {
                    var project =
                        AppGlobals.LookupContext.Projects.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    DefaultProjectAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Projects,
                            project.Id.ToString());
                }
            }

            base.Initialize();
        }

        private void SetUserFilter()
        {
            UserAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            if (ProjectAutoFillValue.IsValid())
            {
                var project = ProjectAutoFillValue.GetEntity<Project>();
                if (project != null)
                {
                    var formula = string.Empty;
                    var selectQuery = new SelectQuery(AppGlobals.LookupContext.ProjectUsers.TableName);
                    selectQuery.AddSelectColumn(AppGlobals.LookupContext.ProjectUsers.GetFieldDefinition(p => p.UserId).FieldName);
                    selectQuery.AddWhereItem(AppGlobals.LookupContext.ProjectUsers.GetFieldDefinition(p => p.ProjectId).FieldName
                        , Conditions.Equals, project.Id.ToString(), false, ValueTypes.Numeric);
                    var selectStatement =
                        TableDefinition.Context.DataProcessor.SqlGenerator.GenerateSelectStatement(selectQuery);
                    formula = $"{AppGlobals.LookupContext.Users.GetFieldDefinition(p => p.Id).GetSqlFormatObject()} IN ({selectStatement})";
                    UserAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter("User", null, "", formula);

                }
            }
        }
        protected override void LoadFromEntity(ProjectTask entity)
        {
            ProjectAutoFillValue = entity.Project.GetAutoFillValue();
            UserAutoFillValue = entity.User.GetAutoFillValue();
            MinutesCost = entity.MinutesCost;
            PercentComplete = entity.PercentComplete;
            Notes = entity.Notes;
            LaborPartsManager.LoadGrid(entity.LaborParts);
            LaborPartsManager.CalculateTotalMinutesCost();
        }

        protected override ProjectTask GetEntityData()
        {
            var result = new ProjectTask
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                ProjectId = ProjectAutoFillValue.GetEntity<Project>().Id,
                UserId = UserAutoFillValue.GetEntity<User>().Id,
                MinutesCost = MinutesCost,
                PercentComplete = PercentComplete,
                Notes = Notes,
            };
            if (!TableDefinition.HasRight(RightTypes.AllowEdit) && Entity != null)
            {
                result.Name = Entity.Name;
            }
            return result;
        }

        protected override bool ValidateEntity(ProjectTask entity)
        {
            if (entity.UserId == 0)
            {
                var message =
                    "The Assigned To User is invalid.  Please select a valid User that is added to this project.";
                var caption = "Invalid Assigned To User";

                View.OnValidationFail(TableDefinition.GetFieldDefinition(p => p.UserId),
                    message, caption);
                return false;
            }

            if (entity.ProjectId == 0)
            {
                return base.ValidateEntity(entity);
            }
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectUser>();
            if (table != null)
            {
                var projectUser = table.FirstOrDefault(p => p.UserId == entity.UserId
                  && p.ProjectId == ProjectAutoFillValue.GetEntity<Project>().Id);
                if (projectUser == null)
                {
                    var message =
                        "The Assigned To User you have chosen has not been added to the project. Please select a user that has been added to the project.";
                    var caption = "Invalid User";
                    View.OnValidationFail(TableDefinition.GetFieldDefinition(p => p.UserId),
                        message, caption);
                    return false;
                }
            }
            return base.ValidateEntity(entity);
        }

        protected override void ClearData()
        {
            Id = 0;
            ProjectAutoFillValue = DefaultProjectAutoFillValue;
            UserAutoFillValue = null;
            MinutesCost = 0;
            PercentComplete = 0;
            Notes = string.Empty;
            LaborPartsManager.SetupForNewRecord();
            LaborPartsManager.CalculateTotalMinutesCost();
        }

        protected override bool SaveEntity(ProjectTask entity)
        {
            var details = LaborPartsManager.GetEntityList();
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Project Task");

            if (result)
            {
                foreach (var projectTaskLaborPart in details)
                {
                    projectTaskLaborPart.ProjectTaskId = entity.Id;
                }

                var table = context.GetTable<ProjectTaskLaborPart>();
                context.RemoveRange(table.Where(p => p.ProjectTaskId == entity.Id));
                context.AddRange(details);
                result = context.Commit("Saving Project Task");
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();

            var entity = context.GetTable<ProjectTask>()
                .FirstOrDefault(p => p.Id == Id);

            var table = context.GetTable<ProjectTaskLaborPart>();
            context.RemoveRange(table.Where(p => p.ProjectTaskId == entity.Id));

            return context.DeleteEntity(entity, "Deleting Project Task");
        }
    }
}
