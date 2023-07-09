using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

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

        private string _totalTimeSpent;

        public string TotalTimeSpent
        {
            get => _totalTimeSpent;
            set
            {
                if (_totalTimeSpent == value)
                    return;

                _totalTimeSpent = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _totalCost;

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost == value)
                    return;

                _totalCost = value;
                OnPropertyChanged(null, false);
            }
        }

        private LookupDefinition<TimeClockLookup, TimeClock> _timeClockLookup;

        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockLookup
        {
            get => _timeClockLookup;
            set
            {
                if (_timeClockLookup == value)
                    return;

                _timeClockLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _timeClockLookupCommand;

        public LookupCommand TimeClockLookupCommand
        {
            get => _timeClockLookupCommand;
            set
            {
                if (_timeClockLookupCommand == value)
                    return;

                _timeClockLookupCommand = value;
                OnPropertyChanged(null, false);
            }
        }


        private SupportTicketCostManager _ticketUserGridManager;

        public SupportTicketCostManager TicketUserGridManager
        {
            get => _ticketUserGridManager;
            set
            {
                if (_ticketUserGridManager == value)
                    return;

                _ticketUserGridManager = value;
                OnPropertyChanged();
            }
        }

        private SupportTicketErrorManager  _ticketErrorGridManager;

        public SupportTicketErrorManager TicketErrorGridManager
        {
            get => _ticketErrorGridManager;
            set
            {
                if (_ticketErrorGridManager == value)
                    return;

                _ticketErrorGridManager = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PunchInCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        public AutoFillValue DefaultCustomerAutoFillValue { get; private set; }

        public double MinutesSpent { get; private set; }

        private bool _loading;

        public SupportTicketViewModel()
        {
            TablesToDelete.Add(AppGlobals.LookupContext.SupportTicketUser);
            TablesToDelete.Add(AppGlobals.LookupContext.SupportTicketError);

            CustomerAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.CustomerId));
            CreateUserAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.CreateUserId));
            ProductAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.ProductId));
            AssignedUserAutoFillSetup = new AutoFillSetup(
                TableDefinition.GetFieldDefinition(p => p.AssignedToUserId));

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            PunchInCommand = new RelayCommand(PunchIn);
            RecalcCommand = new RelayCommand(Recalc);
        }

        protected override void Initialize()
        {
            ViewLookupDefinition.InitialOrderByField = TableDefinition.GetFieldDefinition(p => p.Id);
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

            TicketUserGridManager = new SupportTicketCostManager(this);
            TicketErrorGridManager = new SupportTicketErrorManager(this);
            base.Initialize();
        }

        protected override SupportTicket PopulatePrimaryKeyControls(SupportTicket newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetTicket(newEntity.Id);

            Id = result.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            PunchInCommand.IsEnabled = true;

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.SupportTicketId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

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
                .Include(p => p.SupportTicketUsers)
                .ThenInclude(p => p.User)
                .Include(p => p.Errors)
                .ThenInclude(p => p.Error)
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
            TicketUserGridManager.LoadGrid(entity.SupportTicketUsers);
            TicketErrorGridManager.LoadGrid(entity.Errors);
            MinutesSpent = entity.MinutesSpent;
            ContactName = entity.ContactName;
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
                    ContactName = customer.ContactName;
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
                ContactName = ContactName,
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

        protected override bool ValidateEntity(SupportTicket entity)
        {
            if (!TicketErrorGridManager.ValidateGrid())
            {
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override void ClearData()
        {
            _loading = true;
            KeyAutoFillValue = null;
            Id = 0;
            CustomerAutoFillValue = DefaultCustomerAutoFillValue;
            CreateDate = DateTime.Now;
            PhoneNumber = string.Empty;
            ContactName = string.Empty;
            CreateUserAutoFillValue = AppGlobals.LoggedInUser.GetAutoFillValue();
            ProductAutoFillValue = null;
            AssignedUserAutoFillValue = null;
            ClosedDate = null;
            Notes = null;
            PunchInCommand.IsEnabled = false;
            LoadCustomer();
            TicketUserGridManager.SetupForNewRecord();
            TicketErrorGridManager.SetupForNewRecord();
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            MinutesSpent = 0;
            _loading = false;
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh);
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

            if (result)
            {
                var errorsTable = context.GetTable<SupportTicketError>();
                var oldErrors = errorsTable
                    .Where(p => p.SupportTicketId == Id);

                if (oldErrors.Any())
                {
                    context.RemoveRange(oldErrors);
                }

                var list = TicketErrorGridManager.GetEntityList();
                foreach (var supportTicketError in list)
                {
                    supportTicketError.SupportTicketId = entity.Id;
                }
                context.AddRange(list);
                result = context.Commit("Saving Errors");
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
                var errorsTable = context.GetTable<SupportTicketError>();
                var oldErrors = errorsTable
                    .Where(p => p.SupportTicketId == Id);

                if (oldErrors.Any())
                {
                    context.RemoveRange(oldErrors);
                }

                var usersQuery = context.GetTable<SupportTicketUser>();
                var users = usersQuery.Where(p => p.SupportTicketId == Id);
                context.RemoveRange(users);

                result = context.DeleteEntity(entity, "Deleting Ticket");
            }
            return result;
        }

        private void PunchIn()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClocksTable = context.GetTable<TimeClock>();
            var timeClock = timeClocksTable
                .Include(p => p.User)
                .FirstOrDefault(p => p.SupportTicketId == Id
                                     && p.PunchOutDate == null
                                     && p.UserId != AppGlobals.LoggedInUser.Id);

            if (timeClock != null)
            {
                var message =
                    $"The User {timeClock.User.Name} is already punched in to this Support Ticket.  Punch Denied.";
                var caption = "Punch Denied";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return;
            }


            var table = context.GetTable<SupportTicketUser>();
            var user = table.FirstOrDefault(p => p.SupportTicketId == Id
                                                 && p.UserId == AppGlobals.LoggedInUser.Id);
            if (user == null)
            {
                user = new SupportTicketUser()
                {
                    SupportTicketId = Id,
                    UserId = AppGlobals.LoggedInUser.Id,
                };
                context.AddRange(new List<SupportTicketUser>
                {
                    user
                });
                if (!context.Commit("Adding Ticket User"))
                {
                    return;
                }
                user.User = AppGlobals.LoggedInUser;
                TicketUserGridManager.AddUserRow(user);
            }

            var ticket = GetTicket(Id);
            AppGlobals.MainViewModel.PunchIn(ticket);
        }
        public void RefreshCost(List<SupportTicketUser> users)
        {
            TicketUserGridManager.RefreshCost(users);
            GetTotals();
        }
        public void RefreshCost(SupportTicketUser ticketUser)
        {
            TicketUserGridManager.RefreshCost(ticketUser);
            GetTotals();
        }

        private void GetTotals()
        {
            TicketUserGridManager.GetTotals(out var minutesSpent, out var total);
            MinutesSpent = minutesSpent;
            TotalCost = total;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        public void Recalc()
        {

        }
    }
}
