using System.Linq;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class LaborPartViewModel : DevLogixDbMaintenanceViewModel<LaborPart>
    {
        public override TableDefinition<LaborPart> TableDefinition => AppGlobals.LookupContext.LaborParts;

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

        private decimal _minutesCost;

        public decimal MinutesCost
        {
            get => _minutesCost;
            set
            {
                if (_minutesCost == value)
                    return;

                _minutesCost = value;
                OnPropertyChanged();
            }
        }

        protected override LaborPart PopulatePrimaryKeyControls(LaborPart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<LaborPart>();
            var result = table.FirstOrDefault(p => p.Id == newEntity.Id);
            if (result != null)
            {
                Id = result.Id;
            }

            return result;
        }

        protected override void LoadFromEntity(LaborPart entity)
        {
            KeyAutoFillValue = entity.GetAutoFillValue();
            MinutesCost = entity.MinutesCost;
        }

        protected override LaborPart GetEntityData()
        {
            return new LaborPart
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                MinutesCost = MinutesCost
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            MinutesCost = 0;
        }

        protected override bool SaveEntity(LaborPart entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Labor Part");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var laborPart = context.GetTable<LaborPart>().FirstOrDefault(p => p.Id == Id);
            if (laborPart != null)
            {
                return context.DeleteEntity(laborPart, "Deleting Labor Part");
            }

            return true;
        }
    }
}
