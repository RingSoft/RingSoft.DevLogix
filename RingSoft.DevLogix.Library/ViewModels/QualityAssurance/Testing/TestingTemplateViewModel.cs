using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
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

        public TestingTemplateViewModel()
        {
            BaseAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.TestingTemplates.GetFieldDefinition(p => p.BaseTemplateId));
        }

        protected override TestingTemplate PopulatePrimaryKeyControls(TestingTemplate newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingTemplate>();
            var result = table
                .Include(p => p.BaseTemplate)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = result.Id;

            BaseAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
            var idField = TableDefinition.GetFieldDefinition(p => p.Id);
            BaseAutoFillSetup.LookupDefinition.FilterDefinition.AddFixedFilter(idField, Conditions.NotEquals, Id);

            return result;
        }

        protected override void LoadFromEntity(TestingTemplate entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            BaseAutoFillValue = entity.BaseTemplate.GetAutoFillValue();
        }

        protected override TestingTemplate GetEntityData()
        {
            var result = new TestingTemplate
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                BaseTemplateId = BaseAutoFillValue.GetEntity<TestingTemplate>().Id,
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

        protected override bool SaveEntity(TestingTemplate entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Testing Template");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TestingTemplate>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            return context.DeleteEntity(entity, "Deleting Testing Template");
        }
    }
}
