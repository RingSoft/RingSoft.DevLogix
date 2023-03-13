using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectMaterialView : IDbMaintenanceView
    {
        void GetNewLineType(string text, out PrimaryKeyValue materialPartPkValue, out MaterialPartLineTypes lineType);

        bool ShowCommentEditor(DataEntryGridMemoValue comment);
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

        private decimal _cost;

        public decimal Cost
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


        private decimal _actualCost;

        public decimal ActualCost
        {
            get => _actualCost;
            set
            {
                if (_actualCost == value)
                    return;

                _actualCost = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalCost;

        public decimal TotalCost
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

        private bool _loading;
        private bool _calculating;

        public ProjectMaterialViewModel()
        {
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));
            ProjectMaterialPartManager = new ProjectMaterialPartManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.ProjectMaterialParts);
        }

        protected override void Initialize()
        {
            if (base.View is IProjectMaterialView projectMaterialView)
            {
                View = projectMaterialView;
            }
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
            ProjectAutoFillValue = null;
            Cost = 0;
            IsCostEdited = false;
            ActualCost = 0;
            Notes = string.Empty;
            ProjectMaterialPartManager.SetupForNewRecord();
        }

        public void SetTotalCost(decimal total)
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
    }
}
