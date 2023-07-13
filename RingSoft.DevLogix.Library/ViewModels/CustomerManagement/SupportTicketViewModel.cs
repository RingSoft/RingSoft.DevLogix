﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public interface ISupportTicketView : IDbMaintenanceView
    {
        bool SetupRecalcFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentCustomer, int totalCustomers, string currentCustomerText);

    }

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

        private SupportTicketErrorManager _ticketErrorGridManager;

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

        public new ISupportTicketView View { get; private set; }

        private bool _loading;

        public SupportTicketViewModel()
        {
            AppGlobals.MainViewModel.SupportTicketViewModels.Add(this);
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

            //var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            //timeClockLookup.Include(p => p.User)
            //    .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            //TimeClockLookup = timeClockLookup;
            //TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
            TimeClockLookup = AppGlobals.LookupContext.TimeClockLookup.Clone();
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;

            PunchInCommand = new RelayCommand(PunchIn);
            RecalcCommand = new RelayCommand(Recalc);
        }

        protected override void Initialize()
        {
            if (base.View is ISupportTicketView supportTicketView)
            {
                View = supportTicketView;
            }

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

        protected override SupportTicket PopulatePrimaryKeyControls(SupportTicket newEntity,
            PrimaryKeyValue primaryKeyValue)
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
            Notes = entity.Notes;
            TicketUserGridManager.LoadGrid(entity.SupportTicketUsers);
            TicketErrorGridManager.LoadGrid(entity.Errors);
            MinutesSpent = entity.MinutesSpent;
            ContactName = entity.ContactName;

            _loading = false;
            GetTotals();
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
            var minutesSpent = (double)0;

            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<SupportTicket>();
            var existTicket = table
                .FirstOrDefault(p => p.Id == Id);
            if (existTicket !=  null)
            {
                minutesSpent = existTicket.MinutesSpent;
            }

            var result = new SupportTicket
            {
                Id = Id,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                CreateDate = CreateDate.ToUniversalTime(),
                PhoneNumber = PhoneNumber,
                CreateUserId = CreateUserAutoFillValue.GetEntity<User>().Id,
                ProductId = ProductAutoFillValue.GetEntity<Product>().Id,
                AssignedToUserId = AssignedUserAutoFillValue.GetEntity<User>().Id,
                Notes = Notes,
                ContactName = ContactName,
                MinutesSpent = minutesSpent,
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
            GetTotals();
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
                    , "Support Ticket Recalculating"
                    , RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupToFilter, AppProcedure appProcedure)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = AppGlobals.DataRepository.GetDataContext();
            DbDataProcessor.DontDisplayExceptions = true;

            var totalTickets = lookupData.GetRecordCount();
            var currentTicket = 1;

            lookupData.PrintOutput += (sender, e) =>
            {
                foreach (var primaryKeyValue in e.Result)
                {
                    var processResult = ProcessCurrentTicket(
                        primaryKeyValue
                        , context
                        , totalTickets
                        , currentTicket
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

        private string ProcessCurrentTicket(
            PrimaryKeyValue primaryKeyValue
            , DataAccess.IDbContext context, int totalCustomers
            , int currentTicketIndex
            , AppProcedure procedure)
        {
            var ticketsTable = context.GetTable<SupportTicket>();
            var usersTable = context.GetTable<User>();
            var timeClocksTable = context.GetTable<TimeClock>();

            var currentTicket = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            if (currentTicket != null)
            {
                currentTicket = ticketsTable
                    .Include(p => p.SupportTicketUsers)
                    .ThenInclude(p => p.User)
                    .FirstOrDefault(p => p.Id == currentTicket.Id);

                if (currentTicket != null)
                {
                    var updateResult = UpdateTicketValues(
                        totalCustomers
                        , currentTicketIndex
                        , currentTicket
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

        private string UpdateTicketValues(
      int totalTickets
    , int currentTicketIndex
    , SupportTicket currentTicket
    , IQueryable<TimeClock> timeClocksTable
    , IQueryable<User> usersTable
    , DataAccess.IDbContext context
    , AppProcedure procedure)
        {
            View.UpdateRecalcProcedure(currentTicketIndex, totalTickets, currentTicket.TicketId);
            var ticketUsers = new List<SupportTicketUser>(currentTicket.SupportTicketUsers);
            currentTicket.MinutesSpent = 0;
            currentTicket.Cost = 0;
            var timeClockUsers = timeClocksTable
                .Where(p => p.SupportTicketId == currentTicket.Id)
                .Select(p => p.UserId)
                .Distinct();

            foreach (var timeClockUser in timeClockUsers)
            {
                var updateResult = UpdateTicketTimeClockValues(
                    currentTicket
                    , timeClocksTable
                    , usersTable
                    , context
                    , timeClockUser
                    , ticketUsers
                    , procedure);

                if (!updateResult.IsNullOrEmpty())
                {
                    return updateResult;
                }
            }

            if (!context.SaveNoCommitEntity(currentTicket, "Saving Support Ticket", true))
            {
                return GblMethods.LastError;
            }

            if (currentTicket.Id == Id)
            {
                RefreshCost(ticketUsers);
                TotalCost = currentTicket.Cost.GetValueOrDefault();
                MinutesSpent = currentTicket.MinutesSpent;
            }
            return string.Empty;
        }
        private static string UpdateTicketTimeClockValues(
              SupportTicket currentTicket
            , IQueryable<TimeClock> timeClocksTable
            , IQueryable<User> usersTable
            , IDbContext context
            , int timeClockUser
            , List<SupportTicketUser> ticketUsers
            , AppProcedure procedure)
        {
            var result = string.Empty;
            var ticketUser = currentTicket.SupportTicketUsers.FirstOrDefault(p => p.UserId == timeClockUser);
            if (ticketUser == null)
            {
                ticketUser = new SupportTicketUser()
                {
                    SupportTicketId = currentTicket.Id,
                    UserId = timeClockUser
                };
                UpdateTicketUserCost(usersTable, ticketUser, timeClocksTable, currentTicket);
                context.AddRange(new List<SupportTicketUser>()
                {
                    ticketUser
                });
                ticketUsers.Add(ticketUser);
            }
            else
            {
                UpdateTicketUserCost(usersTable, ticketUser, timeClocksTable, currentTicket);

                if (!context.SaveNoCommitEntity(currentTicket, "Saving Customer User", true))
                {
                    result = GblMethods.LastError;
                    return result;
                }
            }

            currentTicket.MinutesSpent += ticketUser.MinutesSpent;
            currentTicket.Cost += ticketUser.Cost;

            return result;
        }
        private static void UpdateTicketUserCost(IQueryable<User> usersTable, SupportTicketUser ticketUser
            , IQueryable<TimeClock> timeClocksTable
            , SupportTicket ticket)
        {
            var user = usersTable.FirstOrDefault(p => p.Id == ticketUser.UserId);
            var ticketUserMinutes = timeClocksTable
                .Where(p => p.SupportTicketId == ticket.Id
                            && p.UserId == ticketUser.UserId)
                .ToList()
                .Sum(p => p.MinutesSpent);
            if (ticketUserMinutes != null)
            {
                ticketUser.MinutesSpent = ticketUserMinutes.Value;
                ticketUser.Cost = Math.Round((ticketUser.MinutesSpent / 60) * user.HourlyRate, 2);
            }
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.SupportTicketViewModels.Remove(this);
            base.OnWindowClosing(e);
        }
    }
}
