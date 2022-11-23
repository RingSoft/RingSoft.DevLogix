using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorStatusViewModel : DevLogixDbMaintenanceViewModel<ErrorStatus>
    {
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

        public override TableDefinition<ErrorStatus> TableDefinition => AppGlobals.LookupContext.ErrorStatuses;

        protected override ErrorStatus PopulatePrimaryKeyControls(ErrorStatus newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = AppGlobals.DataRepository.GetErrorStatus(newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            return result;
        }

        protected override void LoadFromEntity(ErrorStatus entity)
        {
            
        }

        protected override ErrorStatus GetEntityData()
        {
            var errorStatus = new ErrorStatus
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
            };
            return errorStatus;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
        }

        protected override bool SaveEntity(ErrorStatus entity)
        {
            return AppGlobals.DataRepository.SaveErrorStatus(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteErrorStatus(Id);
        }
    }
}
