using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
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

        private bool _loading;
        private bool _calculating;

        public ProjectMaterialViewModel()
        {
            ProjectAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectId));

        }
        protected override ProjectMaterial PopulatePrimaryKeyControls(ProjectMaterial newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = new ProjectMaterial();
            var context = AppGlobals.DataRepository.GetDataContext();
            result = context.GetTable<ProjectMaterial>()
                .Include(p => p.Project)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
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
        }

        protected override bool SaveEntity(ProjectMaterial entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Project Material");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProjectMaterial>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                return context.DeleteEntity(entity, "Deleting Project Material");
            }

            return true;
        }
    }
}
