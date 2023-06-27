using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
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

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomerAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                    return;

                _customerAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _customerAutoFillValue;

        public AutoFillValue CustomerAutoFillValue
        {
            get => _customerAutoFillValue;
            set
            {
                if (_customerAutoFillValue == value)
                    return;

                _customerAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal? _speed;

        public decimal? Speed
        {
            get => _speed;
            set
            {
                if (_speed == value)
                {
                    return;
                }
                _speed = value;
                OnPropertyChanged();
            }
        }

        public CustomerComputerViewModel()
        {
            CustomerAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(
                p => p.CustomerId));
        }

        protected override CustomerComputer PopulatePrimaryKeyControls(CustomerComputer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<CustomerComputer>();
            var result = table
                .Include(p => p.Customer)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            return result;
        }

        protected override void LoadFromEntity(CustomerComputer entity)
        {
            CustomerAutoFillValue = entity.Customer.GetAutoFillValue();
            Speed = entity.Speed;
        }

        protected override CustomerComputer GetEntityData()
        {
            return new CustomerComputer
            {
                Id = Id,
                Name = KeyAutoFillValue?.Text,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                Speed = Speed,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            CustomerAutoFillValue = null;
            Speed = null;
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
