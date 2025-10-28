using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.LookupModel.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public interface ICustomerView : IDbMaintenanceView
    {
        void RefreshView();

        bool SetupRecalcFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentCustomer, int totalCustomers, string currentCustomerText);
    }

    public class CustomerViewModel : DbMaintenanceViewModel<Customer>
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

        private AutoFillSetup _statusAutoFillSetup;

        public AutoFillSetup StatusAutoFillSetup
        {
            get => _statusAutoFillSetup;
            set
            {
                if (_statusAutoFillSetup == value)
                    return;

                _statusAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _statusAutoFillValue;

        public AutoFillValue StatusAutoFillValue
        {
            get => _statusAutoFillValue;
            set
            {
                if (_statusAutoFillValue == value)
                    return;

                _statusAutoFillValue = value;
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
                UpdateCurrentCustomerTime();
                OnPropertyChanged();
            }
        }

        private string _currentCustomerTime;

        public string CurrentCustomerTime
        {
            get { return _currentCustomerTime; }
            set
            {
                if (_currentCustomerTime == value)
                    return;

                _currentCustomerTime = value;
                OnPropertyChanged(raiseDirtyFlag:false);
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
                RefreshView();
            }
        }

        private string? _webAddress;

        public string? WebAddress
        {
            get => _webAddress;
            set
            {
                if (_webAddress == value)
                    return;

                _webAddress = value;
                OnPropertyChanged();
                RefreshView();
            }
        }

        private double? _supportMinutesLeft;

        public double? SupportMinutesLeft
        {
            get => _supportMinutesLeft;
            set
            {
                if (_supportMinutesLeft == value)
                    return;

                _supportMinutesLeft = value;
                OnPropertyChanged();
            }
        }

        private string _supportTimeSpentText;

        public string SupportTimeSpentText
        {
            get => _supportTimeSpentText;
            set
            {
                if (_supportTimeSpentText == value)
                    return;

                _supportTimeSpentText = value;
                OnPropertyChanged(null, false);
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

        private CustomerCostManager _customerUserGridManager;

        public CustomerCostManager CustomerUserGridManager
        {
            get => _customerUserGridManager;
            set
            {
                if (_customerUserGridManager == value)
                    return;

                _customerUserGridManager = value;
                OnPropertyChanged();
            }
        }


        private LookupDefinition<OrderLookup, Order> _orderLookupDefinition;

        public LookupDefinition<OrderLookup, Order> OrderLookupDefinition
        {
            get => _orderLookupDefinition;
            set
            {
                if (_orderLookupDefinition == value)
                    return;

                _orderLookupDefinition = value;
                OnPropertyChanged();
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

        private LookupDefinition<CustomerComputerLookup, CustomerComputer> _computerLookup;

        public LookupDefinition<CustomerComputerLookup, CustomerComputer> ComputerLookup
        {
            get => _computerLookup;
            set
            {
                if (_computerLookup == value)
                    return;

                _computerLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<SupportTicketLookup, SupportTicket> _supportTicketLookup;

        public LookupDefinition<SupportTicketLookup, SupportTicket> SupportTicketLookup
        {
            get => _supportTicketLookup;
            set
            {
                if (_supportTicketLookup == value)
                    return;

                _supportTicketLookup = value;
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

        private double _supportCost;

        public double SupportCost
        {
            get => _supportCost;
            set
            {
                if (_supportCost == value)
                    return;

                _supportCost = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _totalSales;
        private double _supportMinutesSpent;

        public double TotalSales
        {
            get => _totalSales;
            set
            {
                if (_totalSales == value)
                    return;

                _totalSales = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _newSupportTime;

        public double NewSupportTime
        {
            get => _newSupportTime;
            set
            {
                if (_newSupportTime == value)
                    return;

                _newSupportTime = value;
                OnPropertyChanged(null, false);
            }
        }

        private double _salesDifference;

        public double SalesDifference
        {
            get => _salesDifference;
            set
            {
                if (_salesDifference == value)
                    return;

                _salesDifference = value;
                OnPropertyChanged(null, false);
            }
        }


        public double MinutesSpent { get; private set; }

        public double SupportMinutesSpent
        {
            get => _supportMinutesSpent;
            private set
            {
                _supportMinutesSpent = value;
                SupportTimeSpentText = AppGlobals.MakeTimeSpent(SupportMinutesSpent);
            }
        }

        private DateTime? _lastContactDate;

        public DateTime? LastContactDate
        {
            get => _lastContactDate;
            set
            {
                if (_lastContactDate == value)
                    return;

                _lastContactDate = value;
                OnPropertyChanged(null, false);
            }
        }

        #endregion

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public RelayCommand AddModifyOrderLookupCommand { get; set; }

        public RelayCommand AddModifyComputerLookupCommand { get; set; }

        public RelayCommand AddModifySupportTicketCommand { get; set; }

        public RelayCommand AddSupportCommand { get; set; }

        public UiCommand SendEmailUiCommand { get; } = new UiCommand();

        public UiCommand ClickWebUiCommand { get; } = new UiCommand();

        public UiCommand PhoneUiCommand { get; } = new UiCommand();

        public new ICustomerView View { get; private set; }

        private bool _loading;
        private TimeZone _currentTimeZone;
        private Timer _timeZoneTimer = new Timer(1000);

        public CustomerViewModel()
        {
            AppGlobals.MainViewModel.CustomerViewModels.Add(this);
            TimeZoneAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TimeZoneId));
            TerritoryAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TerritoryId));
            StatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.StatusId));


            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);

            ProductManager = new CustomerProductManager(this);
            CustomerUserGridManager = new CustomerCostManager(this);

            RegisterGrid(ProductManager);
            RegisterGrid(CustomerUserGridManager, true);

            AddModifyOrderLookupCommand = new RelayCommand(AddModifyOrder);

            AddModifyComputerLookupCommand = new RelayCommand(AddModifyComputer);

            AddModifySupportTicketCommand = new RelayCommand(AddModifySupportTicket);

            AddSupportCommand = new RelayCommand((() =>
            {
                if (SupportMinutesLeft == null)
                {
                    SupportMinutesLeft = 0;
                }
                SupportMinutesLeft += NewSupportTime;
            }));

            TimeClockLookup = AppGlobals.LookupContext.TimeClockTabLookup.Clone();
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            ComputerLookup = AppGlobals.LookupContext.CustomerComputerLookup.Clone();

            SupportTicketLookup = AppGlobals.LookupContext.SupportTicketLookup.Clone();
            SupportTicketLookup.InitialOrderByType = OrderByTypes.Descending;

            OrderLookupDefinition = AppGlobals.LookupContext.OrderLookup.Clone();
            OrderLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            RegisterLookup(OrderLookupDefinition);
            RegisterLookup(TimeClockLookup);
            RegisterLookup(ComputerLookup);
            RegisterLookup(SupportTicketLookup);

            MapFieldToUiCommand(PhoneUiCommand
            , TableDefinition.GetFieldDefinition(p => p.Phone));

            _timeZoneTimer.Elapsed += _timeZoneTimer_Elapsed;
        }

        private void _timeZoneTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (_currentTimeZone != null)
            {
                CurrentCustomerTime = AppGlobals.GetCurrentTimezoneTime(_currentTimeZone);
            }
        }

        private void StartCurrentCustomerTimer(TimeZone timeZone)
        {
            if (timeZone != null)
            {
                _currentTimeZone = timeZone;
                _timeZoneTimer.Enabled = true;
                _timeZoneTimer.Start();
            }
            else
            {
                _timeZoneTimer.Enabled = false;
                _timeZoneTimer.Stop();
                _currentTimeZone = timeZone;
                CurrentCustomerTime = string.Empty;
            }
        }

        protected override void Initialize()
        {
            if (base.View is ICustomerView customerView)
            {
                View = customerView;
            }

            base.Initialize();
        }

        private void UpdateCurrentCustomerTime()
        {
            if (TimeZoneAutoFillValue.IsValid())
            {
                var timeZone = TimeZoneAutoFillValue.GetEntity<TimeZone>();
                timeZone = timeZone.FillOutProperties(false);
                StartCurrentCustomerTimer(timeZone);
            }
            else
            {
                StartCurrentCustomerTimer(null);
            }
        }

        protected override void PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        private static Customer? GetCustomer(int customerId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            var result = table
                .FirstOrDefault(p => p.Id == customerId);
            return result;
        }

        protected override void LoadFromEntity(Customer entity)
        {
            _loading = true;
            StatusAutoFillValue = entity.Status.GetAutoFillValue();
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
            WebAddress = entity.WebAddress;
            SupportMinutesLeft = entity.SupportMinutesPurchased;
            MinutesSpent = entity.MinutesSpent;
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.MinutesCost;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            SupportMinutesSpent = entity.SupportMinutesSpent;
            SupportCost = entity.SupportCost;
            TotalSales = entity.TotalSales;
            Notes = entity.Notes;
            LastContactDate = entity.LastContactDate;
            if (LastContactDate.HasValue)
            {
                LastContactDate = LastContactDate.Value.ToLocalTime();
            }

            _loading = false;
            UpdateTotals();
        }

        public void UpdateTotals()
        {
            if (_loading)
            {
                return;
            }

            SalesDifference = TotalSales - TotalCost;
            RefreshView();
        }

        protected override Customer GetEntityData()
        {
            var supportMinutesSpent = 0.0;
            var minutesSpent = 0.0;
            var cost = 0.0;
            DateTime? lastContactDate = null;
            if (Id > 0)
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Customer>();
                var oldCustomer = table
                    .FirstOrDefault(p => p.Id == Id);
                if (oldCustomer != null)
                {
                    supportMinutesSpent = oldCustomer.SupportMinutesSpent;
                    minutesSpent = oldCustomer.MinutesSpent;
                    cost = oldCustomer.MinutesCost;
                    lastContactDate = oldCustomer.LastContactDate;
                }
            }

            var timeZoneId = TimeZoneAutoFillValue.GetEntity<TimeZone>().Id;
            var result = new Customer
            {
                Id = Id,
                StatusId = StatusAutoFillValue.GetEntity<CustomerStatus>().Id,
                CompanyName = KeyAutoFillValue.Text,
                ContactName = ContactName,
                ContactTitle = ContactTitle,
                Address = Address,
                City = City,
                Region = Region,
                PostalCode = PostalCode,
                Country = Country,
                Phone = PhoneNumber,
                TimeZoneId = timeZoneId,
                TerritoryId = TerritoryAutoFillValue.GetEntity<Territory>().Id,
                EmailAddress = EmailAddress,
                WebAddress = WebAddress,
                Notes = Notes,
                SupportMinutesPurchased = SupportMinutesLeft,
                SupportMinutesSpent = supportMinutesSpent,
                MinutesSpent = minutesSpent,
                MinutesCost = cost,
                SupportCost = SupportCost,
                TotalSales = TotalSales,
                LastContactDate = lastContactDate,
            };

            if (result.StatusId == 0)
            {
                result.StatusId = null;
            }

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
            WebAddress = null;
            SupportMinutesLeft = null;
            Notes = null;
            TotalCost = 0;
            MinutesSpent = 0;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            SupportMinutesSpent = 0;
            SupportCost = 0;
            TotalSales = 0;
            SalesDifference = 0;
            LastContactDate = null;
            StatusAutoFillValue = null;
            RefreshView();
        }

        private void PunchIn()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<CustomerUser>();
            var user = table.FirstOrDefault(p => p.CustomerId == Id
                                                 && p.UserId == AppGlobals.LoggedInUser.Id);
            if (user == null)
            {
                user = new CustomerUser()
                {
                    CustomerId = Id,
                    UserId = AppGlobals.LoggedInUser.Id,
                };
                context.AddRange(new List<CustomerUser>
                {
                    user
                });
                if (!context.Commit("Adding Customer User"))
                {
                    return;
                }

                user.User = AppGlobals.LoggedInUser;
                CustomerUserGridManager.AddUserRow(user);
            }

            var customer = GetCustomer(Id);
            AppGlobals.MainViewModel.PunchIn(customer);
        }

        public void RefreshCost(List<CustomerUser> users)
        {
            CustomerUserGridManager.RefreshCost(users);
            GetTotals();
        }

        public void RefreshCost(CustomerUser customerUser)
        {
            CustomerUserGridManager.RefreshCost(customerUser);

            GetTotals();
        }

        public void RefreshSupport(Customer customer)
        {
            SupportMinutesSpent = customer.SupportMinutesSpent;
            SupportCost = customer.SupportCost;
        }

        public void RefreshSales(Customer customer)
        {
            TotalSales = customer.TotalSales;
            GetTotals();
        }

        public void RefreshSupportLookup()
        {
            var command = GetLookupCommand(LookupCommands.Refresh);
            SupportTicketLookup.SetCommand(command);
        }

        private void GetTotals()
        {
            CustomerUserGridManager.GetTotals(out var minutesSpent, out var total);
            MinutesSpent = minutesSpent;
            TotalCost = total;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            SalesDifference = TotalSales - TotalCost;
        }

        private void Recalc()
        {
            var lookupFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(lookupFilter))
                return;
            var result = View.StartRecalcProcedure(lookupFilter);
            if (result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "Recalculation Complete"
                    , "Recalculation Complete"
                    , RsMessageBoxIcons.Information);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(
                    result
                    , "Customer Recalculating"
                    , RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupToFilter, AppProcedure appProcedure)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = SystemGlobals.DataRepository.GetDataContext();
            DbDataProcessor.DontDisplayExceptions = true;

            var totalCustomers = lookupData.GetRecordCount();
            var currentCustomer = 1;
            lookupData.PrintOutput += (sender, e) =>
            {
                foreach (var primaryKeyValue in e.Result)
                {
                    var processResult = ProcessCurrentCustomer(
                        primaryKeyValue
                        , context
                        , totalCustomers
                        , currentCustomer
                        , appProcedure);

                    if (!processResult.IsNullOrEmpty())
                    {
                        result = processResult;
                        return;
                    }
                }
            };
            lookupData.DoPrintOutput(10);
            if (result.IsNullOrEmpty())
            {
                if (!context.Commit("Recalculating Finished", true))
                {
                    result = GblMethods.LastError;
                }
            }

            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        private string ProcessCurrentCustomer(
            PrimaryKeyValue primaryKeyValue
            , DbLookup.IDbContext context, int totalCustomers
            , int currentCustomerIndex
            , AppProcedure procedure)
        {
            var customersTable = context.GetTable<Customer>();
            var usersTable = context.GetTable<User>();
            var timeClocksTable = context.GetTable<TimeClock>();

            var currentCustomer = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            if (currentCustomer != null)
            {
                currentCustomer = customersTable
                    .Include(p => p.Users)
                    .ThenInclude(p => p.User)
                    .FirstOrDefault(p => p.Id == currentCustomer.Id);

                if (currentCustomer != null)
                {
                    var updateResult = UpdateCustomerValues(
                        totalCustomers
                        , currentCustomerIndex
                        , currentCustomer
                        , timeClocksTable
                        , usersTable
                        , context
                        , procedure);

                    if (!updateResult.IsNullOrEmpty())
                    {
                        return updateResult;
                    }
                }
            }

            return string.Empty;
        }

        private string UpdateCustomerValues(
            int totalCustomers
            , int currentCustomerIndex
            , Customer currentCustomer
            , IQueryable<TimeClock> timeClocksTable
            , IQueryable<User> usersTable
            , DbLookup.IDbContext context
            , AppProcedure procedure)
        {
            View.UpdateRecalcProcedure(currentCustomerIndex, totalCustomers, currentCustomer.CompanyName);
            var customerUsers = new List<CustomerUser>(currentCustomer.Users);
            currentCustomer.MinutesSpent = currentCustomer.MinutesCost = 0;
            var timeClockUsers = timeClocksTable
                .Where(p => p.CustomerId == currentCustomer.Id)
                .Select(p => p.UserId)
                .Distinct();

            foreach (var timeClockUser in timeClockUsers)
            {
                var updateResult = UpdateCustomerTimeClockValues(
                    currentCustomer
                    , timeClocksTable
                    , usersTable
                    , context
                    , timeClockUser
                    , customerUsers
                    , procedure);

                if (!updateResult.IsNullOrEmpty())
                {
                    return updateResult;
                }
            }

            UpdateCustomerSupportValues(currentCustomer, timeClocksTable, procedure);

            UpdateCustomerSalesValues(currentCustomer, context);

            if (!context.SaveNoCommitEntity(currentCustomer, "Saving Customer", true))
            {
                return GblMethods.LastError;
            }

            if (currentCustomer.Id == Id)
            {
                if (currentCustomer.LastContactDate.HasValue)
                {
                    LastContactDate = currentCustomer.LastContactDate.Value.ToLocalTime();
                }

                RefreshCost(customerUsers);
                RefreshSales(currentCustomer);
                SupportMinutesSpent = currentCustomer.SupportMinutesSpent;
                SupportCost = currentCustomer.SupportCost;
                UpdateTotals();
            }

            return string.Empty;
        }

        private static void UpdateCustomerSalesValues(
            Customer currentCustomer
            , DbLookup.IDbContext context)
        {
            var ordersTable = context.GetTable<Order>();

            var orders = ordersTable
                .Where(p => p.CustomerId == currentCustomer.Id)
                .ToList();

            currentCustomer.TotalSales = orders.Sum(p => p.Total).GetValueOrDefault();
        }

        private static string UpdateCustomerTimeClockValues(
            Customer currentCustomer
            , IQueryable<TimeClock> timeClocksTable
            , IQueryable<User> usersTable
            , DbLookup.IDbContext context
            , int timeClockUser
            , List<CustomerUser> customerUsers
            , AppProcedure procedure)
        {
            var result = string.Empty;
            var customerUser = currentCustomer.Users.FirstOrDefault(p => p.UserId == timeClockUser);
            if (customerUser == null)
            {
                customerUser = new CustomerUser()
                {
                    CustomerId = currentCustomer.Id,
                    UserId = timeClockUser
                };
                UpdateCustomerUserCost(usersTable, customerUser, timeClocksTable, currentCustomer);
                context.AddRange(new List<CustomerUser>()
                {
                    customerUser
                });
                customerUsers.Add(customerUser);
            }
            else
            {
                UpdateCustomerUserCost(usersTable, customerUser, timeClocksTable, currentCustomer);

                if (!context.SaveNoCommitEntity(currentCustomer, "Saving Customer User", true))
                {
                    result = GblMethods.LastError;
                    return result;
                }
            }

            var lastCustomerTimeClock = timeClocksTable
                .OrderByDescending(p => p.PunchInDate)
                .FirstOrDefault(p => p.CustomerId == currentCustomer.Id);

            currentCustomer.MinutesSpent += customerUser.MinutesSpent;
            currentCustomer.MinutesCost += customerUser.Cost;
            if (lastCustomerTimeClock != null)
            {
                currentCustomer.LastContactDate = lastCustomerTimeClock.PunchInDate;
            }

            return result;
        }

        private static void UpdateCustomerSupportValues(
            Customer currentCustomer
            , IQueryable<TimeClock> timeClocksTable
            , AppProcedure procedure)
        {
            currentCustomer.SupportMinutesSpent = 0;
            currentCustomer.SupportCost = 0;
            var supportTimeClocks = timeClocksTable
                .Include(p => p.User)
                .Include(p => p.SupportTicket)
                .ThenInclude(p => p.Customer)
                .Where(p => p.SupportTicketId != null
                            && p.SupportTicket.CustomerId == currentCustomer.Id);

            var total = supportTimeClocks.Count();
            var index = 1;
            var intControlSetup = new IntegerEditControlSetup();
            var formattedTotal = intControlSetup.FormatValue(total);
            foreach (var supportTimeClock in supportTimeClocks)
            {
                procedure.SplashWindow.SetProgress
                    ($"Processing Time Clock {intControlSetup.FormatValue(index)}/{formattedTotal}");

                currentCustomer.SupportMinutesSpent += supportTimeClock.MinutesSpent.GetValueOrDefault();
                var hours = supportTimeClock.MinutesSpent / 60;
                currentCustomer.SupportCost += Math.Round((double)hours * supportTimeClock.User.HourlyRate, 2);
                index++;
            }
        }


        private static void UpdateCustomerUserCost(IQueryable<User> usersTable, CustomerUser customerUser
            , IQueryable<TimeClock> timeClocksTable
            , Customer customer)
        {
            var user = usersTable.FirstOrDefault(p => p.Id == customerUser.UserId);
            var customerUserMinutes = timeClocksTable
                .Where(p => p.CustomerId == customer.Id
                            && p.UserId == customerUser.UserId)
                .ToList()
                .Sum(p => p.MinutesSpent);
            if (customerUserMinutes != null)
            {
                customerUser.MinutesSpent = customerUserMinutes.Value;
                customerUser.Cost = Math.Round((customerUser.MinutesSpent / 60) * user.HourlyRate, 2);
            }
        }

        private void AddModifyOrder()
        {
            var command = GetLookupCommand(LookupCommands.AddModify);
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrderLookupDefinition.SetCommand(command);
        }

        private void AddModifyComputer()
        {
            var command = GetLookupCommand(LookupCommands.AddModify);
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ComputerLookup.SetCommand(command);
        }

        private void AddModifySupportTicket()
        {
            var command = GetLookupCommand(LookupCommands.AddModify);
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                SupportTicketLookup.SetCommand(command);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.CustomerViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh));
        }

        public void RefreshView()
        {
            if (EmailAddress.IsNullOrEmpty())
            {
                SendEmailUiCommand.Visibility = UiVisibilityTypes.Collapsed;
            }
            else
            {
                SendEmailUiCommand.Visibility = UiVisibilityTypes.Visible;
            }
            if (WebAddress.IsNullOrEmpty())
            {
                ClickWebUiCommand.Visibility = UiVisibilityTypes.Collapsed;
            }
            else
            {
                ClickWebUiCommand.Visibility = UiVisibilityTypes.Visible;
            }

            View.RefreshView();
        }
    }
}
