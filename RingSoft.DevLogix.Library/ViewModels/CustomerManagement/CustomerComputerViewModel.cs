using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class CustomerComputerViewModel : DevLogixDbMaintenanceViewModel<CustomerComputer>
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

        protected override CustomerComputer PopulatePrimaryKeyControls(CustomerComputer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<CustomerComputer>();
            var result = table.FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            return result;
        }

        protected override void LoadFromEntity(CustomerComputer entity)
        {
            
        }

        protected override CustomerComputer GetEntityData()
        {
            return new CustomerComputer
            {
                Id = Id,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
        }

        protected override bool SaveEntity(CustomerComputer entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Customer Computer");
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<CustomerComputer>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting Customer Computer");
            }
            return result;
        }
    }
}
