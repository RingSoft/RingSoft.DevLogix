using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing
{
    public class TestingTemplateViewModel : DbMaintenanceViewModel<TestingTemplate>
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

        #endregion

        public RelayCommand UpdateOutlinesCommand { get; set; }


        public TestingTemplateViewModel()
        {
            BaseAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.TestingTemplates.GetFieldDefinition(p => p.BaseTemplateId));

            UpdateOutlinesCommand = new RelayCommand(UpdateTestingOutlines);
            TestingTemplateItemManager = new TestingTemplateItemManager(this);
            RegisterGrid(TestingTemplateItemManager);
        }

        protected override void PopulatePrimaryKeyControls(TestingTemplate newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;

            BaseAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            var idField = TableDefinition.GetFieldDefinition(p => p.Id);
            BaseAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(idField, Conditions.NotEquals, Id);
        }

        protected override void LoadFromEntity(TestingTemplate entity)
        {
            BaseAutoFillValue = entity.BaseTemplate.GetAutoFillValue();
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
