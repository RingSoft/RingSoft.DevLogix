using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class CustomerComputerViewModel : DevLogixDbMaintenanceViewModel<CustomerComputer>
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

        private string? _brand;

        public string? Brand
        {
            get => _brand;
            set
            {
                if (_brand == value)
                    return;

                _brand = value;
                OnPropertyChanged();
            }
        }

        private string? _operatingSystem;

        public string? OperatingSystem
        {
            get => _operatingSystem;
            set
            {
                if (_operatingSystem == value)
                    return;

                _operatingSystem = value;
                OnPropertyChanged();
            }
        }

        private double? _speed;

        public double? Speed
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

        private int? _ramSize;

        public int? RamSize
        {
            get => _ramSize;
            set
            {
                if (_ramSize == value)
                    return;

                _ramSize = value;
                OnPropertyChanged();
            }
        }

        private int? _hardDriveSize;

        public int? HardDriveSize
        {
            get => _hardDriveSize;
            set
            {
                if (_hardDriveSize == value)
                    return;

                _hardDriveSize = value;
                OnPropertyChanged();
            }
        }

        private int? _hardDriveFree;

        public int? HardDriveFree
        {
            get => _hardDriveFree;
            set
            {
                if (_hardDriveFree == value)
                    return;

                _hardDriveFree = value;
                OnPropertyChanged();
            }
        }

        private string? _screenResolution;

        public string? ScreenResolution
        {
            get => _screenResolution;
            set
            {
                if (_screenResolution == value)
                    return;

                _screenResolution = value;
                OnPropertyChanged();
            }
        }

        private int? _internetSpeed;

        public int? InternetSpeed
        {
            get => _internetSpeed;
            set
            {
                if (_internetSpeed == value)
                    return;

                _internetSpeed = value;
                OnPropertyChanged();
            }
        }

        private string? _databasePlatform;

        public string? DatabasePlatform
        {
            get => _databasePlatform;
            set
            {
                if (_databasePlatform == value)
                    return;

                _databasePlatform = value;
                OnPropertyChanged();
            }
        }

        private string? _printer;

        public string? Printer
        {
            get => _printer;
            set
            {
                if (_printer == value)
                    return;

                _printer = value;
                OnPropertyChanged();
            }
        }

        private string? _notes;

        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value) return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AutoFillValue DefaultCustomerAutoFillValue { get; private set; }
        public CustomerComputerViewModel()
        {
            CustomerAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(
                p => p.CustomerId));
        }

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Customer)
                {
                    var customer =
                        AppGlobals.LookupContext.Customer.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);

                    var context = AppGlobals.DataRepository.GetDataContext();
                    var table = context.GetTable<Customer>();
                    customer = table.FirstOrDefault(p => p.Id == customer.Id);
                    DefaultCustomerAutoFillValue = customer.GetAutoFillValue();
                }
            }

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(CustomerComputer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(CustomerComputer entity)
        {
            CustomerAutoFillValue = entity.Customer.GetAutoFillValue();
            Brand = entity.Brand;
            OperatingSystem = entity.OperatingSystem;
            Speed = entity.Speed;
            RamSize = entity.RamSize;
            HardDriveSize = entity.HardDriveSize;
            HardDriveFree = entity.HardDriveFree;
            ScreenResolution = entity.ScreenResolution;
            InternetSpeed = entity.InternetSpeed;
            DatabasePlatform = entity.DatabasePlatform;
            Printer = entity.Printer;
            Notes = entity.Notes;
        }

        protected override CustomerComputer GetEntityData()
        {
            return new CustomerComputer
            {
                Id = Id,
                Name = KeyAutoFillValue?.Text,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                Brand = Brand,
                OperatingSystem = OperatingSystem,
                Speed = Speed,
                RamSize = RamSize,
                HardDriveSize = HardDriveSize,
                HardDriveFree = HardDriveFree,
                ScreenResolution = ScreenResolution,
                InternetSpeed = InternetSpeed,
                DatabasePlatform = DatabasePlatform,
                Printer = Printer,
                Notes = Notes,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            CustomerAutoFillValue = DefaultCustomerAutoFillValue;
            Brand = null;
            OperatingSystem = null;
            Speed = null;
            RamSize = null;
            HardDriveSize = null;
            HardDriveFree = null;
            ScreenResolution = null;
            InternetSpeed = null;
            DatabasePlatform = null;
            Printer = null;
            Notes = null;
        }
    }
}
