using System.Linq;
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

        protected override void PopulatePrimaryKeyControls(ErrorStatus newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
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
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, $"Saving Error Status '{entity.Description}'");
        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<ErrorStatus>();
            var entity = query.FirstOrDefault(p => p.Id == Id);
            var context = AppGlobals.DataRepository.GetDataContext();
            return entity != null && context.DeleteEntity(entity, $"Deleting Error Status '{entity.Description}'");
        }
    }
}
