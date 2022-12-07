using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ProductViewModel : DevLogixDbMaintenanceViewModel<Product>
    {
        public override TableDefinition<Product> TableDefinition => AppGlobals.LookupContext.Products;

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

        private string? _notes;

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

        protected override Product PopulatePrimaryKeyControls(Product newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<Product>();
            var result = query.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
                KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            }

            return result;
        }

        protected override void LoadFromEntity(Product entity)
        {
            Notes = entity.Notes;
        }

        protected override Product GetEntityData()
        {
            var result = new Product()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Notes = Notes
            };

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            Notes = null;
        }

        protected override bool SaveEntity(Product entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                return context.SaveEntity(entity, $"Saving Product '{entity.Description}'");
            }
            return false;

        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<Product>();
            if (query != null)
            {
                var entity = query.FirstOrDefault(p => p.Id == Id);
                var context = AppGlobals.DataRepository.GetDataContext();
                if (context != null && entity != null)
                {
                    return context.DeleteEntity(entity, $"Deleting Product '{entity.Description}'");
                }
            }
            return false;

        }
    }
}
