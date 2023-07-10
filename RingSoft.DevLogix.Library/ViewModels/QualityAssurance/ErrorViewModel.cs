using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IErrorView : IDbMaintenanceView
    {
        void SetFocusAfterText(string text, bool descrioption, bool setFocus);

        void CopyToClipboard(string text);

        //void PunchIn(Error error);

        bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentError, int totalErrors, string currentErrorText);
    }

    public class ErrorViewModel :DevLogixDbMaintenanceViewModel<Error>
    {
        public override TableDefinition<Error> TableDefinition => AppGlobals.LookupContext.Errors;

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

        private DateTime _errorDate;

        public DateTime ErrorDate
        {
            get => _errorDate;
            set
            {
                if (_errorDate == value)
                {
                    return;
                }
                _errorDate = value;
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
                {
                    return;
                }
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
                {
                    return;
                }
                _statusAutoFillValue = value;
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
                {
                    return;
                }
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
                {
                    return;
                }
                _productAutoFillValue = value;
                SetProductVersionFilter();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _priorityAutoFillSetup;

        public AutoFillSetup PriorityAutoFillSetup
        {
            get => _priorityAutoFillSetup;
            set
            {
                if (_priorityAutoFillSetup == value)
                {
                    return;
                }
                _priorityAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _priorityAutoFillValue;

        public AutoFillValue PriorityAutoFillValue
        {
            get => _priorityAutoFillValue;
            set
            {
                if (_priorityAutoFillValue == value)
                {
                    return;
                }
                _priorityAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _foundUserAutoFillSetup;

        public AutoFillSetup FoundUserAutoFillSetup
        {
            get => _foundUserAutoFillSetup;
            set
            {
                if (_foundUserAutoFillSetup == value)
                {
                    return;
                }
                _foundUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _foundUserAutoFillValue;

        public AutoFillValue FoundUserAutoFillValue
        {
            get => _foundUserAutoFillValue;
            set
            {
                if (_foundUserAutoFillValue == value)
                {
                    return;
                }
                _foundUserAutoFillValue = value;
                OnPropertyChanged();
                
            }
        }


        private AutoFillSetup _foundVersionAutoFillSetup;

        public AutoFillSetup FoundVersionAutoFillSetup
        {
            get => _foundVersionAutoFillSetup;
            set
            {
                if (_foundVersionAutoFillSetup == value)
                {
                    return;
                }
                _foundVersionAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _fixedUserAutoFillSetup;

        public AutoFillSetup FixedUserAutoFillSetup
        {
            get => _fixedUserAutoFillSetup;
            set
            {
                if (_fixedUserAutoFillSetup == value)
                {
                    return;
                }
                _fixedUserAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _foundVersionAutoFillValue;

        public AutoFillValue FoundVersionAutoFillValue
        {
            get => _foundVersionAutoFillValue;
            set
            {
                if (_foundVersionAutoFillValue == value)
                {
                    return;
                }
                _foundVersionAutoFillValue = value;
                OnPropertyChanged();
           }
        }

        private AutoFillSetup _fixedVersionAutoFillSetup;

        public AutoFillSetup FixedVersionAutoFillSetup
        {
            get => _fixedVersionAutoFillSetup;
            set
            {
                if (_fixedVersionAutoFillSetup == value)
                {
                    return;
                }
                _fixedVersionAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _fixedVersionAutoFillValue;

        public AutoFillValue FixedVersionAutoFillValue
        {
            get => _fixedVersionAutoFillValue;
            set
            {
                if (_fixedVersionAutoFillValue == value)
                {
                    return;
                }
                _fixedVersionAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _assignedDeveloperAutoFillSetup;

        public AutoFillSetup AssignedDeveloperAutoFillSetup
        {
            get => _assignedDeveloperAutoFillSetup;
            set
            {
                if (_assignedDeveloperAutoFillSetup == value)
                {
                    return;
                }
                _assignedDeveloperAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedDeveloperAutoFillValue;

        public AutoFillValue AssignedDeveloperAutoFillValue
        {
            get => _assignedDeveloperAutoFillValue;
            set
            {
                if (_assignedDeveloperAutoFillValue == value)
                {
                    return;
                }
                _assignedDeveloperAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _assignedQualityAssuranceAutoFillSetup;

        public AutoFillSetup AssignedQualityAssuranceAutoFillSetup
        {
            get => _assignedQualityAssuranceAutoFillSetup;
            set
            {
                if (_assignedQualityAssuranceAutoFillSetup == value)
                {
                    return;
                }
                _assignedQualityAssuranceAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedQualityAssuranceAutoFillValue;

        public AutoFillValue AssignedQualityAssuranceAutoFillValue
        {
            get => _assignedQualityAssuranceAutoFillValue;
            set
            {
                if (_assignedQualityAssuranceAutoFillValue == value)
                {
                    return;
                }
                _assignedQualityAssuranceAutoFillValue = value;
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


        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;

                _description = value;
                OnPropertyChanged();
            }
        }

        private string _resolution;

        public string Resolution
        {
            get => _resolution;
            set
            {
                if (_resolution == value)
                    return;

                _resolution = value;
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

        private ErrorUserGridManager _errorUserGridManager;

        public ErrorUserGridManager ErrorUserGridManager
        {
            get => _errorUserGridManager;
            set
            {
                if (_errorUserGridManager == value)
                    return;

                _errorUserGridManager = value;
                OnPropertyChanged();
            }
        }


        private ErrorDeveloperManager _developerManager;

        public ErrorDeveloperManager DeveloperManager
        {
            get => _developerManager;
            set
            {
                if (_developerManager == value)
                {
                    return;
                }
                _developerManager = value;
                OnPropertyChanged();
            }
        }

        private ErrorSupportTicketManager _supportTicketManager;

        public ErrorSupportTicketManager SupportTicketManager
        {
            get => _supportTicketManager;
            set
            {
                if (_supportTicketManager == value)
                {
                    return;
                }
                _supportTicketManager = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _testingOutlineAutoFillSetup;

        public AutoFillSetup TestingOutlineAutoFillSetup
        {
            get => _testingOutlineAutoFillSetup;
            set
            {
                if (_testingOutlineAutoFillSetup == value)
                    return;

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
                    return;

                _testingOutlineAutoFillValue = value;
                OnPropertyChanged(null, _testingOutlineAutoFillSetup.SetDirty);
            }
        }


        private ErrorQaManager _errorQaManager;

        public ErrorQaManager ErrorQaManager
        {
            get => _errorQaManager;
            set
            {
                if (_errorQaManager == value)
                    return;

                _errorQaManager = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ClipboardCopyCommand { get; set; }

        public RelayCommand WriteOffCommand { get; set; }

        public RelayCommand PassCommand { get; set; }

        public RelayCommand FailCommand { get; set; }

        public RelayCommand PunchInCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        public new IErrorView View { get; private set; }

        public double MinutesSpent { get; private set; }

        public AutoFillValue DefaultTestOutlineAutoFillValue { get; private set; }

        public AutoFillValue CurrentTestingOutlineAutoFillValue { get; private set; }

        private IDbContext _makeErrorIdContext;
        private bool _makeErrorId;
        private bool _loading;

        public ErrorViewModel()
        {
            ClipboardCopyCommand = new RelayCommand(() =>
            {
                var user = GetUser();
                if (user != null)
                {
                    var text =
                        $"{user.Name} - {GblMethods.FormatDateValue(DateTime.Now, DbDateTypes.DateTime)} - {KeyAutoFillValue.Text}";
                    View.CopyToClipboard(text);
                }
            });
            WriteOffCommand = new RelayCommand(WriteOffError);
            PassCommand = new RelayCommand(PassError);
            FailCommand = new RelayCommand(FailError);
            PunchInCommand = new RelayCommand(() =>
            {
                PunchIn();
            });
            RecalcCommand = new RelayCommand(Recalc);

            PrintProcessingHeader += ErrorViewModel_PrintProcessingHeader;

            //var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            //timeClockLookup.Include(p => p.User)
            //    .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            //timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);
            TimeClockLookup = AppGlobals.LookupContext.TimeClockLookup.Clone();
            //TimeClockLookup = timeClockLookup;
            //TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
        }

        protected override void Initialize()
        {
            AppGlobals.MainViewModel.ErrorViewModels.Add(this);
            
            if (base.View is IErrorView errorView)
            {
                View = errorView;
            }

            ViewLookupDefinition.InitialOrderByField = TableDefinition.GetFieldDefinition(p => p.Id);
            StatusAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorStatusId));
            ProductAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));
            PriorityAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ErrorPriorityId));
            FoundVersionAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FoundVersionId));
            FixedVersionAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FixedVersionId));
            FoundUserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.FoundByUserId));
            TestingOutlineAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.TestingOutlineId));

            AssignedDeveloperAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedDeveloperId));
            AssignedQualityAssuranceAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedTesterId));

            ErrorUserGridManager = new ErrorUserGridManager(this);
            DeveloperManager = new ErrorDeveloperManager(this);
            ErrorQaManager = new ErrorQaManager(this);
            SupportTicketManager = new ErrorSupportTicketManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.ErrorDevelopers);
            TablesToDelete.Add(AppGlobals.LookupContext.ErrorTesters);
            TablesToDelete.Add(AppGlobals.LookupContext.ErrorUsers);
            TablesToDelete.Add(AppGlobals.LookupContext.SupportTicketError);

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.TestingOutlines)
                {
                    var outline =
                        AppGlobals.LookupContext.TestingOutlines.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    CurrentTestingOutlineAutoFillValue = outline.GetAutoFillValue();
                    DefaultTestOutlineAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.TestingOutlines,
                            outline.Id.ToString());
                }
            }


            base.Initialize();
        }

        private void SetProductVersionFilter()
        {
            FoundVersionAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            FixedVersionAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();

            if (ProductAutoFillValue.IsValid())
            {
                var product =
                    AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue
                        .PrimaryKeyValue);

                FoundVersionAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId), Conditions.Equals,
                    product.Id);

                FixedVersionAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(
                    AppGlobals.LookupContext.ProductVersions.GetFieldDefinition(p => p.ProductId), Conditions.Equals,
                product.Id);

                //Peter Ringering -02 / 17 / 2023 12:49:33 AM - E - 13
                //if (MaintenanceMode == DbMaintenanceModes.AddMode)
                //{
                //    FoundVersionAutoFillValue = GetVersionForUser();
                //}
                if (!_loading)
                {
                    FoundVersionAutoFillValue = GetVersionForUser();
                    if (FixedVersionAutoFillValue != null)
                    {
                        FixedVersionAutoFillValue = GetVersionForUser();
                    }
                }
            }
        }

        private AutoFillValue GetVersionForUser()
        {
            if (ProductAutoFillValue.IsValid())
            {
                var product = ProductAutoFillValue.GetEntity<Product>();
                return AppGlobals.GetVersionForUser(product);
            }

            return null;
        }

        protected override Error PopulatePrimaryKeyControls(Error newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetError(newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                //if (_makeErrorIdContext != null && _makeErrorId)
                //{
                //    var errorId = $"E-{result.Id}";
                //    result.ErrorId = errorId;
                //    _makeErrorIdContext.SaveEntity(result, "Updating ErrorId");
                //}
                _makeErrorIdContext = null;
                _makeErrorId = false;
                KeyAutoFillValue = result.GetAutoFillValue();
            }

            if (MaintenanceMode == DbMaintenanceModes.EditMode)
            {
                WriteOffCommand.IsEnabled = ClipboardCopyCommand.IsEnabled =
                    PassCommand.IsEnabled = FailCommand.IsEnabled = PunchInCommand.IsEnabled = true;
            }

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.ErrorId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return result;
        }

        private static Error GetError(int errorId)
        {
            Error newEntity;
            var context = AppGlobals.DataRepository.GetDataContext();
            var errorTable = context.GetTable<Error>();
            var result = errorTable.Include(p => p.Developers)
                .ThenInclude(p => p.Developer)
                .Include(p => p.Testers)
                .ThenInclude(p => p.Tester)
                .Include(p => p.Testers)
                .ThenInclude(p => p.NewErrorStatus)
                .Include(p => p.ErrorStatus)
                .Include(p => p.Product)
                .Include(p => p.ErrorPriority)
                .Include(p => p.FoundVersion)
                .Include(p => p.FixedVersion)
                .Include(p => p.FoundByUser)
                .Include(p => p.AssignedDeveloper)
                .Include(p => p.AssignedTester)
                .Include(p => p.Users)
                .ThenInclude(p => p.User)
                .Include(p => p.TestingOutline)
                .Include(p => p.SupportTickets)
                .ThenInclude(p => p.SupportTicket)
                .FirstOrDefault(p => p.Id == errorId);
            return result;
        }

        protected override void LoadFromEntity(Error entity)
        {
            _loading = true;
            ErrorDate = entity.ErrorDate.ToLocalTime();
            StatusAutoFillValue = entity.ErrorStatus.GetAutoFillValue();

            ProductAutoFillValue = entity.Product.GetAutoFillValue();

            PriorityAutoFillValue = entity.ErrorPriority.GetAutoFillValue();

            FoundVersionAutoFillValue = entity.FoundVersion.GetAutoFillValue();

            FixedVersionAutoFillValue = entity.FixedVersion.GetAutoFillValue();

            FoundUserAutoFillValue = entity.FoundByUser.GetAutoFillValue();

            AssignedDeveloperAutoFillValue = entity.AssignedDeveloper.GetAutoFillValue();

            AssignedQualityAssuranceAutoFillValue = entity.AssignedTester.GetAutoFillValue();

            Description = entity.Description;
            Resolution = entity.Resolution;
            DeveloperManager.LoadGrid(entity.Developers);
            CurrentTestingOutlineAutoFillValue = TestingOutlineAutoFillValue = entity.TestingOutline.GetAutoFillValue();
            ErrorQaManager.LoadGrid(entity.Testers);
            ErrorUserGridManager.LoadGrid(entity.Users);
            SupportTicketManager.LoadGrid(entity.SupportTickets);
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.Cost;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            _loading = false;
        }

        protected override Error GetEntityData()
        {
            var result = new Error
            {
                Id = Id,
                ErrorDate = ErrorDate.ToUniversalTime(),
                ErrorStatusId = StatusAutoFillValue.GetEntity<ErrorStatus>().Id,
                ProductId = ProductAutoFillValue.GetEntity<Product>().Id,
                ErrorPriorityId = PriorityAutoFillValue.GetEntity<ErrorPriority>().Id,
                FoundByUserId = FoundUserAutoFillValue.GetEntity<User>().Id,
                FoundVersionId = FoundVersionAutoFillValue.GetEntity<ProductVersion>().Id,
                FixedVersionId = FixedVersionAutoFillValue.GetEntity<ProductVersion>().Id,
                AssignedDeveloperId = AssignedDeveloperAutoFillValue.GetEntity<User>().Id,
                AssignedTesterId = AssignedQualityAssuranceAutoFillValue.GetEntity<User>().Id,
                Description = Description,
                Resolution = Resolution,
                TestingOutlineId = TestingOutlineAutoFillValue.GetEntity<TestingOutline>().Id,
            };

            if (KeyAutoFillValue != null)
            {
                result.ErrorId = KeyAutoFillValue.Text;
            }
            if (Id > 0)
            {
                var context = AppGlobals.DataRepository.GetDataContext();
                var error = context.GetTable<Error>()
                    .FirstOrDefault(p => p.Id == Id);
                if (error != null)
                {
                    result.MinutesSpent = error.MinutesSpent;
                    result.Cost = error.Cost;
                }

            }

            if (result.FoundByUserId == 0)
            {
                result.FoundByUserId = null;
                if (AppGlobals.LoggedInUser != null)
                {
                    var message = "Found by user cannot be empty. Do you wish to set the found by user to yourself?";
                    var caption = "Validation Check";
                    if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        MessageBoxButtonsResult.Yes)
                    {
                        FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
                        result.FoundByUserId = AppGlobals.LoggedInUser.Id;
                    }

                }
            }

            if (result.FixedVersionId == 0)
            {
                result.FixedVersionId = null;
            }

            if (result.AssignedDeveloperId == 0)
            {
                result.AssignedDeveloperId = null;
            }

            if (result.AssignedTesterId == 0)
            {
                result.AssignedTesterId = null;
            }

            if (result.TestingOutlineId == 0)
            {
                result.TestingOutlineId = null;
            }

            return result;
        }

        protected override bool ValidateEntity(Error entity)
        {
            var result = base.ValidateEntity(entity);

            if (!result && MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                KeyAutoFillValue = null;
            }
            return result;
        }


        protected override void ClearData()
        {
            Id = 0;
            ErrorDate = DateTime.Now;
            StatusAutoFillValue = ProductAutoFillValue = PriorityAutoFillValue = FoundVersionAutoFillValue =
                FixedVersionAutoFillValue = AssignedDeveloperAutoFillValue = AssignedQualityAssuranceAutoFillValue =  null;
            Description = Resolution = string.Empty;
            if (AppGlobals.LoggedInUser != null)
            {
                FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
                //SetErrorText(GetUser());
            }

            DeveloperManager.SetupForNewRecord();
            ErrorQaManager.SetupForNewRecord();
            ErrorUserGridManager.SetupForNewRecord();
            SupportTicketManager.SetupForNewRecord();

            WriteOffCommand.IsEnabled = ClipboardCopyCommand.IsEnabled = PassCommand.IsEnabled = FailCommand.IsEnabled = PunchInCommand.IsEnabled = false;
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
            TotalCost = 0;
            MinutesSpent = 0;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            CurrentTestingOutlineAutoFillValue = TestingOutlineAutoFillValue = DefaultTestOutlineAutoFillValue;
        }

        protected override bool SaveEntity(Error entity)
        {
            var makeErrorId = entity.Id == 0 && entity.ErrorId.IsNullOrEmpty();
            var result = false;
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                if (makeErrorId)
                {
                    entity.ErrorId = Guid.NewGuid().ToString();
                }
                result = context.SaveEntity(entity, "Saving Error");

                if (result && makeErrorId)
                {
                    var errorId = $"E-{entity.Id}";
                    entity.ErrorId = errorId;
                    result = context.SaveEntity(entity, "Updating Error Id");
                }

                if (result)
                {
                    var supportTicketTable = context.GetTable<SupportTicketError>();
                    if (supportTicketTable != null)
                    {
                        var oldTickets = supportTicketTable
                            .Where(p => p.ErrorId == Id);

                        if (oldTickets.Any())
                        {
                            context.RemoveRange(oldTickets);
                        }

                        var list = SupportTicketManager.GetEntityList();
                        foreach (var supportTicketError in list)
                        {
                            supportTicketError.ErrorId = entity.Id;
                        }
                        context.AddRange(list);
                        result = context.Commit("Saving Support Tickets");
                    }
                }
            }

            //if (MaintenanceMode == DbMaintenanceModes.AddMode)
            //{
            //    _makeErrorIdContext = context as IDbContext;
            //}

            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var developerQuery = context.GetTable<ErrorDeveloper>();
                var developers = developerQuery.Where(p => p.ErrorId == Id);
                context.RemoveRange(developers);

                var testersQuery = context.GetTable<ErrorQa>();
                var testers = testersQuery.Where(p => p.ErrorId == Id);
                context.RemoveRange(testers);

                var usersQuery = context.GetTable<ErrorUser>();
                var users = usersQuery.Where(p => p.ErrorId == Id);
                context.RemoveRange(users);

                var supportTicketTable = context.GetTable<SupportTicketError>();
                var oldTickets = supportTicketTable
                    .Where(p => p.ErrorId == Id);

                if (oldTickets.Any())
                {
                    context.RemoveRange(oldTickets);
                }

                var table = TableDefinition;
                var entity = context.GetTable<Error>().FirstOrDefault(p => p.Id == Id);
                return context.DeleteEntity(entity, "Deleting Error");
            }

            return false;

        }

        private void PassError()
        {
            var user = GetUser();
            if (user != null && user.Department.ErrorPassStatusId.HasValue)
            {
                ErrorQaManager.AddNewRow(user.Department.ErrorPassStatusId.Value);

                StatusAutoFillValue =
                    StatusAutoFillSetup.GetAutoFillValueForIdValue(user.Department.ErrorPassStatusId.Value);

                if (user != null && !user.Department.PassText.IsNullOrEmpty())
                {
                    InsertErrorText(user, user.Department.PassText, false);
                }
            }
        }

        private void FailError()
        {
            var user = GetUser();
            if (user != null && user.Department.ErrorFailStatusId.HasValue)
            {
                ErrorQaManager.AddNewRow(user.Department.ErrorFailStatusId.Value);

                StatusAutoFillValue =
                    StatusAutoFillSetup.GetAutoFillValueForIdValue(user.Department.ErrorFailStatusId.Value);

                if (user != null && !user.Department.FailText.IsNullOrEmpty())
                {
                    InsertErrorText(user, user.Department.FailText, false);
                }
            }
        }

        private User GetUser()
        {
            //if (AppGlobals.LoggedInUser != null)
            //{
            //    var context = AppGlobals.DataRepository.GetDataContext();
            //    if (context != null)
            //    {
            //        var user = context.GetTable<User>().Include(p => p.Department)
            //            .FirstOrDefault(p => p.Id == AppGlobals.LoggedInUser.Id);
            //        return user;
            //    }
            //}

            return AppGlobals.LoggedInUser;
        }

        private void WriteOffError()
        {
            DeveloperManager.AddNewRow();

            var versionAutoFillValue = GetVersionForUser();
            if (versionAutoFillValue.IsValid())
            {
                FixedVersionAutoFillValue = versionAutoFillValue;
            }

            var user = GetUser();
            if (user != null && user.Department.ErrorFixStatusId.HasValue)
            {
                StatusAutoFillValue =
                    StatusAutoFillSetup.GetAutoFillValueForIdValue(user.Department.ErrorFixStatusId.Value);
            }

            if (user != null && !user.Department.FixText.IsNullOrEmpty())
            {
                InsertErrorText(user, user.Department.FixText, false);
            }
        }

        private void InsertErrorText(User user, string text, bool description)
        {
            var newText = $"{user.Name} - {GblMethods.FormatDateValue(DateTime.Now, DbDateTypes.DateTime)} - {text} - ";
            if (description)
            {
                Description = $"{newText}\r\n{Description}";
            }
            else
            {
                Resolution = $"{newText}\r\n{Resolution}";
            }

            View.SetFocusAfterText(newText, description, true);
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            printerSetupArgs.PrintingProperties.ReportType = ReportTypes.Custom;
            printerSetupArgs.PrintingProperties.CustomReportPathFileName =
                $"{RingSoftAppGlobals.AssemblyDirectory}\\QualityAssurance\\Error.rpt";

            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }

        public override void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            base.ProcessPrintOutputData(printerSetupArgs);
            var customProperties = new List<PrintingCustomProperty>();
            customProperties.Add(new PrintingCustomProperty
            {
                Name = "intRecordCount",
                Value = printerSetupArgs.TotalRecords.ToString(),
            });
            PrintingInteropGlobals.PropertiesProcessor.CustomProperties = customProperties;
        }


        private void ErrorViewModel_PrintProcessingHeader(object? sender, PrinterDataProcessedEventArgs e)
        {
            var errorId = TableDefinition.GetEntityFromPrimaryKeyValue(e.PrimaryKey).Id;
            var context = AppGlobals.DataRepository.GetDataContext();
            var errorTableQuery = context.GetTable<Error>();

            var error = errorTableQuery.Include(p => p.Developers)
                .ThenInclude(p => p.Developer)
                .Include(p => p.Testers)
                .ThenInclude(p => p.Tester)
                .Include(p => p.Testers)
                .ThenInclude(p => p.NewErrorStatus)
                .FirstOrDefault(p => p.Id == errorId);

            var detailsChunk = new List<PrintingInputDetailsRow>();
            foreach (var errorDeveloper in error.Developers)
            {
                var detailRow = new PrintingInputDetailsRow();
                detailRow.HeaderRowKey = e.HeaderRow.RowKey;
                detailRow.TablelId = 1;
                detailRow.StringField01 = errorDeveloper.Developer.Name;
                detailRow.StringField02 = errorDeveloper.DateFixed.ToLocalTime()
                    .FormatDateValue(DbDateTypes.DateTime, false);
                detailsChunk.Add(detailRow);
            }
            PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);
            
            detailsChunk.Clear();

            foreach (var errorTester in error.Testers)
            {
                var detailRow = new PrintingInputDetailsRow();
                detailRow.HeaderRowKey = e.HeaderRow.RowKey;
                detailRow.TablelId = 2;
                detailRow.StringField01 = errorTester.Tester.Name;
                detailRow.StringField02 = errorTester.NewErrorStatus.Description;
                detailRow.StringField03 = errorTester.DateChanged.ToLocalTime()
                    .FormatDateValue(DbDateTypes.DateTime, false);
                detailsChunk.Add(detailRow);
            }

            PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);
        }

        private void PunchIn()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ErrorUser>();
            var user = table.FirstOrDefault(p => p.ErrorId == Id
                                                 && p.UserId == AppGlobals.LoggedInUser.Id);
            if (user == null)
            {
                user = new ErrorUser
                {
                    ErrorId = Id,
                    UserId = AppGlobals.LoggedInUser.Id,
                };
                context.AddRange(new List<ErrorUser>
                {
                    user
                });
                if (!context.Commit("Adding Error User"))
                {
                    return;
                }
                user.User = AppGlobals.LoggedInUser;
                ErrorUserGridManager.AddUserRow(user);
            }
            AppGlobals.MainViewModel.PunchIn(GetError(Id));
        }

        public void RefreshCost(List<ErrorUser> users)
        {
            ErrorUserGridManager.RefreshCost(users);
            GetTotals();
        }
        public void RefreshCost(ErrorUser errorUser)
        {
            ErrorUserGridManager.RefreshCost(errorUser);
            GetTotals();
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }

        private void GetTotals()
        {
            ErrorUserGridManager.GetTotals(out var minutesSpent, out var total);
            MinutesSpent = minutesSpent;
            TotalCost = total;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        private void Recalc()
        {
            var lookupFilter = ViewLookupDefinition.Clone();
            if (!View.ProcessRecalcLookupFilter(lookupFilter))
                return;
            var result = View.StartRecalcProcedure(lookupFilter);
            if (!result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Error Recalculating", RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalculateProcedure(LookupDefinitionBase lookupToFilter)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = AppGlobals.DataRepository.GetDataContext();
            var errorsTable = context.GetTable<Error>();
            var usersTable = context.GetTable<User>();
            var timeClocksTable = context.GetTable<TimeClock>();
            DbDataProcessor.DontDisplayExceptions = true;

            var totalErrors = lookupData.GetRecordCount();
            var currentError = 1;
            lookupData.PrintOutput += (sender, e) =>
            {
                foreach (var primaryKeyValue in e.Result)
                {
                    var error = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
                    if (error != null)
                    {
                        error = errorsTable
                            .Include(p => p.Users)
                            .ThenInclude(p => p.User)
                            .FirstOrDefault(p => p.Id == error.Id);
                    }

                    if (error != null)
                    {
                        View.UpdateRecalcProcedure(currentError, totalErrors, error.ErrorId);
                        var errorUsers = new List<ErrorUser>(error.Users);
                        error.MinutesSpent = error.Cost = 0;
                        var timeClockUsers = timeClocksTable
                            .Where(p => p.ErrorId == error.Id)
                            .Select(p => p.UserId)
                            .Distinct();
                        foreach (var timeClockUser in timeClockUsers)
                        {
                            var errorUser = error.Users.FirstOrDefault(p => p.UserId == timeClockUser);
                            if (errorUser == null)
                            {
                                errorUser = new ErrorUser
                                {
                                    ErrorId = error.Id,
                                    UserId = timeClockUser
                                };
                                UpdateErrorUserCost(usersTable, errorUser, timeClocksTable, error);
                                context.AddRange(new List<ErrorUser>()
                                {
                                    errorUser
                                });
                                errorUsers.Add(errorUser);
                            }
                            else
                            {
                                UpdateErrorUserCost(usersTable, errorUser, timeClocksTable, error);

                                if (!context.SaveNoCommitEntity(errorUser, "Saving Error User"))
                                {
                                    result = DbDataProcessor.LastException;
                                    e.Abort = true;
                                    return;
                                }
                            }
                            error.MinutesSpent += errorUser.MinutesSpent;
                            error.Cost += errorUser.Cost;
                        }

                        if (!context.SaveNoCommitEntity(error, "Saving Error"))
                        {
                            result = DbDataProcessor.LastException;
                            e.Abort = true;
                            return;
                        }

                        if (error.Id == Id)
                        {
                            RefreshCost(errorUsers);
                        }
                    }

                    currentError++;
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

        private static void UpdateErrorUserCost(IQueryable<User> usersTable, ErrorUser errorUser, IQueryable<TimeClock> timeClocksTable,
            Error error)
        {
            var user = usersTable.FirstOrDefault(p => p.Id == errorUser.UserId);
            var errorUserMinutes = timeClocksTable
                .Where(p => p.ErrorId == error.Id
                            && p.UserId == errorUser.UserId)
                .ToList()
                .Sum(p => p.MinutesSpent);
            if (errorUserMinutes != null)
            {
                errorUser.MinutesSpent = errorUserMinutes.Value;
                errorUser.Cost = Math.Round((errorUser.MinutesSpent / 60) * user.HourlyRate, 2);
            }
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.ErrorViewModels.Remove(this);

            base.OnWindowClosing(e);
        }
    }
}
