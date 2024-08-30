using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Linq;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class TerritoryViewModel : DbMaintenanceViewModel<Territory>
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

        #endregion

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
            SalespersonAutoFillValue = null;
        }
    }
}
