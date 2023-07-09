using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
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
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;
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
                View.RefreshView();
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
                View.RefreshView();
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

        private LookupCommand _orderLookupCommand;

        public LookupCommand OrderLookupCommand
        {
            get => _orderLookupCommand;
            set
            {
                if (_orderLookupCommand == value)
                    return;

                _orderLookupCommand = value;
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

        private LookupCommand _computerLookupCommand;

        public LookupCommand ComputerLookupCommand
        {
            get => _computerLookupCommand;
            set
            {
                if (_computerLookupCommand == value)
                    return;

                _computerLookupCommand = value;
                OnPropertyChanged(null, false);
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

        private LookupCommand _supportTicketLookupCommand;

        public LookupCommand SupportTicketLookupCommand
        {
            get => _supportTicketLookupCommand;
            set
            {
                if (_supportTicketLookupCommand == value)
                    return;

                _supportTicketLookupCommand = value;
                OnPropertyChanged(null, false);
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

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public RelayCommand AddModifyOrderLookupCommand { get; set; }

        public RelayCommand AddModifyComputerLookupCommand { get; set; }

        public RelayCommand AddModifySupportTicketCommand { get; set; }

        public RelayCommand AddSupportCommand { get; set; }

        public new ICustomerView View { get; private set; }

        public CustomerViewModel()
        {
            TimeZoneAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TimeZoneId));
            TerritoryAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TerritoryId));

            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);

            ProductManager = new CustomerProductManager(this);

            CustomerUserGridManager = new CustomerCostManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.CustomerProduct);
            TablesToDelete.Add(AppGlobals.LookupContext.CustomerUser);

            OrderLookupDefinition = AppGlobals.LookupContext.OrderLookup.Clone();
            OrderLookupDefinition.InitialOrderByField = AppGlobals.LookupContext
                .Order.GetFieldDefinition(p => p.Id);

            AddModifyOrderLookupCommand = new RelayCommand(AddModifyOrder);

            AddModifyComputerLookupCommand = new RelayCommand(AddModifyComputer);

            AddModifySupportTicketCommand = new RelayCommand(AddModifySupportTicket);

            AddSupportCommand = new RelayCommand((() =>
            {
                SupportMinutesLeft += NewSupportTime;
            }));

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            ComputerLookup = AppGlobals.LookupContext.CustomerComputerLookup.Clone();

            SupportTicketLookup = AppGlobals.LookupContext.SupportTicketLookup.Clone();
            SupportTicketLookup.InitialOrderByField = AppGlobals.LookupContext
                .SupportTicket.GetFieldDefinition(p => p.Id);

        }

        protected override void Initialize()
        {
            if (base.View is ICustomerView customerView)
            {
                View = customerView;
            }
            base.Initialize();
        }

        protected override Customer PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetCustomer(newEntity.Id);

            Id = newEntity.Id;
            KeyAutoFillValue = result.GetAutoFillValue();

            OrderLookupDefinition.FilterDefinition.ClearFixedFilters();
            OrderLookupDefinition.FilterDefinition.AddFixedFilter(p => p.CustomerId, Conditions.Equals, Id);
            OrderLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.CustomerId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            ComputerLookup.FilterDefinition.ClearFixedFilters();
            ComputerLookup.FilterDefinition.AddFixedFilter(p => p.CustomerId, Conditions.Equals, Id);
            ComputerLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            SupportTicketLookup.FilterDefinition.ClearFixedFilters();
            SupportTicketLookup.FilterDefinition.AddFixedFilter(p => p.CustomerId, Conditions.Equals, Id);
            SupportTicketLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

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
                .Include(p => p.Users)
                .ThenInclude(p => p.User)
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
            WebAddress = entity.WebAddress;
            ProductManager.LoadGrid(entity.CustomerProducts);
            CustomerUserGridManager.LoadGrid(entity.Users);
            SupportMinutesLeft = entity.SupportMinutesPurchased;
            MinutesSpent = entity.MinutesSpent;
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.MinutesCost;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            SupportMinutesSpent = entity.SupportMinutesSpent.GetValueOrDefault();
            SupportCost = entity.SupportCost.GetValueOrDefault();
            TotalSales = entity.TotalSales;
            Notes = entity.Notes;
        }

        protected override Customer GetEntityData()
        {
            double? supportMinutesSpent = null;
            if (Id > 0)
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<Customer>();
                var oldCustomer = table
                    .FirstOrDefault(p => p.Id == Id);
                if (oldCustomer != null)
                {
                    supportMinutesSpent = oldCustomer.SupportMinutesSpent;
                    //result.MinutesSpent = oldCustomer.MinutesSpent;
                    //result.Cost = oldCustomer.Cost;

                }
            }

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
                WebAddress = WebAddress,
                Notes = Notes,
                SupportMinutesPurchased = SupportMinutesLeft,
                SupportMinutesSpent = supportMinutesSpent,
                SupportCost = SupportCost,
                TotalSales = TotalSales,
            };
            return result;
        }

        protected override bool ValidateEntity(Customer entity)
        {
            var result = base.ValidateEntity(entity);
            if (result)
            {
                if (!ProductManager.ValidateGrid())
                {
                    result = false;
                }
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
            ProductManager.SetupForNewRecord();
            CustomerUserGridManager.SetupForNewRecord();
            OrderLookupCommand = GetLookupCommand(LookupCommands.Clear);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            ComputerLookupCommand = GetLookupCommand(LookupCommands.Clear);
            SupportTicketLookupCommand = GetLookupCommand(LookupCommands.Clear);
            SupportMinutesLeft = null;
            Notes = null;
            TotalCost = 0;
            MinutesSpent = 0;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            SupportMinutesSpent = 0;
            SupportCost = 0;
            TotalSales = 0;
            View.RefreshView();
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
            var context = AppGlobals.DataRepository.GetDataContext();
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

        private void GetTotals()
        {
            CustomerUserGridManager.GetTotals(out var minutesSpent, out var total);
            MinutesSpent = minutesSpent;
            TotalCost = total;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        private void Recalc()
        {
            var lookupFilter = ViewLookupDefinition.Clone();
            if (!View.SetupRecalcFilter(lookupFilter))
                return;
            var result = View.StartRecalcProcedure(lookupFilter);
            if (!result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Customer Recalculating", RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupToFilter)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = AppGlobals.DataRepository.GetDataContext();
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
                        , currentCustomer);

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
                if (!context.Commit("Recalculating Finished"))
                {
                    result = DbDataProcessor.LastException;
                }
            }

            DbDataProcessor.DontDisplayExceptions = false;
            return result;
        }

        private string ProcessCurrentCustomer(
            PrimaryKeyValue primaryKeyValue
            , DataAccess.IDbContext context, int totalCustomers
            , int currentCustomerIndex)
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
                        , context);

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
            , DataAccess.IDbContext context)
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
                    , customerUsers);

                if (!updateResult.IsNullOrEmpty())
                {
                    return updateResult;
                }
            }
            if (!context.SaveNoCommitEntity(currentCustomer, "Saving Customer"))
            {
                return DbDataProcessor.LastException;
            }

            if (currentCustomer.Id == Id)
            {
                RefreshCost(customerUsers);
            }
            return string.Empty;
        }

        private static string UpdateCustomerTimeClockValues(
            Customer currentCustomer
            , IQueryable<TimeClock> timeClocksTable
            , IQueryable<User> usersTable
            , IDbContext context
            , int timeClockUser
            , List<CustomerUser> customerUsers)
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

                if (!context.SaveNoCommitEntity(currentCustomer, "Saving Customer User"))
                {
                    result = DbDataProcessor.LastException;
                    return result;
                }
            }
            currentCustomer.MinutesSpent += customerUser.MinutesSpent;
            currentCustomer.MinutesCost += customerUser.Cost;

            return result;
        }


        private static void UpdateCustomerUserCost(IQueryable<User> usersTable, CustomerUser customerUser
            , IQueryable<TimeClock> timeClocksTable
            , Customer customer)
        {
            var user = usersTable.FirstOrDefault(p => p.Id == customerUser.UserId);
            var customerUserMinutes = timeClocksTable
                .Where(p => p.ErrorId == customer.Id
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
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                OrderLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        private void AddModifyComputer()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ComputerLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        private void AddModifySupportTicket()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                SupportTicketLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }
    }
}
