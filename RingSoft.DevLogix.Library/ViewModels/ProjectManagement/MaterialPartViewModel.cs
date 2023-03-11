using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class MaterialPartViewModel : DevLogixDbMaintenanceViewModel<MaterialPart>
    {
        public override TableDefinition<MaterialPart> TableDefinition => AppGlobals.LookupContext.MaterialParts;

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

        private decimal _cost;

        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost == value)
                    return;

                _cost = value;
                OnPropertyChanged();
            }
        }


        protected override MaterialPart PopulatePrimaryKeyControls(MaterialPart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var table = AppGlobals.DataRepository.GetDataContext().GetTable<MaterialPart>();
            var result = table.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
            }

            return result;
        }

        protected override void LoadFromEntity(MaterialPart entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            Cost = entity.Cost;
        }

        protected override MaterialPart GetEntityData()
        {
            return new MaterialPart
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Cost = Cost,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            Cost = 0;
        }

        protected override bool SaveEntity(MaterialPart entity)
        {
            var result = false;
            var context = AppGlobals.DataRepository.GetDataContext();
            result = context.SaveEntity(entity, "Saving Material Part");

            return result;
        }

        protected override bool DeleteEntity()
        {
            var result = true;
            var context = AppGlobals.DataRepository.GetDataContext();
            var entity = context.GetTable<MaterialPart>()
                .FirstOrDefault(p => p.Id == Id);
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting Material Part");
            }
            return result;
        }
    }
}
