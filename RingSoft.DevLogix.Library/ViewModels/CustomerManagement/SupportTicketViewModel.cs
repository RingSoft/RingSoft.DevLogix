using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class SupportTicketViewModel : DevLogixDbMaintenanceViewModel<SupportTicket>
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
                if (!_loading)
                {
                    LoadCustomer();
                }
            }
        }

        private DateTime _createDate;

        public DateTime CreateDate
        {
            get => _createDate;
            set
            {
                if (_createDate == value)
                {
                    return;
                }
                _createDate = value;
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

        private AutoFillSetup _createUserAutoFillSetup;

        public AutoFillSetup CreateUserAutoFillSetup
        {
            get => _createUserAutoFillSetup;
            set
            {
                if (_createUserAutoFillSetup == value)
                    return;

                _createUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _createUserAutoFillValue;

        public AutoFillValue CreateUserAutoFillValue
        {
            get => _createUserAutoFillValue;
            set
            {
                if (_createUserAutoFillValue == value)
                    return;

                _createUserAutoFillValue = value;
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

        private AutoFillSetup _assignedUserAutoFillSetup;

        public AutoFillSetup AssignedUserAutoFillSetup
        {
            get => _assignedUserAutoFillSetup;
            set
            {
                if (_assignedUserAutoFillSetup == value)
                    return;

                _assignedUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedUserAutoFillValue;

        public AutoFillValue AssignedUserAutoFillValue
        {
            get => _assignedUserAutoFillValue;
            set
            {
                if (_assignedUserAutoFillValue == value)
                    return;

                _assignedUserAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _closedDateTime;

        public DateTime? ClosedDate
        {
            get => _closedDateTime;
            set
            {
                if (_closedDateTime == value)
                    return;

                _closedDateTime = value;
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
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PunchInCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        private bool _loading;

        public SupportTicketViewModel()
        {
            CustomerAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.CustomerId));
            CreateUserAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.CreateUserId));
            ProductAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.ProductId));
            AssignedUserAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.AssignedToUserId));

            PunchInCommand = new RelayCommand(PunchIn);
            RecalcCommand = new RelayCommand(Recalc);
        }

        protected override void Initialize()
        {
            ViewLookupDefinition.InitialOrderByField = TableDefinition.GetFieldDefinition(p => p.Id);
            base.Initialize();
        }

        protected override SupportTicket PopulatePrimaryKeyControls(SupportTicket newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetTicket(newEntity.Id);

            Id = result.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            PunchInCommand.IsEnabled = true;
            return result;
        }

        public SupportTicket GetTicket(int ticketId)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<SupportTicket>();
            return table
                .Include(p => p.Customer)
                .Include(p => p.CreateUser)
                .Include(p => p.Product)
                .Include(p => p.AssignedToUser)
                .FirstOrDefault(p => p.Id == ticketId);
        }
        protected override void LoadFromEntity(SupportTicket entity)
        {
            _loading = true;
            CustomerAutoFillValue = entity.Customer.GetAutoFillValue();
            CreateDate = entity.CreateDate.ToLocalTime();
            CreateUserAutoFillValue = entity.CreateUser.GetAutoFillValue();
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            AssignedUserAutoFillValue = entity.AssignedToUser.GetAutoFillValue();
            PhoneNumber = entity.PhoneNumber;
            ClosedDate = entity.CloseDate;
            Notes = entity.Notes;
            _loading = false;
        }

        private void LoadCustomer()
        {
            if (CustomerAutoFillValue.IsValid())
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Customer>();
                var customer = CustomerAutoFillValue.GetEntity<Customer>();
                customer = table.FirstOrDefault(p => p.Id == customer.Id);
                if (customer != null)
                {
                    PhoneNumber = customer.Phone;
                }
            }
        }


        protected override SupportTicket GetEntityData()
        {
            var result = new SupportTicket
            {
                Id = Id,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                CreateDate = CreateDate.ToUniversalTime(),
                PhoneNumber = PhoneNumber,
                CreateUserId = CreateUserAutoFillValue.GetEntity<User>().Id,
                ProductId = ProductAutoFillValue.GetEntity<Product>().Id,
                AssignedToUserId = AssignedUserAutoFillValue.GetEntity<User>().Id,
                CloseDate = ClosedDate,
                Notes = Notes,
            };

            if (KeyAutoFillValue != null)
            {
                result.TicketId = KeyAutoFillValue.Text;
            }

            if (result.AssignedToUserId == 0)
            {
                result.AssignedToUserId = null;
            }

            return result;
        }

        protected override void ClearData()
        {
            _loading = true;
            KeyAutoFillValue = null;
            Id = 0;
            CustomerAutoFillValue = null;
            CreateDate = DateTime.Now;
            PhoneNumber = string.Empty;
            CreateUserAutoFillValue = AppGlobals.LoggedInUser.GetAutoFillValue();
            ProductAutoFillValue = null;
            AssignedUserAutoFillValue = null;
            ClosedDate = null;
            Notes = null;
            PunchInCommand.IsEnabled = false;
            LoadCustomer();
            _loading = false;
        }

        protected override bool SaveEntity(SupportTicket entity)
        {
            var makeTicketId = entity.Id == 0 && entity.TicketId.IsNullOrEmpty();
            var context = AppGlobals.DataRepository.GetDataContext();
            if (makeTicketId)
            {
                entity.TicketId = Guid.NewGuid().ToString();
            }

            var result = context.SaveEntity(entity, "Saving Ticket");
            if (result && makeTicketId)
            {
                entity.TicketId = $"ST-{entity.Id}";
                result = context.SaveEntity(entity, "Updating Ticket ID");
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<SupportTicket>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting Ticket");
            }
            return result;
        }

        private void PunchIn()
        {
            var ticket = GetTicket(Id);
            AppGlobals.MainViewModel.PunchIn(ticket);
        }

        public void Recalc()
        {

        }
    }
}
