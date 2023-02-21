using System;
using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Project>
    {
        public override TableDefinition<Project> TableDefinition => AppGlobals.LookupContext.Projects;

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

        private DateTime _deadline;

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline == value)
                    return;

                _deadline = value;
                OnPropertyChanged();
            }
        }

        private DateTime _originalDeadline;

        public DateTime OriginalDeadline
        {
            get => _originalDeadline;
            set
            {
                if (_originalDeadline == value)
                    return;

                _originalDeadline = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _productAutoFillSetup;

        public AutoFillSetup ProductAutoFillSetup
        {
            get => _productAutoFillSetup;
            set
            {
                if (_productAutoFillSetup == value)
                    return;

                _productAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _productAutoFillValue;

        public AutoFillValue ProductAutoFillValue
        {
            get => _productAutoFillValue;
            set
            {
                if (_productAutoFillValue == value)
                    return;

                _productAutoFillValue = value;
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
                {
                    return;
                }
                _notes = value;
                OnPropertyChanged();
            }
        }

        public ProjectMaintenanceViewModel()
        {
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));
        }

        protected override Project PopulatePrimaryKeyControls(Project newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var project = context.GetTable<Project>()
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = project.Id;
            KeyAutoFillValue = KeyAutoFillSetup.GetAutoFillValueForIdValue(project.Id);

            return project;
        }

        protected override void LoadFromEntity(Project entity)
        {
            Deadline = entity.Deadline.ToLocalTime();
            OriginalDeadline = entity.OriginalDeadline.ToLocalTime();
            ProductAutoFillValue = ProductAutoFillSetup.GetAutoFillValueForIdValue(entity.ProductId);
            Notes = entity.Notes;
        }

        protected override Project GetEntityData()
        {
            var result = new Project
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Deadline = Deadline.ToUniversalTime(),
                OriginalDeadline = OriginalDeadline.ToUniversalTime(),
                ProductId = ProductAutoFillValue.GetEntity(AppGlobals.LookupContext.Products).Id,
                Notes = Notes,
            };

            if (result.ProductId == 0)
            {
                result.ProductId = null;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            OriginalDeadline = Deadline = DateTime.Now;
            ProductAutoFillValue = null;
            Notes = null;
        }

        protected override bool SaveEntity(Project entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();

            var result = false;

            result = context.SaveEntity(entity, "Saving Project");
            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<Project>();

            var result = false;
            var project = query.FirstOrDefault(p => p.Id == Id);
            if (project != null)
            {
                result = context.DeleteEntity(project, "Deleting Project");
            }

            return result;
        }
    }
}
