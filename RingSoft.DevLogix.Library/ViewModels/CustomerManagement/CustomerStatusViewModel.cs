using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public  class CustomerStatusViewModel : DevLogixDbMaintenanceViewModel<CustomerStatus>
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

        protected override void PopulatePrimaryKeyControls(CustomerStatus newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(CustomerStatus entity)
        {
            
        }

        protected override CustomerStatus GetEntityData()
        {
            var customerStatus = new CustomerStatus()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
            };
            return customerStatus;

        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
        }

        protected override bool SaveEntity(CustomerStatus entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, $"Saving Customer Status '{entity.Description}'");
        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<CustomerStatus>();
            var entity = query.FirstOrDefault(p => p.Id == Id);
            var context = AppGlobals.DataRepository.GetDataContext();
            return entity != null && context.DeleteEntity(entity, $"Deleting Customer Status '{entity.Description}'");
        }
    }
}
