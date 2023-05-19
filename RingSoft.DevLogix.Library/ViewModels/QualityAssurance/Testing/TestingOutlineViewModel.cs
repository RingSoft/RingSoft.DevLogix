using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public interface ITestingOutlineView : IDbMaintenanceView
    {
        void PunchIn(TestingOutline testingOutline);
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
                OnPropertyChanged();
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

        public new ITestingOutlineView  View { get; private set; }

        public RelayCommand GenerateDetailsCommand { get; private set; }

        public RelayCommand RetestCommand { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

        public decimal MinutesSpent { get; private set; }

        public AutoFillValue DefaultProductAutoFillValue { get; private set; }

        public TestingOutlineViewModel()
        {
            GenerateDetailsCommand = new RelayCommand(GenerateDetails);
            RetestCommand = new RelayCommand(Retest);
            PunchInCommand = new RelayCommand(PunchIn);

            ProductSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProductId));
            CreatedByAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CreatedByUserId));
            AssignedToAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.AssignedToUserId));

            DetailsGridManager = new TestingOutlineDetailsGridManager(this);
            TemplatesGridManager = new TestingOutlineTemplatesGridManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.TestingOutlineDetails);
            TablesToDelete.Add(AppGlobals.LookupContext.TestingOutlineTemplates);
            TestingOutlineCostManager = new TestingOutlineCostManager(this);
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
            return result;
        }

        private static TestingOutline? GetTestingOutline(int id)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
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
            ProductValue = entity.Product.GetAutoFillValue();
            CreatedByAutoFillValue = entity.CreatedByUser.GetAutoFillValue();
            AssignedToAutoFillValue = entity.AssignedToUser.GetAutoFillValue();
            DueDate = entity.DueDate;
            if (DueDate != null)
            {
                DueDate = DueDate.Value.ToLocalTime();
            }
            PercentComplete = entity.PercentComplete;
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

        protected override void ClearData()
        {
            Id = 0;
            ProductValue = DefaultProductAutoFillValue;
            CreatedByAutoFillValue = null;
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

        }

        private void Retest()
        {

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
    }
}
