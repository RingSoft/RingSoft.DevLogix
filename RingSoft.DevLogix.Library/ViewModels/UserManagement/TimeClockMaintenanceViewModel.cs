using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Timers;
using IDbContext = RingSoft.DevLogix.DataAccess.IDbContext;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public enum TimeClockModes
    {
        Error = 1,
        ProjectTask = 2,
        TestingOutline = 3,
        Customer = 4,
        SupportTicket = 5,
    }
    public interface ITimeClockView : IDbMaintenanceView
    {
        void SetTimeClockMode(TimeClockModes timeClockMode);

        void SetElapsedTime();

        void FocusNotes();

        void SetDialogMode();

        bool GetManualPunchOutDate(out DateTime? punchInDate, out DateTime? punchOutDate);
    }

    public class TimeClockMaintenanceViewModel : DevLogixDbMaintenanceViewModel<TimeClock>
    {
        public override TableDefinition<TimeClock> TableDefinition => AppGlobals.LookupContext.TimeClocks;

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

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }

                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                {
                    return;
                }

                _userAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private string _keyText;

        public string KeyText
        {
            get => _keyText;
            set
            {
                if (_keyText == value)
                {
                    return;
                }

                _keyText = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _errorAutoFillSetup;

        public AutoFillSetup ErrorAutoFillSetup
        {
            get => _errorAutoFillSetup;
            set
            {
                if (_errorAutoFillSetup == value)
                {
                    return;
                }

                _errorAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _errorAutoFillValue;

        public AutoFillValue ErrorAutoFillValue
        {
            get => _errorAutoFillValue;
            set
            {
                if (_errorAutoFillValue == value)
                {
                    return;
                }

                _errorAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _projectTaskAutoFillSetup;

        public AutoFillSetup ProjectTaskAutoFillSetup
        {
            get => _projectTaskAutoFillSetup;
            set
            {
                if (_projectTaskAutoFillSetup == value)
                {
                    return;
                }

                _projectTaskAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectTaskAutoFillValue;

        public AutoFillValue ProjectTaskAutoFillValue
        {
            get => _projectTaskAutoFillValue;
            set
            {
                if (_projectTaskAutoFillValue == value)
                {
                    return;
                }

                _projectTaskAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _testingOutlineAutoFillSetup;

        public AutoFillSetup TestingOutlineAutoFillSetup
        {
            get => _testingOutlineAutoFillSetup;
            set
            {
                if (_testingOutlineAutoFillSetup == value)
                {
                    return;
                }

                _testingOutlineAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _testingOutlineAutoFillValue;

        public AutoFillValue TestingOutlineAutoFillValue
        {
            get => _testingOutlineAutoFillValue;
            set
            {
                if (_testingOutlineAutoFillValue == value)
                {
                    return;
                }

                _testingOutlineAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomerAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                {
                    return;
                }

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
                {
                    return;
                }

                _customerAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private AutoFillSetup _supportTicketAutoFillSetup;

        public AutoFillSetup SupportTicketAutoFillSetup
        {
            get => _supportTicketAutoFillSetup;
            set
            {
                if (_supportTicketAutoFillSetup == value)
                {
                    return;
                }

                _supportTicketAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _supportTicketAutoFillValue;

        public AutoFillValue SupportTicketAutoFillValue
        {
            get => _supportTicketAutoFillValue;
            set
            {
                if (_supportTicketAutoFillValue == value)
                {
                    return;
                }

                _supportTicketAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }

        private DateTime _punchInDate;

        public DateTime PunchInDate
        {
            get => _punchInDate;
            set
            {
                if (_punchInDate == value)
                {
                    return;
                }

                _punchInDate = value;
                ChangePunchDates(true);
                OnPropertyChanged();
            }
        }

        private DateTime? _punchOutDate;

        public DateTime? PunchOutDate
        {
            get => _punchOutDate;
            set
            {
                if (_punchOutDate == value)
                {
                    return;
                }

                _punchOutDate = value;
                ChangePunchDates(false);
                OnPropertyChanged();
            }
        }

        private string _notes;

        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                {
                    return;
                }

                _notes = value;
                OnPropertyChanged();
            }
        }

        private bool _isEdited;

        public bool IsEdited
        {
            get => _isEdited;
            set
            {
                if (_isEdited == value)
                    return;
                _isEdited = value;
                OnPropertyChanged();
            }
        }

        private string _elapsedTime;

        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (_elapsedTime == value)
                {
                    return;
                }
                _elapsedTime = value;
                //OnPropertyChanged(null, false);
            }
        }

        public new ITimeClockView View { get; private set; }

        public double MinutesSpent { get; private set; }

        public double OriginalMinutesSpent { get; private set; }

        public DialogInput DialogInput { get; private set; }

        public RelayCommand PunchOutCommand { get; private set; }

        public RelayCommand ManualPunchOutCommand { get; private set; }

        public UiCommand PunchOutDateUiCommand { get; }

        public double? SupportMinutesPurchased { get; private set; }

        public string? SupportTimeLeft { get; private set; }

        public double? SupportMinutesLeft
        {
            get => _supportMinutesLeft;
            private set
            {
                _supportMinutesLeft = value;
            }
        }

        public TimeClockModes TimeClockMode { get; private set; }

        public bool ManualPunchOut { get; set; }

        private DateTime _endDate;
        private Timer _timer = new Timer();
        private bool _loading;
        private bool _timerActive;
        private bool _setDirty = true;
        private double? _supportMinutesLeft;
        private DateTime? _newPunchOutDate;
        private DateTime? _newPunchInDate;
        public TimeClockMaintenanceViewModel()
        {
            PunchOutCommand = new RelayCommand((() =>
            {
                _setDirty = false;
                PunchOut();

                _setDirty = true;
            }));
            PunchOutDateUiCommand = new UiCommand();

            MapFieldToUiCommand(PunchOutDateUiCommand
                , AppGlobals.LookupContext.TimeClocks.GetFieldDefinition(p => p.PunchOutDate));
            ;

            ManualPunchOutCommand = new RelayCommand((() =>
            {
                if (View.GetManualPunchOutDate(out var punchInDate, out var punchOutDate))
                {
                    ManualPunchOut = true;
                    StopTimer();
                    PunchInDate = punchInDate.GetValueOrDefault();
                    PunchOutDate = punchOutDate;
                    if (TimeClockMode == TimeClockModes.SupportTicket
                        && SupportMinutesPurchased != 0)
                    {
                        _newPunchOutDate = punchOutDate;
                        _newPunchInDate = punchInDate;
                        SetElapsedTime(GetElapsedTime());
                    }

                    DoSave();
                    ManualPunchOut = false;
                }
            }));

        AppGlobals.MainViewModel.TimeClockMaintenanceViewModel = this;
        }

        private void SetError(Error error)
        {
            ErrorAutoFillValue = error.GetAutoFillValue();
        }

        private void SetProjectTask(ProjectTask projectTask)
        {
            ProjectTaskAutoFillValue = projectTask.GetAutoFillValue();
        }

        private void SetTestingOutline(TestingOutline testingOutline)
        {
            TestingOutlineAutoFillValue = testingOutline.GetAutoFillValue();
        }

        private void SetCustomer(Customer customer)
        {
            CustomerAutoFillValue = customer.GetAutoFillValue();
        }

        private void SetTicket(SupportTicket ticket)
        {
            SupportTicketAutoFillValue = ticket.GetAutoFillValue();
        }


        //SetTicket(ticket);
        protected override void Initialize()
        {
            _loading = true;
            ViewLookupDefinition.InitialOrderByField = TableDefinition.GetFieldDefinition(p => p.Id);

            NewButtonEnabled = false;
            var punchIn = false;
            _timer.Elapsed += (sender, args) =>
            {
                if (_timerActive)
                {
                    _endDate = DateTime.Now;
                    SetElapsedTime(GetElapsedTime());
                }
            };

            ErrorAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorId));
            ProjectTaskAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectTaskId));
            TestingOutlineAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TestingOutlineId));
            CustomerAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerId));
            SupportTicketAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SupportTicketId));

            if (base.View is ITimeClockView timeClockView)
            {
                View = timeClockView;

                if (InputParameter is DialogInput dialogInput)
                {
                    DialogInput = dialogInput;
                    NextCommand.IsEnabled = false;
                    PreviousCommand.IsEnabled = false;
                    View.SetDialogMode();
                }
            }

            UserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.UserId));
            UserAutoFillValue = UserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            PunchInDate = DateTime.Now;

            if (punchIn)
            {
                PunchIn(true);
            }

            base.Initialize();
            if (punchIn)
            {
                DoPostPunchIn();
            }

            _loading = false;
        }

        private void DoPostPunchIn()
        {
            if (TableDefinition.HasRight(RightTypes.AllowDelete))
            {
                DeleteButtonEnabled = true;
            }
        }

        public void PunchIn(Error error)
        {
            TimeClockMode = TimeClockModes.Error;
            SetError(error);
            PunchIn(true);
            DoPostPunchIn();
            View.SetTimeClockMode(TimeClockModes.Error);

        }

        public void PunchIn(ProjectTask task)
        {
            TimeClockMode = TimeClockModes.ProjectTask;
            SetProjectTask(task);
            PunchIn(true);
            DoPostPunchIn();
            View.SetTimeClockMode(TimeClockModes.ProjectTask);
        }

        public void PunchIn(TestingOutline outline)
        {
            TimeClockMode = TimeClockModes.TestingOutline;
            SetTestingOutline(outline);
            PunchIn(true);
            DoPostPunchIn();
            View.SetTimeClockMode(TimeClockModes.TestingOutline);
        }

        public void PunchIn(Customer customer)
        {
            TimeClockMode = TimeClockModes.Customer;
            SetCustomer(customer);
            PunchIn(true);
            DoPostPunchIn();
            View.SetTimeClockMode(TimeClockModes.Customer);
        }

        public void PunchIn(SupportTicket ticket)
        {

            TimeClockMode = TimeClockModes.SupportTicket;
            SetTicket(ticket);
            PunchIn(true);
            DoPostPunchIn();
            View.SetTimeClockMode(TimeClockModes.SupportTicket);
        }

        protected override TimeClock PopulatePrimaryKeyControls(TimeClock newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClock = context.GetTable<TimeClock>()
                .Include(p => p.User)
                .Include(p => p.ProjectTask)
                .Include(p => p.Error)
                .Include(p => p.TestingOutline)
                .Include(p => p.Customer)
                .Include(p => p.SupportTicket)
                .ThenInclude(p => p.Customer)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;
            if (timeClock.MinutesSpent != null) 
                OriginalMinutesSpent = timeClock.MinutesSpent.Value;
            EnablePunchOut(timeClock);
            KeyAutoFillValue = timeClock.GetAutoFillValue();
            return timeClock;
        }

        private void EnablePunchOut(TimeClock timeClock)
        {
            if (timeClock.PunchOutDate.HasValue)
            {
                ManualPunchOutCommand.IsEnabled = PunchOutCommand.IsEnabled = false;
            }
            else
            {
                if (timeClock.UserId == AppGlobals.LoggedInUser.Id)
                {
                    ManualPunchOutCommand.IsEnabled = PunchOutCommand.IsEnabled = true;
                    EnablePunchOutDate();
                }
                else if (timeClock.User.IsSupervisor())
                {
                    ManualPunchOutCommand.IsEnabled = PunchOutCommand.IsEnabled = true;
                    EnablePunchOutDate();
                }
                else
                {
                    ManualPunchOutCommand.IsEnabled = PunchOutCommand.IsEnabled = false;
                }
            }
        }

        private void EnablePunchOutDate()
        {
            ChangePunchDates(false);
        }

        protected override void LoadFromEntity(TimeClock entity)
        {
            _loading = true;
            UserAutoFillValue = entity.User.GetAutoFillValue();
            ErrorAutoFillValue = entity.Error.GetAutoFillValue();
            ProjectTaskAutoFillValue = entity.ProjectTask.GetAutoFillValue();
            TestingOutlineAutoFillValue = entity.TestingOutline.GetAutoFillValue();
            CustomerAutoFillValue = entity.Customer.GetAutoFillValue();
            SupportTicketAutoFillValue = entity.SupportTicket.GetAutoFillValue();

            var timeClockMode = TimeClockModes.Error;

            if (ErrorAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.Error;
            }
            else if (ProjectTaskAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.ProjectTask;
            }
            else if (TestingOutlineAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.TestingOutline;
            }
            else if (CustomerAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.Customer;
            }
            else if (SupportTicketAutoFillValue.IsValid())
            {
                timeClockMode = TimeClockModes.SupportTicket;
            }

            TimeClockMode = timeClockMode;
            View.SetTimeClockMode(timeClockMode);
            

            PunchInDate = entity.PunchInDate.ToLocalTime();
            PunchOutDate = entity.PunchOutDate;
            if (timeClockMode == TimeClockModes.SupportTicket)
            {
                SupportMinutesPurchased = entity
                    .SupportTicket
                    .Customer
                    .SupportMinutesPurchased;

                if (entity.Id == AppGlobals.MainViewModel.ActiveTimeClockId 
                    || AppGlobals.MainViewModel.ActiveTimeClockId == 0)
                {
                    if (!PunchOutDate.HasValue)
                    {
                        AppGlobals.MainViewModel.SupportMinutesPurchased =
                            entity.SupportTicket.Customer.SupportMinutesPurchased;
                        AppGlobals.MainViewModel.ActiveCustomerName = entity.SupportTicket.Customer.CompanyName;
                    }
                }
            }
            if (PunchOutDate.HasValue)
            {
                PunchOutDate = PunchOutDate.Value.ToLocalTime();
                _endDate = PunchOutDate.Value;
            }
            else
            {
                PunchIn(false);
            }

            EnablePunchOut(entity);


            Notes = entity.Notes;
            IsEdited = entity.AreDatesEdited;

            _loading = false;
            if (PunchOutDate.HasValue)
            {
                ChangePunchDates(false);
                IsEdited = entity.AreDatesEdited;
            }

            View.FocusNotes();
        }

        protected override bool ValidateEntity(TimeClock entity)
        {
            if (entity.PunchInDate > entity.PunchOutDate)
            {
                PunchOutDateUiCommand.SetFocus();
                var message = "The Punch In Date cannot be greater than the Punch Out Date";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail"
                    , RsMessageBoxIcons.Exclamation);
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override TimeClock GetEntityData()
        {
            if (PunchOutDate != null)
            {
                var duration = PunchOutDate.Value.Subtract(PunchInDate);
                MinutesSpent = (double)duration.TotalMinutes;
            }

            var timeClock = new TimeClock
            {
                Id = Id,
                UserId = UserAutoFillValue.GetEntity(AppGlobals.LookupContext.Users).Id,
                ErrorId = ErrorAutoFillValue.GetEntity(AppGlobals.LookupContext.Errors).Id,
                ProjectTaskId = ProjectTaskAutoFillValue.GetEntity(AppGlobals.LookupContext.ProjectTasks).Id,
                TestingOutlineId = TestingOutlineAutoFillValue.GetEntity<TestingOutline>().Id,
                CustomerId = CustomerAutoFillValue.GetEntity<Customer>().Id,
                SupportTicketId = SupportTicketAutoFillValue.GetEntity<SupportTicket>().Id,
                PunchInDate = PunchInDate.ToUniversalTime(),
                PunchOutDate = PunchOutDate,
                MinutesSpent = MinutesSpent,
                Notes = Notes,
                AreDatesEdited = IsEdited
            };
            if (KeyAutoFillValue != null)
            {
                timeClock.Name = KeyAutoFillValue.Text;
            }

            if (timeClock.PunchOutDate.HasValue)
            {
                timeClock.PunchOutDate = timeClock.PunchOutDate.Value.ToUniversalTime();
            }

            if (timeClock.ErrorId == 0)
            {
                timeClock.ErrorId = null;
            }

            if (timeClock.ProjectTaskId == 0)
            {
                timeClock.ProjectTaskId = null;
            }

            if (timeClock.TestingOutlineId == 0)
            {
                timeClock.TestingOutlineId = null;
            }

            if (timeClock.CustomerId == 0)
            {
                timeClock.CustomerId = null;
            }

            if (timeClock.SupportTicketId == 0)
            {
                timeClock.SupportTicketId = null;
            }

            return timeClock;
        }

        protected override void ClearData()
        {
        }

        protected override bool SaveEntity(TimeClock entity)
        {
            if (Id == AppGlobals.MainViewModel.ActiveTimeClockId && PunchOutDate != null)
            {
                AppGlobals.MainViewModel.SetupTimer(null, null);
            }

            int? customerId = null;
            var makeName = Id == 0 && entity.Name.IsNullOrEmpty();
            var saveChildren = entity.Id != 0;
            var context = AppGlobals.DataRepository.GetDataContext();
            var user = context.GetTable<User>().FirstOrDefault(p => p.Id == entity.UserId);
            ProjectTask projectTask = null;
            if (makeName)
            {
                entity.Name = Guid.NewGuid().ToString();
            }

            var result = context.SaveEntity(entity, "Saving Time Clock");
            if (result && makeName)
            {
                entity.Name = $"T-{entity.Id}";
                result = context.SaveEntity(entity, "Updating Name");
            }
            if (result && saveChildren)
            {
                if (entity.ProjectTaskId.HasValue)
                {
                    projectTask = context.GetTable<ProjectTask>()
                        .Include(p => p.Project)
                        .ThenInclude(p => p.ProjectUsers)
                        .Include(p => p.Project)
                        .ThenInclude(p => p.Product)
                        .FirstOrDefault(p => p.Id == entity.ProjectTaskId.Value);
                    if (projectTask != null)
                    {
                        result = UpdateProjectTask(entity, projectTask, context, user);
                        if (result && projectTask.Project.IsBillable)
                        {
                            user.BillableProjectsMinutesSpent += GetNewMinutesSpent();
                        }
                        else
                        {
                            user.NonBillableProjectsMinutesSpent += GetNewMinutesSpent();
                        }
                    }
                }
                else if (entity.ErrorId.HasValue)
                {
                    var error = context.GetTable<Error>()
                        .Include(p => p.Users)
                        .ThenInclude(p => p.User)
                        .Include(p => p.Product)
                        .FirstOrDefault(p => p.Id == entity.ErrorId.Value);
                    if (error != null)
                    {
                        result = UpdateError(entity, error, context, user);
                    }
                }
                else if (entity.TestingOutlineId.HasValue)
                {
                    var testingOutline = context.GetTable<TestingOutline>()
                        .Include(p => p.Costs)
                        .ThenInclude(p => p.User)
                        .Include(p => p.Product)
                        .FirstOrDefault(p => p.Id == entity.TestingOutlineId.Value);
                    if (testingOutline != null)
                    {
                        result = UpdateTestingOutline(entity, testingOutline, context, user);
                    }
                }
                else if (entity.CustomerId.HasValue)
                {
                    var customer = context.GetTable<Customer>()
                        .Include(p => p.Users)
                        .ThenInclude(p => p.User)
                        .FirstOrDefault(p => p.Id == entity.CustomerId.Value);
                    if (customer != null)
                    {
                        result = UpdateCustomer(entity, customer, context, user);
                    }
                }

                else if (entity.SupportTicketId.HasValue)
                {
                    var ticket = context.GetTable<SupportTicket>()
                        .Include(p => p.SupportTicketUsers)
                        .ThenInclude(p => p.User)
                        .Include(p => p.Customer)
                        .Include(p => p.Product)
                        .FirstOrDefault(p => p.Id == entity.SupportTicketId.Value);
                    if (ticket != null)
                    {
                        customerId = ticket.CustomerId;
                        result = UpdateTicket(entity, ticket, context, user);
                    }
                }

                if (result)
                {
                    result = context.SaveNoCommitEntity(user, "Saving User");
                }
            }

            if (result)
            {
                result = context.Commit("Saving Timeclock");
                if (result && saveChildren)
                {
                    var viewModels = AppGlobals.MainViewModel.UserViewModels.Where(p => p.Id == user.Id);

                    if (viewModels.Any())
                    {
                        foreach (var model in viewModels)
                        {
                            model.RefreshBillability(user);
                        }
                    }

                    if (projectTask != null)
                    {
                        var projectTaskViewModels = AppGlobals.MainViewModel.ProjectTaskViewModels
                            .Where(p => p.Id == projectTask.Id);

                        if (projectTaskViewModels.Any())
                        {
                            foreach (var model in projectTaskViewModels)
                            {
                                model.RefreshCostGrid(projectTask);
                            }
                        }

                        var projectViewModels = AppGlobals.MainViewModel.ProjectViewModels
                            .Where(p => p.Id == projectTask.ProjectId);
                        foreach (var projectViewModel in projectViewModels)
                        {
                            projectViewModel.RefreshCostGrid(projectTask.Project);
                        }
                    }
                }

            }

            if (result)
            {
                //Peter Ringering - 05/25/2023 11:09:38 AM - E-36
                var userViewModels = AppGlobals.MainViewModel.UserViewModels
                    .Where(p => p.Id == entity.UserId);
                foreach (var userViewModel in userViewModels)
                {
                    userViewModel.RefreshTimeClockLookup();
                }
                if (entity.ErrorId.HasValue)
                {
                    var errorViewModels = AppGlobals.MainViewModel.ErrorViewModels
                        .Where(p => p.Id == entity.ErrorId);
                    foreach (var errorViewModel in errorViewModels)
                    {
                        errorViewModel.RefreshTimeClockLookup();
                    }
                }

                if (entity.ProjectTaskId.HasValue)
                {
                    var projectTaskViewModels = AppGlobals.MainViewModel.ProjectTaskViewModels
                        .Where(p => p.Id == entity.ProjectTaskId.Value);
                    foreach (var projectTaskViewModel in projectTaskViewModels)
                    {
                        projectTaskViewModel.RefreshTimeClockLookup();
                    }
                    var projectViewModels = AppGlobals.MainViewModel.ProjectViewModels
                        .Where(p => projectTask != null && p.Id == projectTask.ProjectId);
                    foreach (var projectViewModel in projectViewModels)
                    {
                        projectViewModel.RefreshTimeClockLookup();
                    }

                }

                if (entity.TestingOutlineId.HasValue)
                {
                    var testingOutlineViewModels = AppGlobals.MainViewModel.TestingOutlineViewModels
                        .Where(p => p.Id == entity.TestingOutlineId.Value);
                    foreach (var outlineViewModel in testingOutlineViewModels)
                    {
                        outlineViewModel.RefreshTimeClockLookup();
                    }
                }

                if (entity.CustomerId.HasValue)
                {
                    var customerViewModels = AppGlobals.MainViewModel.CustomerViewModels
                        .Where(p => p.Id == entity.CustomerId.Value);
                    foreach (var customerViewModel in customerViewModels)
                    {
                        customerViewModel.RefreshTimeClockLookup();
                    }
                }

                if (entity.SupportTicketId.HasValue)
                {
                    var ticketViewModels = AppGlobals.MainViewModel.SupportTicketViewModels
                        .Where(p => p.Id == entity.SupportTicketId.Value);
                    foreach (var ticketViewModel in ticketViewModels)
                    {
                        ticketViewModel.RefreshTimeClockLookup();
                    }

                    if (customerId.HasValue)
                    {
                        var customerViewModels = AppGlobals.MainViewModel.CustomerViewModels
                            .Where(p => p.Id == customerId.GetValueOrDefault());

                        foreach (var customerViewModel in customerViewModels)
                        {
                            customerViewModel.RefreshSupportLookup();
                        }
                    }
                }

            }
            return result;
        }

        private bool UpdateError(TimeClock entity, Error error, IDbContext context, User user)
        {
            var result = true;
            user.ErrorsMinutesSpent += entity.MinutesSpent.Value;
            error.MinutesSpent += entity.MinutesSpent.Value;
            var errorUser = error.Users.FirstOrDefault(p => p.UserId == user.Id);
            if (errorUser != null)
            {
                errorUser.MinutesSpent += GetNewMinutesSpent();
                errorUser.Cost = Math.Round((errorUser.MinutesSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(errorUser, "Saving Error User");
            }

            if (result)
            {
                AppGlobals.CalculateError(error, error.Users.ToList());
                result = context.SaveNoCommitEntity(error, "Saving Error");
            }
            if (result)
            {
                result = UpdateProductCost(context, error.Product, entity);
            }

            var errorViewModels = AppGlobals.MainViewModel.ErrorViewModels
                .Where(p => p.Id == error.Id);
            foreach (var errorViewModel in errorViewModels)
            {
                errorViewModel.RefreshCost(errorUser);
            }

            return result;

        }

        private bool UpdateTestingOutline(TimeClock entity, TestingOutline testingOutline, IDbContext context, User user)
        {
            var result = true;
            user.TestingOutlinesMinutesSpent += entity.MinutesSpent.Value;
            testingOutline.MinutesSpent += entity.MinutesSpent.Value;
            var testingUser = testingOutline.Costs.FirstOrDefault(p => p.UserId == user.Id);
            if (testingUser != null)
            {
                testingUser.TimeSpent += GetNewMinutesSpent();
                testingUser.Cost = Math.Round((testingUser.TimeSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(testingUser, "Saving Testing Outline User");
            }

            if (result)
            {
                AppGlobals.CalculateTestingOutline(testingOutline, testingOutline.Costs.ToList());
                result = context.SaveNoCommitEntity(testingOutline, "Saving Testing Outline");
            }
            if (result)
            {
                result = UpdateProductCost(context, testingOutline.Product, entity);
            }


            var testingOutlineViewModels = AppGlobals.MainViewModel.TestingOutlineViewModels.Where(p => p.Id == testingOutline.Id);
            foreach (var testingOutlineViewModel in testingOutlineViewModels)
            {
                testingOutlineViewModel.RefreshCost(testingUser);
            }
            return result;

        }

        private bool UpdateCustomer(TimeClock entity, Customer customer, IDbContext context, User user)
        {
            var result = true;
            user.CustomerMinutesSpent += entity.MinutesSpent.Value;
            customer.MinutesSpent += entity.MinutesSpent.Value;

            var customerUser = customer.Users.FirstOrDefault(p => p.UserId == user.Id);
            if (customerUser != null)
            {
                customerUser.MinutesSpent += GetNewMinutesSpent();
                customerUser.Cost = Math.Round((customerUser.MinutesSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(customerUser, "Saving Customer User");
            }

            if (result)
            {
                AppGlobals.CalculateCustomer(customer, customer.Users.ToList());
                customer.LastContactDate = PunchInDate.ToUniversalTime();
                result = context.SaveNoCommitEntity(customer, "Saving Customer");
            }

            var customerViewModels = AppGlobals.MainViewModel.CustomerViewModels
                .Where(p => p.Id == customer.Id);
            foreach (var customerViewModel in customerViewModels)
            {
                if (customer.LastContactDate.HasValue)
                {
                    customerViewModel.LastContactDate = customer.LastContactDate.Value.ToLocalTime();
                }
                customerViewModel.RefreshCost(customerUser);
            }
            return result;

        }

        private bool UpdateTicket(TimeClock entity, SupportTicket ticket, IDbContext context, User user)
        {
            var result = true;
            user.SupportTicketsMinutesSpent += entity.MinutesSpent.Value;
            ticket.MinutesSpent += entity.MinutesSpent.Value;
            if (ticket.Customer.SupportMinutesSpent == null)
            {
                ticket.Customer.SupportMinutesSpent = 0;
            }

            ticket.Customer.SupportMinutesSpent += entity.MinutesSpent.Value;
            ticket.Customer.SupportMinutesPurchased -= entity.MinutesSpent.Value;
            SupportMinutesLeft = ticket.Customer.SupportMinutesPurchased;
            
            var ticketUser = ticket.SupportTicketUsers.FirstOrDefault(p => p.UserId == user.Id);
            if (ticketUser != null)
            {
                ticketUser.MinutesSpent += GetNewMinutesSpent();
                ticketUser.Cost = Math.Round((ticketUser.MinutesSpent / 60) * user.HourlyRate, 2);
                ticket.Customer.SupportCost += ticketUser.Cost;
                result = context.SaveNoCommitEntity(ticketUser, "Saving Support Tickets User");
            }

            if (result)
            {
                AppGlobals.CalculateTicket(ticket, ticket.SupportTicketUsers.ToList());
                result = context.SaveNoCommitEntity(ticket, "Saving Support Ticket");
            }

            if (result)
            {
                result = UpdateProductCost(context, ticket.Product, entity);
            }
            var ticketViewModels = AppGlobals.MainViewModel.SupportTicketViewModels
                .Where(p => p.Id == ticket.Id);
            foreach (var ticketViewModel in ticketViewModels)
            {
                ticketViewModel.RefreshCost(ticketUser);
            }

            var customerViewModels = AppGlobals.MainViewModel.CustomerViewModels
                .Where(p => p.Id == ticket.CustomerId);
            foreach (var customerViewModel in customerViewModels)
            {
                customerViewModel.RefreshSupport(ticket.Customer);
            }

            return result;

        }

        private bool UpdateProjectTask(TimeClock entity, ProjectTask projectTask, IDbContext context, User user)
        {
            var result = true;
            projectTask.MinutesSpent += entity.MinutesSpent.Value;
            var cost = (projectTask.MinutesSpent / 60) * projectTask.HourlyRate;
            projectTask.Cost = cost;

            var projectUser = projectTask.Project.ProjectUsers.FirstOrDefault(p => p.UserId == user.Id);
            if (projectUser != null)
            {
                projectUser.MinutesSpent += GetNewMinutesSpent();
                projectUser.Cost = Math.Round((projectUser.MinutesSpent / 60) * user.HourlyRate, 2);
                result = context.SaveNoCommitEntity(projectUser, "Saving Project User");
            }

            if (result)
            {
                
                AppGlobals.CalculateProject(projectTask.Project, projectTask.Project.ProjectUsers.ToList());
                result = context.SaveNoCommitEntity(projectTask.Project, "Saving Project");

                if (result)
                {
                    result = UpdateProductCost(context, projectTask.Project.Product, entity);
                }
            }
            if (result)
            {
                result = context.SaveNoCommitEntity(projectTask, "Saving Project Task");
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            if (Id == AppGlobals.MainViewModel.ActiveTimeClockId)
            {
                AppGlobals.MainViewModel.SetupTimer(null, null);
                StopTimer();
            }

            var context = AppGlobals.DataRepository.GetDataContext();
            var timeClock = context.GetTable<TimeClock>().FirstOrDefault(p => p.Id == Id);
            var result = context.DeleteEntity(timeClock, "Deleting Time Clock");

            if (result)
            {
                if (context.GetTable<TimeClock>().Any())
                {
                    OnGotoNextButton();
                }
                else
                {
                    Processor.CloseWindow();
                }
            }

            return result;
        }

        public void PunchIn(bool save)
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
            
            _timerActive = true;
            if (save)
            {
                var timeClock = GetEntityData();
                if (SaveEntity(timeClock))
                {
                    double? supportMinutesPurchased =null;
                    
                    if (timeClock.SupportTicketId != null)
                    {
                        var context1 = AppGlobals.DataRepository.GetDataContext();
                        var ticketsTable = context1.GetTable<SupportTicket>();
                        SupportTicket? ticket = null;

                        if (ticketsTable != null && timeClock.SupportTicketId.HasValue)
                        {
                            ticket = ticketsTable
                                .Include(p => p.Customer)
                                .FirstOrDefault(p => p.Id == timeClock.SupportTicketId);
                        }

                        if (ticket != null)
                        {
                            supportMinutesPurchased = ticket.Customer.SupportMinutesPurchased;
                        }
                    }
                    var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(timeClock);
                    SelectPrimaryKey(primaryKey);
                    GblMethods.DoRecordLock(primaryKey);
                    LockDate = DateTime.Now;
                    
                    KeyAutoFillValue = timeClock.GetAutoFillValue();
                    Id = timeClock.Id;
                    AppGlobals.MainViewModel.SetupTimer(timeClock, TimeClockMode);
                    RecordDirty = false;
                }
                var context = AppGlobals.DataRepository.GetDataContext();
                var usersQuery = context.GetTable<User>();
                if (usersQuery != null)
                {
                    var user = usersQuery.FirstOrDefault(p => p.Id == timeClock.UserId);
                    //if (user != null && user.ClockDate == null)
                    //{
                    //    user.ClockDate = timeClock.PunchInDate;
                    //}
                    //context.SaveEntity(user, "Updating Clock Date");
                    AppGlobals.ClockInUser(context, user);
                }

                MaintenanceMode = DbMaintenanceModes.EditMode;
            }
        }

        private string GetElapsedTime()
        {
            var result = string.Empty;

            var duration = _endDate.Subtract(PunchInDate);
            result = $"{duration.Days.ToString("00")} {duration.ToString("hh\\:mm\\:ss")}";

            return result;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            StopTimer();
            if (DialogInput != null && PunchOutDate != null)
            {
                DialogInput.DialogResult = true;
            }

            AppGlobals.MainViewModel.TimeClockMaintenanceViewModel = null;
            base.OnWindowClosing(e);
        }

        private void PunchOut()
        {
            //_loading = true;
            PunchOutSave();
            View.FocusNotes();
            //_loading = false;
        }

        private void PunchOutSave(bool save = true)
        {
            if (!PunchOutDate.HasValue)
            {
                PunchOutDate = DateTime.Now;
            }

            _endDate = PunchOutDate.Value;
            StopTimer();
            if (save)
            {
                DoSave();
            }
            ManualPunchOutCommand.IsEnabled = PunchOutCommand.IsEnabled = false;
        }
        private void StopTimer()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _timerActive = false;
        }

        private void ChangePunchDates(bool fromPunchIn)
        {
            if (_loading)
            {
                return;
            }

            var elapsedTime = string.Empty;
            
            if (PunchOutDate.HasValue)
            {
                if (!fromPunchIn)
                {
                    PunchOutSave(false);
                }
            }
            else if (!fromPunchIn)
            {
                _endDate = DateTime.Now;
                PunchIn(false);
            }

            if (_setDirty)
            {
                IsEdited = true;
            }

            if (PunchOutDate.HasValue)
            {
                _endDate = PunchOutDate.Value;
                StopTimer();
                elapsedTime = GetElapsedTime();
                SetElapsedTime(elapsedTime);
            }
            else
            {
                if (_timer.Enabled)
                {
                    elapsedTime = GetElapsedTime();
                    SetElapsedTime(elapsedTime);
                }
            }
        }

        private void SetElapsedTime(string elapsedTime)
        {
            var punchInDate = _newPunchInDate;
            if (!punchInDate.HasValue)
            {
                punchInDate = PunchInDate;
            }
            SupportTimeLeft = AppGlobals.GetSupportTimeLeftTextFromDate(
                punchInDate.GetValueOrDefault()
                , SupportMinutesPurchased
                , out var supportMinutesLeft
                , _newPunchOutDate);
            ElapsedTime = elapsedTime;
            SupportMinutesLeft = supportMinutesLeft;
            View.SetElapsedTime();
        }

        public double GetNewMinutesSpent()
        {
            var result = MinutesSpent - OriginalMinutesSpent;
            return result;
        }

        private bool UpdateProductCost(IDbContext context, Product product, TimeClock timeClock)
        {
            if (product != null && timeClock != null)
            {
                var hoursSpent = timeClock.MinutesSpent / 60;
                var cost = hoursSpent * timeClock.User.HourlyRate;
                product.Cost += cost;

                return context.SaveNoCommitEntity(product, "Saving new cost.");
            }
            return true;
        }
    }
}
