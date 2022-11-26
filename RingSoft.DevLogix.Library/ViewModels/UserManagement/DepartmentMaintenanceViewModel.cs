using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class DepartmentMaintenanceViewModel : DevLogixDbMaintenanceViewModel<Department>
    {
        public override TableDefinition<Department> TableDefinition => AppGlobals.LookupContext.Departments;

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

        private AutoFillSetup _fixStatusAutoFillSetup;

        public AutoFillSetup FixStatusAutoFillSetup
        {
            get => _fixStatusAutoFillSetup;
            set
            {
                if (_fixStatusAutoFillSetup == value)
                    return;

                _fixStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _fixStatusAutoFillValue;

        public AutoFillValue FixStatusAutoFillValue
        {
            get => _fixStatusAutoFillValue;
            set
            {
                if (_fixStatusAutoFillValue == value)
                    return;

                _fixStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _passStatusAutoFillSetup;

        public AutoFillSetup PassStatusAutoFillSetup
        {
            get => _passStatusAutoFillSetup;
            set
            {
                if (_passStatusAutoFillSetup == value)
                    return;

                _passStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _passStatusAutoFillValue;

        public AutoFillValue PassStatusAutoFillValue
        {
            get => _passStatusAutoFillValue;
            set
            {
                if (_passStatusAutoFillValue == value)
                    return;

                _passStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _failStatusAutoFillSetup;

        public AutoFillSetup FailStatusAutoFillSetup
        {
            get => _failStatusAutoFillSetup;
            set
            {
                if (_failStatusAutoFillSetup == value)
                    return;

                _failStatusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _failStatusAutoFillValue;

        public AutoFillValue FailStatusAutoFillValue
        {
            get => _failStatusAutoFillValue;
            set
            {
                if (_failStatusAutoFillValue == value)
                    return;

                _failStatusAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _fixStatusText;

        public string FixStatusText
        {
            get => _fixStatusText;
            set
            {
                if (_fixStatusText == value) return;
                _fixStatusText = value;
                OnPropertyChanged();
            }
        }

        private string _passStatusText;

        public string PassStatusText
        {
            get => _passStatusText;
            set
            {
                if (_passStatusText == value) return;
                _passStatusText = value;
                OnPropertyChanged();
            }
        }

        private string _failStatusText;

        public string FailStatusText
        {
            get => _failStatusText;
            set
            {
                if (_failStatusText == value) return;
                _failStatusText = value;
                OnPropertyChanged();
            }
        }

        protected override void Initialize()
        {
            FixStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFixStatusId));
            PassStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorPassStatusId));
            FailStatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorFailStatusId));
            var test = this;
            base.Initialize();
        }

        protected override Department PopulatePrimaryKeyControls(Department newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var query = AppGlobals.DataRepository.GetTable<Department>();
            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            }

            return result;
        }

        protected override void LoadFromEntity(Department entity)
        {
            FixStatusAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                    entity.ErrorFixStatusId.ToString());

            PassStatusAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                    entity.ErrorPassStatusId.ToString());

            FailStatusAutoFillValue =
                AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.ErrorStatuses,
                    entity.ErrorFailStatusId.ToString());

            FixStatusText = entity.FixText;
            PassStatusText = entity.PassText;
            FailStatusText = entity.FailText;
        }

        protected override Department GetEntityData()
        {
            var result = new Department
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                FixText = FixStatusText,
                PassText = PassStatusText,
                FailText = FailStatusText,
            };

            if (FixStatusAutoFillValue.IsValid())
            {
                result.ErrorFixStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(FixStatusAutoFillValue.PrimaryKeyValue).Id;
            }

            if (PassStatusAutoFillValue.IsValid())
            {
                result.ErrorPassStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(PassStatusAutoFillValue.PrimaryKeyValue).Id;
            }

            if (FailStatusAutoFillValue.IsValid())
            {
                result.ErrorFailStatusId = AppGlobals.LookupContext.ErrorStatuses
                    .GetEntityFromPrimaryKeyValue(FailStatusAutoFillValue.PrimaryKeyValue).Id;
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            FixStatusAutoFillValue = null;
            PassStatusAutoFillValue = null;
            FailStatusAutoFillValue = null;
            FixStatusText = PassStatusText = FailStatusText = string.Empty;
        }

        protected override bool SaveEntity(Department entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                return context.SaveEntity(entity, $"Saving Department '{entity.Description}'");
            }
            return false;
        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetTable<Department>();
            if (query != null)
            {
                var department = query.FirstOrDefault(p => p.Id == Id);
                var context = AppGlobals.DataRepository.GetDataContext();
                if (context != null && department != null)
                {
                    return context.DeleteEntity(department, $"Deleting Department '{department.Description}'");
                }
            }
            return false;

        }
    }
}
