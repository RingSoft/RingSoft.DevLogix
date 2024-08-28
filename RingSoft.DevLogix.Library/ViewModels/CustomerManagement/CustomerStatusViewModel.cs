using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public  class CustomerStatusViewModel : DbMaintenanceViewModel<CustomerStatus>
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

        #endregion

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
    }
}
