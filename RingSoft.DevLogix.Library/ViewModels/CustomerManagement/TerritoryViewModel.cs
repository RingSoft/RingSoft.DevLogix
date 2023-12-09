using System.Linq;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;
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

        private AutoFillSetup _salespersonAutoFillSetup;

        public AutoFillSetup SalespersonAutoFillSetup
        {
            get => _salespersonAutoFillSetup;
            set
            {
                if (_salespersonAutoFillSetup == value)
                    return;

                _salespersonAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _salespersonAutoFillValue;

        public AutoFillValue SalespersonAutoFillValue
        {
            get => _salespersonAutoFillValue;
            set
            {
                if (_salespersonAutoFillValue == value)
                    return;

                _salespersonAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        public TerritoryViewModel()
        {
            SalespersonAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.SalespersonId));

        }

        protected override void PopulatePrimaryKeyControls(Territory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(Territory entity)
        {
            SalespersonAutoFillValue = entity.Salesperson.GetAutoFillValue();
        }

        protected override Territory GetEntityData()
        {
            var result = new Territory()
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                SalespersonId = SalespersonAutoFillValue.GetEntity<User>().Id,
            };
            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            SalespersonAutoFillValue = null;
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
