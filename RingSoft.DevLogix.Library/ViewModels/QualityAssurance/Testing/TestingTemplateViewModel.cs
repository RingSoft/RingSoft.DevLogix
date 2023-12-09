using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingTemplateViewModel : DevLogixDbMaintenanceViewModel<TestingTemplate>
    {
        public override TableDefinition<TestingTemplate> TableDefinition => AppGlobals.LookupContext.TestingTemplates;

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

        private AutoFillSetup _baseAutoFillSetup;

        public AutoFillSetup BaseAutoFillSetup
        {
            get => _baseAutoFillSetup;
            set
            {
                if (_baseAutoFillSetup == value)
                    return;

                _baseAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _baseAutoFillValue;

        public AutoFillValue BaseAutoFillValue
        {
            get => _baseAutoFillValue;
            set
            {
                if (_baseAutoFillValue == value)
                    return;

                _baseAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private TestingTemplateItemManager _testingTemplateItemManager;

        public TestingTemplateItemManager TestingTemplateItemManager
        {
            get => _testingTemplateItemManager;
            set
            {
                if (_testingTemplateItemManager == value)
                    return;

                _testingTemplateItemManager = value;
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

        public RelayCommand UpdateOutlinesCommand { get; set; }


        public TestingTemplateViewModel()
        {
            BaseAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.TestingTemplates.GetFieldDefinition(p => p.BaseTemplateId));

            UpdateOutlinesCommand = new RelayCommand(UpdateTestingOutlines);
            TestingTemplateItemManager = new TestingTemplateItemManager(this);

            TablesToDelete.Add(AppGlobals.LookupContext.TestingTemplatesItems);
        }

        protected override void PopulatePrimaryKeyControls(TestingTemplate newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            BaseAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            var idField = TableDefinition.GetFieldDefinition(p => p.Id);
            BaseAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(idField, Conditions.NotEquals, Id);
        }

        protected override TestingTemplate GetEntityFromDb(TestingTemplate newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingTemplate>();
            var result = table
                .Include(p => p.BaseTemplate)
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            return result;
        }

        protected override void LoadFromEntity(TestingTemplate entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            BaseAutoFillValue = entity.BaseTemplate.GetAutoFillValue();
            TestingTemplateItemManager.LoadGrid(entity.Items);
            Notes = entity.Notes;
        }

        protected override TestingTemplate GetEntityData()
        {
            var result = new TestingTemplate
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                BaseTemplateId = BaseAutoFillValue.GetEntity<TestingTemplate>().Id,
                Notes = Notes,
            };
            if (result.BaseTemplateId == 0)
            {
                result.BaseTemplateId = null;
            }
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            BaseAutoFillValue = null;
            BaseAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            TestingTemplateItemManager.SetupForNewRecord();
        }

        protected override bool SaveEntity(TestingTemplate entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var result = context.SaveEntity(entity, "Saving Testing Template");
            if (result)
            {
                var items = TestingTemplateItemManager.GetEntityList();
                foreach (var item in items)
                {
                    item.TestingTemplateId = entity.Id;
                }

                var existingItems = context.GetTable<TestingTemplateItem>()
                    .Where(p => p.TestingTemplateId == Id);
                context.RemoveRange(existingItems);
                context.AddRange(items);
                result = context.Commit("Saving Items");
            }
            return result;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingTemplate>();
            var entity = table.FirstOrDefault(p => p.Id == Id);

            var existingItems = context.GetTable<TestingTemplateItem>()
                .Where(p => p.TestingTemplateId == Id);
            context.RemoveRange(existingItems);

            return context.DeleteEntity(entity, "Deleting Testing Template");
        }

        private void UpdateTestingOutlines()
        {
            if (DoSave() == DbMaintenanceResults.Success)
            {
                TestingTemplateItemManager.UpdateTestingOutlines();
            }
        }
    }
}
