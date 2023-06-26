using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.DataAccess.LookupModel;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public interface ITestingOutlineView : IDbMaintenanceView
    {
        void PunchIn(TestingOutline testingOutline);

        bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup);

        string StartRecalcProcedure(LookupDefinitionBase lookup);

        void UpdateRecalcProcedure(int currentOutline, int totalOutlines, string currentOutlineText);
    }
    public class TestingOutlineViewModel : DevLogixDbMaintenanceViewModel<TestingOutline>
    {
        public override TableDefinition<TestingOutline> TableDefinition => AppGlobals.LookupContext.TestingOutlines;

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

        private AutoFillSetup _productSetup;

        public AutoFillSetup ProductSetup
        {
            get => _productSetup;
            set
            {
                if (_productSetup == value)
                    return;

                _productSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _productValue;

        public AutoFillValue ProductValue
        {
            get => _productValue;
            set
            {
                if (_productValue == value)
                    return;

                _productValue = value;
                OnPropertyChanged();
                DetailsGridManager.UpdateProductVersion(value.GetEntity<Product>().Id);
            }
        }

        private AutoFillSetup _createdByAutoFillSetup;

        public AutoFillSetup CreatedByAutoFillSetup
        {
            get => _createdByAutoFillSetup;
            set
            {
                if (_createdByAutoFillSetup == value)
                    return;

                _createdByAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _createdByAutoFillValue;

        public AutoFillValue CreatedByAutoFillValue
        {
            get => _createdByAutoFillValue;
            set
            {
                if (_createdByAutoFillValue == value)
                    return;

                _createdByAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _assignedToAutoFillSetup;

        public AutoFillSetup AssignedToAutoFillSetup
        {
            get => _assignedToAutoFillSetup;
            set
            {
                if (_assignedToAutoFillSetup == value)
                    return;

                _assignedToAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _assignedToAutoFillValue;

        public AutoFillValue AssignedToAutoFillValue
        {
            get => _assignedToAutoFillValue;
            set
            {
                if (_assignedToAutoFillValue == value)
                    return;

                _assignedToAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dueDate;

        public DateTime? DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate == value)
                    return;

                _dueDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _percentComplete;

        public decimal PercentComplete
        {
            get => _percentComplete;
            set
            {
                if (_percentComplete == value)
                    return;

                _percentComplete = value;
                OnPropertyChanged(null, false);
            }
        }

        private TestingOutlineDetailsGridManager _detailsGridManager;

        public TestingOutlineDetailsGridManager DetailsGridManager
        {
            get => _detailsGridManager;
            set
            {
                if (_detailsGridManager == value)
                    return;

                _detailsGridManager = value;
                OnPropertyChanged();
            }
        }

        private TestingOutlineTemplatesGridManager _templatesGridManager;

        public TestingOutlineTemplatesGridManager TemplatesGridManager
        {
            get => _templatesGridManager;
            set
            {
                if (_templatesGridManager == value)
                    return;

                _templatesGridManager = value;
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

        private decimal _totalCost;

        public decimal TotalCost
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

        private TestingOutlineCostManager _testingOutlineCostManager;

        public TestingOutlineCostManager TestingOutlineCostManager
        {
            get => _testingOutlineCostManager;
            set
            {
                if (_testingOutlineCostManager == value)
                    return;

                _testingOutlineCostManager = value;
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

        private LookupDefinition<ErrorLookup, Error> _errorLookup;

        public LookupDefinition<ErrorLookup, Error> ErrorLookup
        {
            get => _errorLookup;
            set
            {
                if (_errorLookup == value)
                    return;

                _errorLookup = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _errorLookupCommand;

        public LookupCommand ErrorLookupCommand
        {
            get => _errorLookupCommand;
            set
            {
                if (_errorLookupCommand == value)
                    return;

                _errorLookupCommand = value;
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


        public new ITestingOutlineView  View { get; private set; }

        public RelayCommand GenerateDetailsCommand { get; private set; }

        public RelayCommand RetestCommand { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public RelayCommand ErrorAddModifyCommand { get; private set; }

        public decimal MinutesSpent { get; private set; }

        public AutoFillValue DefaultProductAutoFillValue { get; private set; }

        public AutoFillValue OldProductValue { get; private set; }

        public AutoFillValue CurrentUserAutoFillValue { get; private set; }

        public TestingOutlineViewModel()
        {
            GenerateDetailsCommand = new RelayCommand(GenerateDetails);
            RetestCommand = new RelayCommand(Retest);
            PunchInCommand = new RelayCommand(PunchIn);
            RecalcCommand = new RelayCommand(Recalculate);
            ErrorAddModifyCommand = new RelayCommand(OnErrorAddModify);

            ProductSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));
            CreatedByAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CreatedByUserId));
            AssignedToAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedToUserId));

            DetailsGridManager = new TestingOutlineDetailsGridManager(this);
            TemplatesGridManager = new TestingOutlineTemplatesGridManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.TestingOutlineDetails);
            TablesToDelete.Add(AppGlobals.LookupContext.TestingOutlineTemplates);
            TestingOutlineCostManager = new TestingOutlineCostManager(this);

            CurrentUserAutoFillValue = AppGlobals.LoggedInUser.GetAutoFillValue();

            var errorLookup = new LookupDefinition<ErrorLookup, Error>(AppGlobals.LookupContext.Errors);
            errorLookup.AddVisibleColumnDefinition(p => p.ErrorId
                , p => p.ErrorId);
            errorLookup.Include(p => p.FoundByUser)
                .AddVisibleColumnDefinition(p => p.User
                    ,p => p.Name);
            errorLookup.Include(p => p.ErrorStatus)
                .AddVisibleColumnDefinition(p => p.Status, p => p.Description);

            errorLookup.AddVisibleColumnDefinition(p => p.Date, p => p.ErrorDate);

            ErrorLookup = errorLookup;

            var timeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(AppGlobals.LookupContext.TimeClocks);
            timeClockLookup.AddVisibleColumnDefinition(p => p.PunchInDate, p => p.PunchInDate);
            timeClockLookup.Include(p => p.User)
                .AddVisibleColumnDefinition(p => p.UserName, p => p.Name);
            timeClockLookup.AddVisibleColumnDefinition(p => p.MinutesSpent, p => p.MinutesSpent);

            TimeClockLookup = timeClockLookup;
            TimeClockLookup.InitialOrderByType = OrderByTypes.Descending;
        }

        protected override void Initialize()
        {
            if (base.View is ITestingOutlineView testingOutlineView)
            {
                View = testingOutlineView;
            }
            AppGlobals.MainViewModel.TestingOutlineViewModels.Add(this);
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.Products)
                {
                    var product =
                        AppGlobals.LookupContext.Products.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    DefaultProductAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Products,
                            product.Id.ToString());
                }
            }

            base.Initialize();
        }

        protected override TestingOutline PopulatePrimaryKeyControls(TestingOutline newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var result = GetTestingOutline(newEntity.Id);

            Id = result.Id;
            PunchInCommand.IsEnabled = true;

            ErrorLookup.FilterDefinition.ClearFixedFilters();
            ErrorLookup.FilterDefinition.AddFixedFilter(p => p.TestingOutlineId, Conditions.Equals, Id);
            ErrorLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            TimeClockLookup.FilterDefinition.ClearFixedFilters();
            TimeClockLookup.FilterDefinition.AddFixedFilter(p => p.TestingOutlineId, Conditions.Equals, Id);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return result;
        }

        public TestingOutline? GetTestingOutline(int id, DataAccess.IDbContext context = null)
        {
            if (context == null)
            {
                context = AppGlobals.DataRepository.GetDataContext();
            }
            var table = context.GetTable<TestingOutline>();
            var result = table
                .Include(p => p.Product)
                .Include(p => p.CreatedByUser)
                .Include(p => p.AssignedToUser)
                .Include(p => p.Details)
                .ThenInclude(p => p.CompletedVersion)
                .Include(p => p.Details)
                .ThenInclude(p => p.TestingTemplate)
                .Include(p => p.Templates)
                .ThenInclude(p => p.TestingTemplate)
                .Include(p => p.Costs)
                .ThenInclude(p => p.User)
                .FirstOrDefault(p => p.Id == id);
            return result;
        }

        protected override void LoadFromEntity(TestingOutline entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            OldProductValue = ProductValue = entity.Product.GetAutoFillValue();
            CreatedByAutoFillValue = entity.CreatedByUser.GetAutoFillValue();
            AssignedToAutoFillValue = entity.AssignedToUser.GetAutoFillValue();
            DueDate = entity.DueDate;
            if (DueDate != null)
            {
                DueDate = DueDate.Value.ToLocalTime();
            }

            PercentComplete = AppGlobals.CalcPercentComplete(entity.Details);
            DetailsGridManager.LoadGrid(entity.Details);
            TemplatesGridManager.LoadGrid(entity.Templates);
            TestingOutlineCostManager.LoadGrid(entity.Costs);
            MinutesSpent = entity.MinutesSpent;
            TotalCost = entity.TotalCost;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            Notes = entity.Notes;
        }

        protected override TestingOutline GetEntityData()
        {
            var result = new TestingOutline()
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                ProductId = ProductValue.GetEntity<Product>().Id,
                CreatedByUserId = CreatedByAutoFillValue.GetEntity<User>().Id,
                AssignedToUserId = AssignedToAutoFillValue.GetEntity<User>().Id,
                DueDate = DueDate,
                PercentComplete = PercentComplete,
                Notes = Notes
            };

            if (result.DueDate != null)
            {
                result.DueDate = result.DueDate.Value.ToUniversalTime();
            }

            if (result.AssignedToUserId == 0)
            {
                result.AssignedToUserId = null;
            }
            return result;
        }

        protected override bool ValidateEntity(TestingOutline entity)
        {
            if (!base.ValidateEntity(entity))
            {
                return false;
            }

            return TemplatesGridManager.ValidateGrid();
        }

        protected override void ClearData()
        {
            Id = 0;
            OldProductValue = ProductValue = DefaultProductAutoFillValue;
            CreatedByAutoFillValue = CurrentUserAutoFillValue;
            AssignedToAutoFillValue = null;
            DueDate = null;
            PercentComplete = 0;
            Notes = null;
            DetailsGridManager.SetupForNewRecord();
            TemplatesGridManager.SetupForNewRecord();
            TestingOutlineCostManager.SetupForNewRecord();
            PunchInCommand.IsEnabled = false;
            MinutesSpent = 0;
            TotalCost = 0;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
            ErrorLookupCommand = GetLookupCommand(LookupCommands.Clear);
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(TestingOutline entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Testing Outline");
            if (result)
            {
                var details = DetailsGridManager.GetEntityList();
                if (details != null)
                {
                    foreach (var detail in details)
                    {
                        detail.TestingOutlineId = entity.Id;
                    }
                }

                var templates = TemplatesGridManager.GetEntityList();
                if (templates != null)
                {
                    foreach (var template in templates)
                    {
                        template.TestingOutlineId = entity.Id;
                    }
                }

                var existingDetails = context.GetTable<TestingOutlineDetails>()
                    .Where(p => p.TestingOutlineId == entity.Id);
                context.RemoveRange(existingDetails);
                context.AddRange(details);

                var existingTemplates = context.GetTable<TestingOutlineTemplate>()
                    .Where(p => p.TestingOutlineId == entity.Id);
                context.RemoveRange(existingTemplates);
                context.AddRange(templates);

                result = context.Commit("Saving Grids");
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingOutline>();
            var existOutline = table.FirstOrDefault(p => p.Id == Id);
            if (existOutline != null)
            {
                var existingDetails = context.GetTable<TestingOutlineDetails>()
                    .Where(p => p.TestingOutlineId == existOutline.Id);
                context.RemoveRange(existingDetails);

                var existingTemplates = context.GetTable<TestingOutlineTemplate>()
                    .Where(p => p.TestingOutlineId == Id);
                context.RemoveRange(existingTemplates);

                result = context.DeleteEntity(existOutline, "Deleting Testing Outline");
            }
            return result;
        }

        private void GenerateDetails()
        {
            if (DoSave() == DbMaintenanceResults.Success)
            {
                TemplatesGridManager.GenerateDetails();
            }
        }

        public void UpdateDetails(List<TestingOutlineDetails> newDetails)
        {
            DetailsGridManager.UpdateDetails(newDetails);
        }

        private void Retest()
        {
            DetailsGridManager.Retest();
            var details = DetailsGridManager.GetEntityList();
            PercentComplete = AppGlobals.CalcPercentComplete(details);
            RecordDirty = true;
        }

        private void PunchIn()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingOutlineCost>();
            var user = table.FirstOrDefault(p => p.TestingOutlineId == Id
                                                 && p.UserId == AppGlobals.LoggedInUser.Id);
            if (user == null)
            {
                user = new TestingOutlineCost()
                {
                    TestingOutlineId = Id,
                    UserId = AppGlobals.LoggedInUser.Id,
                };
                context.AddRange(new List<TestingOutlineCost>
                {
                    user
                });
                if (!context.Commit("Adding Cost"))
                {
                    return;
                }
                user.User = AppGlobals.LoggedInUser;
                TestingOutlineCostManager.AddUserRow(user);
            }

            var testingOutline = GetTestingOutline(Id);
            View.PunchIn(testingOutline);
        }

        public void RefreshCost(List<TestingOutlineCost> users)
        {
            TestingOutlineCostManager.RefreshCost(users);
            GetTotals();
        }
        public void RefreshCost(TestingOutlineCost costUser)
        {
            TestingOutlineCostManager.RefreshCost(costUser);
            GetTotals();
        }

        private void GetTotals()
        {
            TestingOutlineCostManager.GetTotals(out var minutesSpent, out var total);
            TotalCost = total;
            MinutesSpent = minutesSpent;
            TotalCost = total;
            TotalTimeSpent = AppGlobals.MakeTimeSpent(MinutesSpent);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.TestingOutlineViewModels.Remove(this);
            base.OnWindowClosing(e);
        }

        private void Recalculate()
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

        public string StartRecalcProcedure(LookupDefinitionBase lookupToFilter)
        {
            var result = string.Empty;
            var lookupUi = new LookupUserInterface { PageSize = 10 };
            var lookupData = new LookupDataBase(lookupToFilter, lookupUi);
            var context = AppGlobals.DataRepository.GetDataContext();
            var outlinesTable = context.GetTable<TestingOutline>();
            var usersTable = context.GetTable<User>();
            var timeClocksTable = context.GetTable<TimeClock>();
            DbDataProcessor.DontDisplayExceptions = true;

            var totalOutlines = lookupData.GetRecordCountWait();
            var currentOutline = 1;

            lookupData.PrintDataChanged += (sender, e) =>
            {
                foreach (DataRow outputTableRow in e.OutputTable.Rows)
                {
                    var primaryKey = new PrimaryKeyValue(TableDefinition);
                    primaryKey.PopulateFromDataRow(outputTableRow);
                    var outline = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKey);
                    if (outline != null)
                    {
                        outline = outlinesTable
                            .Include(p => p.Costs)
                            .ThenInclude(p => p.User)
                            .FirstOrDefault(p => p.Id == outline.Id);
                    }

                    if (outline != null)
                    {
                        View.UpdateRecalcProcedure(currentOutline, totalOutlines, outline.Name);
                        var outlineUsers = new List<TestingOutlineCost>(outline.Costs);
                        outline.MinutesSpent = outline.TotalCost = 0;
                        var timeClockUsers = timeClocksTable
                            .Where(p => p.TestingOutlineId == outline.Id)
                            .Select(p => p.UserId)
                            .Distinct();
                        foreach (var timeClockUser in timeClockUsers)
                        {
                            var outlineUser = outline.Costs.FirstOrDefault(p => p.UserId == timeClockUser);
                            if (outlineUser == null)
                            {
                                outlineUser = new TestingOutlineCost()
                                {
                                    TestingOutlineId = outline.Id,
                                    UserId = timeClockUser
                                };
                                UpdateOutlineUserCost(usersTable, outlineUser, timeClocksTable, outline);
                                context.AddRange(new List<TestingOutlineCost>()
                                {
                                    outlineUser
                                });
                                outlineUsers.Add(outlineUser);
                            }
                            else
                            {
                                UpdateOutlineUserCost(usersTable, outlineUser, timeClocksTable, outline);

                                if (!context.SaveNoCommitEntity(outlineUser, "Saving Timeclock Cost"))
                                {
                                    result = DbDataProcessor.LastException;
                                    e.Abort = true;
                                    return;
                                }
                            }

                            outline.MinutesSpent += outlineUser.TimeSpent;
                            outline.TotalCost += outlineUser.Cost;
                        }

                        if (!context.SaveNoCommitEntity(outline, "Saving Error"))
                        {
                            result = DbDataProcessor.LastException;
                            e.Abort = true;
                            return;
                        }

                        if (outline.Id == Id)
                        {
                            RefreshCost(outlineUsers);
                        }
                    }

                    currentOutline++;
                }
            };
            lookupData.GetPrintData();
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

        private static void UpdateOutlineUserCost(IQueryable<User> usersTable, TestingOutlineCost outlineUser, IQueryable<TimeClock> timeClocksTable,
            TestingOutline outline)
        {
            var user = usersTable.FirstOrDefault(p => p.Id == outlineUser.UserId);
            var outlineUserMinutes = timeClocksTable
                .Where(p => p.TestingOutlineId == outline.Id
                            && p.UserId == outlineUser.UserId)
                .ToList()
                .Sum(p => p.MinutesSpent);
            if (outlineUserMinutes != null)
            {
                outlineUser.TimeSpent = outlineUserMinutes.Value;
                outlineUser.Cost = Math.Round((outlineUserMinutes.Value / 60) * user.HourlyRate, 2);
            }
        }

        public bool ChangeProduct()
        {
            if (DetailsGridManager.CompletedRowsExist())
            {
                var oldProduct = OldProductValue.GetEntity<Product>();
                var currentProduct = ProductValue.GetEntity<Product>();

                if (currentProduct.Id != oldProduct.Id)
                {
                    var message =
                        "Changing the Product will force a retest on this Testing Outline.  Are you sure you want to do this?";
                    var caption = "Force Retest?";
                    var result = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true);

                    if (result == MessageBoxButtonsResult.Yes)
                    {
                        Retest();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            OldProductValue = ProductValue;
            return true;
        }

        private void OnErrorAddModify()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                ErrorLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        public void RefreshTimeClockLookup()
        {
            TimeClockLookupCommand = GetLookupCommand(LookupCommands.Refresh);
        }

    }
}
