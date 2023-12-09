using System.Linq;
using System.Xml.Linq;
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

        private double _cost;

        public double Cost
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

        private string? _comment;

        public string? Comment
        {
            get => _comment;
            set
            {
                if (_comment == value)
                    return;

                _comment = value;
                OnPropertyChanged();
            }
        }


        protected override void PopulatePrimaryKeyControls(MaterialPart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(MaterialPart entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            Cost = entity.Cost;
            Comment = entity.Comment;
        }

        protected override MaterialPart GetEntityData()
        {
            return new MaterialPart
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                Cost = Cost,
                Comment = Comment,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            Cost = 0;
            Comment = null;
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
