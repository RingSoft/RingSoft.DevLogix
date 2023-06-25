using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class CustomerViewModel : DevLogixDbMaintenanceViewModel<Customer>
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

        private string? _contactName;

        public string? ContactName
        {
            get => _contactName;
            set
            {
                if (_contactName == value)
                    return;

                _contactName = value;
                OnPropertyChanged();
            }
        }

        private string? _contactTitle;

        public string? ContactTitle
        {
            get => _contactTitle;
            set
            {
                if (_contactTitle == value)
                    return;

                _contactTitle = value;
                OnPropertyChanged();
            }
        }

        private string? _address;

        public string? Address
        {
            get => _address;
            set
            {
                if (_address == value)
                {
                    return;
                }
                _address = value;
                OnPropertyChanged();
            }
        }

        private string? _city;

        public string? City
        {
            get => _city;
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged();
            }
        }

        private string? _region;

        public string? Region
        {
            get => _region;
            set
            {
                if (_region == value)
                    return;

                _region = value;
                OnPropertyChanged();
            }
        }

        private string? _postalCode;

        public string? PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode == value)
                    return;

                _postalCode = value;
                OnPropertyChanged();
            }
        }

        private string? _country;

        public string? Country
        {
            get => _country;
            set
            {
                if (_country == value)
                    return;

                _country = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber == value)
                    return;

                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _timeZoneAutoFillSetup;

        public AutoFillSetup TimeZoneAutoFillSetup 
        {
            get => _timeZoneAutoFillSetup;
            set
            {
                if (_timeZoneAutoFillSetup == value)
                    return;

                _timeZoneAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _timeZoneAutoFillValue;

        public AutoFillValue TimeZoneAutoFillValue
        {
            get => _timeZoneAutoFillValue;
            set
            {
                if (_timeZoneAutoFillValue == value)
                    return;

                _timeZoneAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _terroitoryAutoFillSetup;

        public AutoFillSetup TerritoryAutoFillSetup
        {
            get => _terroitoryAutoFillSetup;
            set
            {
                if (_terroitoryAutoFillSetup == value)
                    return;

                _terroitoryAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _territoryAutoFillValue;

        public AutoFillValue TerritoryAutoFillValue
        {
            get => _territoryAutoFillValue;
            set
            {
                if (_territoryAutoFillValue == value)
                    return;

                _territoryAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string? _emailAddress;

        public string? EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (_emailAddress == value)
                    return;

                _emailAddress = value;
                OnPropertyChanged();
            }
        }

        private CustomerProductManager _productManager;

        public CustomerProductManager ProductManager
        {
            get => _productManager;
            set
            {
                if (_productManager == value)
                    return;

                _productManager = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public CustomerViewModel()
        {
            TimeZoneAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TimeZoneId));
            TerritoryAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TerritoryId));

            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);

            ProductManager = new CustomerProductManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.CustomerProduct);
        }

        protected override Customer PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetCustomer(newEntity.Id);

            Id = newEntity.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            return result;
        }

        private static Customer? GetCustomer(int customerId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            var result = table
                .Include(p => p.TimeZone)
                .Include(p => p.Territory)
                .Include(p => p.CustomerProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(p => p.Id == customerId);
            return result;
        }

        protected override void LoadFromEntity(Customer entity)
        {
            ContactName = entity.ContactName;
            ContactTitle = entity.ContactTitle;
            Address = entity.Address;
            City = entity.City;
            Region = entity.Region;
            PostalCode = entity.PostalCode;
            Country = entity.Country;
            PhoneNumber = entity.Phone;
            TimeZoneAutoFillValue = entity.TimeZone.GetAutoFillValue();
            TerritoryAutoFillValue = entity.Territory.GetAutoFillValue();
            EmailAddress = entity.EmailAddress;
            ProductManager.LoadGrid(entity.CustomerProducts);
        }

        protected override Customer GetEntityData()
        {
            var result = new Customer
            {
                Id = Id,
                CompanyName = KeyAutoFillValue.Text,
                ContactName = ContactName,
                ContactTitle = ContactTitle,
                Address = Address,
                City = City,
                Region = Region,
                PostalCode = PostalCode,
                Country = Country,
                Phone = PhoneNumber,
                TimeZoneId = TimeZoneAutoFillValue.GetEntity<TimeZone>().Id,
                TerritoryId = TerritoryAutoFillValue.GetEntity<Territory>().Id,
                EmailAddress = EmailAddress,
            };
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            ContactName = null;
            ContactTitle = null;
            Address = null;
            City = null;
            Region = null;
            PostalCode = null;
            Country = null;
            PhoneNumber = string.Empty;
            TimeZoneAutoFillValue = null;
            TerritoryAutoFillValue = null;
            EmailAddress = null;
            ProductManager.SetupForNewRecord();
        }

        protected override bool SaveEntity(Customer entity)
        {
            var customerProducts = ProductManager.GetEntityList();
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Customer");

            var table = context.GetTable<CustomerProduct>();
            var existingProducts = table.Where(
                p => p.CustomerId == entity.Id).ToList();

            if (result)
            {
                foreach (var customerProduct in customerProducts)
                {
                    customerProduct.CustomerId = entity.Id;
                }
                context.RemoveRange(existingProducts);
                context.AddRange(customerProducts);
            }

            if (result)
            {
                result = context.Commit("Finializing Customer Save");
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = (context.GetTable<Customer>());
            var delCustomer = table.FirstOrDefault(p => p.Id == Id);
            if (delCustomer != null)
            {
                var productTable = context.GetTable<CustomerProduct>();
                var existingProducts = productTable.Where(
                    p => p.CustomerId == Id).ToList();

                context.RemoveRange(existingProducts);

                result = context.DeleteEntity(delCustomer, "Deleting Customer");
            }
            return result;
        }

        private void PunchIn()
        {
            var customer = GetCustomer(Id);
            AppGlobals.MainViewModel.PunchIn(customer);
        }

        private void Recalc()
        {

        }
    }
}
