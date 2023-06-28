using System.ComponentModel;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectMaterialView : IDbMaintenanceView
    {
        void GetNewLineType(string text, out PrimaryKeyValue materialPartPkValue, out MaterialPartLineTypes lineType);

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

    public class ProjectMaterialViewModel : DevLogixDbMaintenanceViewModel<ProjectMaterial>
    {
        public override TableDefinition<ProjectMaterial> TableDefinition => AppGlobals.LookupContext.ProjectMaterials;

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

        private LookupCommand _historyLookupCommand;

        public LookupCommand HistoryLookupCommand
        {
            get => _historyLookupCommand;
            set
            {
                if (_historyLookupCommand == value)
                    return;

                _historyLookupCommand = value;
                OnPropertyChanged(null, false);
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

            base.Initialize();
        }

        protected override ProjectMaterial PopulatePrimaryKeyControls(ProjectMaterial newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = new ProjectMaterial();
            var context = AppGlobals.DataRepository.GetDataContext();
            result = context.GetTable<ProjectMaterial>()
                .Include(p => p.Project)
                .Include(p => p.Parts)
                .ThenInclude(p => p.MaterialPart)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = result.GetAutoFillValue();
                HistoryLookup.FilterDefinition.ClearFixedFilters();
                HistoryLookup.FilterDefinition.AddFixedFilter(p => p.ProjectMaterialId, Conditions.Equals, Id);
                HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);
                PostCommand.IsEnabled = true;
            }

            return result;
        }

        protected override void LoadFromEntity(ProjectMaterial entity)
        {
            _loading = true;
            Cost = entity.Cost;
            ProjectAutoFillValue = entity.Project.GetAutoFillValue();
            ActualCost = entity.ActualCost;
            IsCostEdited = entity.IsCostEdited;
            Notes = entity.Notes;
            ProjectMaterialPartManager.LoadGrid(entity.Parts);
            ProjectMaterialPartManager.CalculateTotalCost();
            _loading = false;
        }

        protected override ProjectMaterial GetEntityData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
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
            ProjectMaterialPartManager.SetupForNewRecord();
            PostCommand.IsEnabled = ProjectAutoFillValue.IsValid();
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool ValidateEntity(ProjectMaterial entity)
        {
            if (!ProjectMaterialPartManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
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


        protected override bool SaveEntity(ProjectMaterial entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Project Material");
            if (result)
            {
                var parts = ProjectMaterialPartManager.GetEntityList();
                foreach (var projectMaterialPart in parts)
                {
                    projectMaterialPart.ProjectMaterialId = entity.Id;
                }

                var origParts = context.GetTable<ProjectMaterialPart>()
                    .Where(p => p.ProjectMaterialId == Id).ToList();
                context.RemoveRange(origParts);
                context.AddRange(parts);

            }

            result = context.Commit("Committing Project Material");
            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectMaterial>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                var origParts = context.GetTable<ProjectMaterialPart>()
                    .Where(p => p.ProjectMaterialId == Id).ToList();
                context.RemoveRange(origParts);

                return context.DeleteEntity(entity, "Deleting Project Material");
            }

            return true;
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
            var lookupData = new LookupDataBase(recalcFilter, new LookupUserInterface()
            {
                PageSize = 10
            });
            var recordCount = lookupData.GetRecordCountWait();
            var currentProjectMaterial = 1;
            var context = AppGlobals.DataRepository.GetDataContext();
            var projectMaterialTable = context.GetTable<ProjectMaterial>();
            
            lookupData.PrintDataChanged += (sender, args) =>
            {
                var table = args.OutputTable;
                foreach (DataRow tableRow in table.Rows)
                {
                    var projectMaterialPrimaryKey = new PrimaryKeyValue(TableDefinition);
                    projectMaterialPrimaryKey.PopulateFromDataRow(tableRow);
                    if (projectMaterialPrimaryKey.IsValid)
                    {
                        var projectMaterial = TableDefinition.GetEntityFromPrimaryKeyValue(projectMaterialPrimaryKey);
                        projectMaterial = projectMaterialTable
                            .Include(p => p.History)
                            .FirstOrDefault(p => p.Id == projectMaterial.Id);
                        if (projectMaterial != null)
                        {
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
            lookupData.GetPrintData();
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
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKey);
        }
        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.MaterialViewModels.Remove(this);
            base.OnWindowClosing(e);
        }
    }
}
