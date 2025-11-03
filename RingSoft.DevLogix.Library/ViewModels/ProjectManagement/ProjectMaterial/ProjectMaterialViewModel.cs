using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using System.ComponentModel;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectMaterialView : IDbMaintenanceView
    {
        bool GetNewLineType(string text, out PrimaryKeyValue materialPartPkValue, out MaterialPartLineTypes lineType);

        bool ShowCommentEditor(DataEntryGridMemoValue comment);

        bool DoPostCosts(Project project);

        bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition);

        string StartRecalcProcedure(LookupDefinitionBase lookupDefinition);

        void UpdateRecalcProcedure(int currentProjectTask, int totalProjectTasks, string currentProjectTaskText);

        void GotoGrid();
    }
    public enum ProjectMaterialSpecialRights
    {
        AllowMaterialPost = 1,
    }

    public class ProjectMaterialViewModel : DbMaintenanceViewModel<ProjectMaterial>
    {
        #region Properties

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
                OnPropertyChanged();
            }
        }

        private double _cost;

        public double Cost
        {
            get => _cost;
            set
            {
                if (_cost == value)
                    return;

                _cost = value;
                if (!_loading && !_calculating)
                {
                    IsCostEdited = true;
                }
                OnPropertyChanged();
            }
        }

        private bool _isCostEdited;

        public bool IsCostEdited
        {
            get => _isCostEdited;
            set
            {
                if (_isCostEdited == value)
                    return;

                _isCostEdited = value;
                OnPropertyChanged();
            }
        }


        private double _actualCost;

        public double ActualCost
        {
            get => _actualCost;
            set
            {
                if (_actualCost == value)
                    return;

                _actualCost = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _totalCost;

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost == value)
                    return;

                _totalCost = value;
                OnPropertyChanged();
            }
        }

        private ProjectMaterialPartManager _projectMaterialPartManager;

        public ProjectMaterialPartManager ProjectMaterialPartManager
        {
            get => _projectMaterialPartManager;
            set
            {
                if (_projectMaterialPartManager == value)
                    return;

                _projectMaterialPartManager = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> _historyLookup;

        public LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> HistoryLookup
        {
            get => _historyLookup;
            set
            {
                if (_historyLookup == value)
                    return;

                _historyLookup = value;
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

        #endregion

        public new IProjectMaterialView View { get; private set; }

        public AutoFillValue DefaultProjectAutoFillValue { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public RelayCommand PostCommand { get; private set; }

        private bool _loading;
        private bool _calculating;

        public ProjectMaterialViewModel()
        {
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));
            ProjectMaterialPartManager = new ProjectMaterialPartManager(this);
            RegisterGrid(ProjectMaterialPartManager);

            RecalcCommand = new RelayCommand(Recalc);

            PostCommand = new RelayCommand(PostCost);

            TablesToDelete.Add(AppGlobals.LookupContext.ProjectMaterialParts);
        }

        protected override void Initialize()
        {
            if (base.View is IProjectMaterialView projectMaterialView)
            {
                View = projectMaterialView;
            }

            Project project = null;
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Projects)
                {
                    project =
                        AppGlobals.LookupContext.Projects.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    project = project.FillOutProperties(false);
                    DefaultProjectAutoFillValue = project.GetAutoFillValue();
                }
            }

            if (DefaultProjectAutoFillValue.IsValid())
            {
                FindButtonLookupDefinition.FilterDefinition.AddFixedFieldFilter(
                    TableDefinition
                        .GetFieldDefinition(p => p.ProjectId), Conditions.Equals, project.Id.ToString());
            }
            AppGlobals.MainViewModel.MaterialViewModels.Add(this);

            var historyLookup =
                new LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory>(AppGlobals.LookupContext
                    .ProjectMaterialHistory);
            historyLookup.AddVisibleColumnDefinition(p => p.Date, p => p.Date);
            historyLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            historyLookup.AddVisibleColumnDefinition(p => p.Quantity, p => p.Quantity);
            historyLookup.AddVisibleColumnDefinition(p => p.Cost, p => p.Cost);
            historyLookup.InitialOrderByType = OrderByTypes.Descending;
            HistoryLookup = historyLookup;

            RegisterLookup(HistoryLookup);

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(ProjectMaterial newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            //Peter Ringering - 11/23/2024 07:15:52 PM - E-70
            PostCommand.IsEnabled = true;
        }

        protected override void LoadFromEntity(ProjectMaterial entity)
        {
            _loading = true;
            Cost = entity.Cost;
            ProjectAutoFillValue = entity.Project.GetAutoFillValue();
            ActualCost = entity.ActualCost;
            IsCostEdited = entity.IsCostEdited;
            Notes = entity.Notes;
            ProjectMaterialPartManager.CalculateTotalCost();
            _loading = false;
        }

        protected override ProjectMaterial GetEntityData()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectMaterial>();
            var existingMaterial = table.FirstOrDefault(p => p.Id == Id);
            if (existingMaterial != null)
            {
                ActualCost = existingMaterial.ActualCost;
            }
            return new ProjectMaterial
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                ProjectId = ProjectAutoFillValue.GetEntity<Project>().Id,
                Cost = Cost,
                IsCostEdited = IsCostEdited,
                ActualCost = ActualCost,
                Notes = Notes,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            ProjectAutoFillValue = DefaultProjectAutoFillValue;
            Cost = 0;
            IsCostEdited = false;
            ActualCost = 0;
            Notes = string.Empty;
            PostCommand.IsEnabled = ProjectAutoFillValue.IsValid();
        }

        public void SetTotalCost(double total)
        {
            _calculating = true;
            TotalCost = total;
            if (!IsCostEdited)
            {
                Cost = total;
            }

            _calculating = false;
        }

        public void Recalc()
        {
            var recalcFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(recalcFilter))
                return;

            var result = View.StartRecalcProcedure(recalcFilter);
            if (result.IsNullOrEmpty())
            {
                var message = "Recalculation complete.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Recalculation", RsMessageBoxIcons.Information);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Recalculation Error", RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase recalcFilter)
        {
            var result = string.Empty;
            DbDataProcessor.DontDisplayExceptions = true;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(recalcFilter, false);
            var recordCount = lookupData.GetRecordCount();
            var currentProjectMaterial = 1;
            var context = SystemGlobals.DataRepository.GetDataContext();
            var projectMaterialTable = context.GetTable<ProjectMaterial>();
            
            lookupData.PrintOutput += (sender, args) =>
            {
                var total = args.Result.Count;
                var index = 0;
                foreach (var primaryKeyValue in args.Result)
                {
                    index++;
                    if (primaryKeyValue.IsValid())
                    {
                        var projectMaterial = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
                        projectMaterial = projectMaterialTable
                            .Include(p => p.History)
                            .FirstOrDefault(p => p.Id == projectMaterial.Id);
                        if (projectMaterial != null)
                        {
                            View.UpdateRecalcProcedure(
                                index
                                , total
                                , $"Recalculating {projectMaterial.Name}");
                            var historyItems = projectMaterial.History.ToList();
                            projectMaterial.ActualCost = historyItems.Sum(p => p.Quantity * p.Cost);
                            if (!context.SaveNoCommitEntity(projectMaterial, "Saving Project Material"))
                            {
                                result = DbDataProcessor.LastException;
                                args.Abort = true;
                                return;
                            }

                            if (projectMaterial.Id == Id)
                            {
                                ActualCost = projectMaterial.ActualCost;
                            }
                        }
                    }
                }
            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (!context.Commit("Saving Project Materials"))
                {
                    result = DbDataProcessor.LastException;
                }
            }


            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        public void PostCost()
        {
            var project = ProjectAutoFillValue.GetEntity<Project>();
            if (View.DoPostCosts(project))
            {
                
            }
        }

        public void RefreshCost(double cost)
        {
            ActualCost = cost;
            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(Entity);
            HistoryLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh, primaryKey));
        }
        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.MaterialViewModels.Remove(this);
            base.OnWindowClosing(e);
        }
    }
}
