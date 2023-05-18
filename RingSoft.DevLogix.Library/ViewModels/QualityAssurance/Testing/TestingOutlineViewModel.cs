using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
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



        public RelayCommand GenerateDetailsCommand { get; private set; }

        public RelayCommand RetestCommand { get; private set; }

        public RelayCommand PunchInCommand { get; private set; }

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
        }

        protected override TestingOutline PopulatePrimaryKeyControls(TestingOutline newEntity, PrimaryKeyValue primaryKeyValue)
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
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = result.Id;
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
            ProductValue = null;
            CreatedByAutoFillValue = null;
            AssignedToAutoFillValue = null;
            DueDate = null;
            PercentComplete = 0;
            Notes = null;
            DetailsGridManager.SetupForNewRecord();
            TemplatesGridManager.SetupForNewRecord();
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

        }
    }
}
