using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class TerritoryViewModel : DevLogixDbMaintenanceViewModel<Territory>
    {
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

        protected override Territory PopulatePrimaryKeyControls(Territory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Territory>();
            var result = table
                .Include(p => p.Salesperson)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = newEntity.Id;
            KeyAutoFillValue = result.GetAutoFillValue();
            return result;
        }

        protected override void LoadFromEntity(Territory entity)
        {
            
        }

        protected override Territory GetEntityData()
        {
            var result = new Territory()
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
            };
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
        }

        protected override bool SaveEntity(Territory entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, "Saving Territory");
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Territory>();
            var entity = table.FirstOrDefault(p => p.Id == Id);
            var result = true;
            if (entity != null)
            {
                result = context.DeleteEntity(entity, "Deleting Territory");
            }
            return result;
        }
    }
}
