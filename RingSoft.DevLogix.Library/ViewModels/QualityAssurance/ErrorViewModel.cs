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
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public interface IErrorView : IDbMaintenanceView
    {
        void SetFocusAfterText(string text, bool descrioption, bool setFocus);

        void CopyToClipboard(string text);

        bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentError, int totalErrors, string currentErrorText);
    }

    public class ErrorViewModel :DbMaintenanceViewModel<Error>
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

        #endregion

        public RelayCommand ClipboardCopyCommand { get; set; }

        public RelayCommand WriteOffCommand { get; set; }

        public RelayCommand PassCommand { get; set; }

        public RelayCommand FailCommand { get; set; }

        public RelayCommand PunchInCommand { get; set; }

        public RelayCommand RecalcCommand { get; set; }

        public UiCommand DescriptionUiCommand { get; } = new UiCommand();

        public UiCommand ResolutionUiCommand { get; } = new UiCommand();

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
            TimeClockLookup = AppGlobals.LookupContext.TimeClockTabLookup.Clone();
            //TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
            RegisterLookup(TimeClockLookup);

            ErrorUserGridManager = new ErrorUserGridManager(this);
            DeveloperManager = new ErrorDeveloperManager(this);
            ErrorQaManager = new ErrorQaManager(this);
            SupportTicketManager = new ErrorSupportTicketManager(this);

            RegisterGrid(ErrorUserGridManager, true);
            RegisterGrid(DeveloperManager, true);
            RegisterGrid(ErrorQaManager, true);
            RegisterGrid(SupportTicketManager);

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

        }

        protected override void Initialize()
        {
            ViewLookupDefinition.InitialOrderByField = TableDefinition.GetFieldDefinition(p => p.Id);
            AppGlobals.MainViewModel.ErrorViewModels.Add(this);
            
            if (base.View is IErrorView errorView)
            {
                View = errorView;
            }

            MapFieldToUiCommand(DescriptionUiCommand, TableDefinition.GetFieldDefinition(p => p.Description));
            MapFieldToUiCommand(ResolutionUiCommand, TableDefinition.GetFieldDefinition(p => p.Resolution));

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

        protected override void PopulatePrimaryKeyControls(Error newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            if (MaintenanceMode == DbMaintenanceModes.EditMode)
            {
                WriteOffCommand.IsEnabled = ClipboardCopyCommand.IsEnabled =
                    PassCommand.IsEnabled = FailCommand.IsEnabled = PunchInCommand.IsEnabled = true;
            }
        }

        private static Error GetError(int errorId)
        {
            var error = new Error()
            {
                Id = errorId,
            };

            var result = error.FillOutProperties(new List<TableDefinitionBase>()
            {
                AppGlobals.LookupContext.ErrorDevelopers,
            });
            return result;

            //var context = SystemGlobals.DataRepository.GetDataContext();
            //var errorTable = context.GetTable<Error>();
            //var result = errorTable.Include(p => p.Developers)
            //    .FirstOrDefault(p => p.Id == errorId);
            //return result;
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
            CurrentTestingOutlineAutoFillValue = TestingOutlineAutoFillValue = entity.TestingOutline.GetAutoFillValue();
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
                var context = SystemGlobals.DataRepository.GetDataContext();
                var error = context.GetTable<Error>()
                    .FirstOrDefault(p => p.Id == Id);
                if (error != null)
                {
                    result.MinutesSpent = error.MinutesSpent;
                    result.Cost = error.Cost;
                }

            }

            ValidateFoundByUser(result);

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

        private async void ValidateFoundByUser(Error result)
        {
            if (result.FoundByUserId == 0 
                && (FoundUserAutoFillValue == null 
                || FoundUserAutoFillValue.Text == string.Empty))
            {
                result.FoundByUserId = null;
                if (AppGlobals.LoggedInUser != null)
                {
                    var message = "Found by user cannot be empty. Do you wish to set the found by user to yourself?";
                    var caption = "Validation Check";
                    if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        MessageBoxButtonsResult.Yes)
                    {
                        FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
                        result.FoundByUserId = AppGlobals.LoggedInUser.Id;
                    }
                }
            }
        }

        protected override bool ValidateEntity(Error entity)
        {
            var caption = "Validation Error";
            if (!StatusAutoFillValue.IsValid(true))
            {
                StatusAutoFillSetup.HandleValFail();
                return false;
            }

            if (!ProductAutoFillValue.IsValid(true))
            {
                ProductAutoFillSetup.HandleValFail();
                return false;
            }

            if (!PriorityAutoFillValue.IsValid(true))
            {
                PriorityAutoFillSetup.HandleValFail();
                return false;
            }

            if (!FoundVersionAutoFillValue.IsValid(true))
            {
                FoundVersionAutoFillSetup.HandleValFail();
                return false;
            }

            if (!FixedVersionAutoFillValue.IsValid(checkDb:true, valHasText:true))
            {
                FixedVersionAutoFillSetup.HandleValFail();
                return false;
            }

            if (!FoundUserAutoFillValue.IsValid(true))
            {
                FoundUserAutoFillSetup.HandleValFail();
                return false;
            }

            if (!AssignedDeveloperAutoFillValue.IsValid(checkDb:true, valHasText:true))
            {
                AssignedDeveloperAutoFillSetup.HandleValFail();
                return false;
            }

            if (!AssignedQualityAssuranceAutoFillValue.IsValid(checkDb:true, valHasText:true))
            {
                AssignedQualityAssuranceAutoFillSetup.HandleValFail();
                return false;
            }

            if (Description.IsNullOrEmpty())
            {
                var message = "Description must contain some text.";
                OnValidationFail(TableDefinition.GetFieldDefinition(p => p.Description)
                , message, caption);
                return false;
            }

            if (!TestingOutlineAutoFillValue.IsValid(checkDb: true, valHasText: true))
            {
                TestingOutlineAutoFillSetup.HandleValFail();
                return false;
            }

            //var result = base.ValidateEntity(entity);

            //if (!result && MaintenanceMode == DbMaintenanceModes.AddMode)
            //{
            //    KeyAutoFillValue = null;
            //}

            var result = SupportTicketManager.ValidateGrid();
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            ErrorDate = GblMethods.NowDate();
            StatusAutoFillValue = ProductAutoFillValue = PriorityAutoFillValue = FoundVersionAutoFillValue =
                FixedVersionAutoFillValue = AssignedDeveloperAutoFillValue = AssignedQualityAssuranceAutoFillValue =  null;
            Description = Resolution = string.Empty;
            if (AppGlobals.LoggedInUser != null)
            {
                FoundUserAutoFillValue = FoundUserAutoFillSetup.GetAutoFillValueForIdValue(AppGlobals.LoggedInUser.Id);
            }

            WriteOffCommand.IsEnabled = ClipboardCopyCommand.IsEnabled = PassCommand.IsEnabled = FailCommand.IsEnabled = PunchInCommand.IsEnabled = false;
            TotalCost = 0;
            MinutesSpent = 0;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            CurrentTestingOutlineAutoFillValue = TestingOutlineAutoFillValue = DefaultTestOutlineAutoFillValue;
        }

        protected override bool SaveEntity(Error entity)
        {
            GenerateKeyValue("E", entity);
            return base.SaveEntity(entity);
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
            var context = SystemGlobals.DataRepository.GetDataContext();
            var errorTableQuery = context.GetTable<Error>();

            var error = errorTableQuery.Include(p => p.Developers)
                .ThenInclude(p => p.Developer)
                .Include(p => p.Testers)
                .ThenInclude(p => p.Tester)
                .Include(p => p.Testers)
                .ThenInclude(p => p.NewErrorStatus)
                .FirstOrDefault(p => p.Id == errorId);

            var detailsChunk = new List<PrintingInputDetailsRow>();
            if (error != null)
            {
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
            }

            PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);
        }

        private void PunchIn()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
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

                var test = this;
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
            TimeClockLookup.SetCommand(GetLookupCommand(LookupCommands.Refresh));
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
            if (result.IsNullOrEmpty())
            {
                ControlsGlobals.UserInterface.ShowMessageBox("Recalculation Complete"
                , "Recalculation", RsMessageBoxIcons.Information);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Error Recalculating", RsMessageBoxIcons.Error);
            }
        }

        public string StartRecalculateProcedure(LookupDefinitionBase lookupToFilter)
        {
            var result = string.Empty;
            var lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToFilter, false);
            var context = SystemGlobals.DataRepository.GetDataContext();
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
                if (user != null) errorUser.Cost = Math.Round((errorUser.MinutesSpent / 60) * user.HourlyRate, 2);
            }
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.ErrorViewModels.Remove(this);

            base.OnWindowClosing(e);
        }
    }
}
