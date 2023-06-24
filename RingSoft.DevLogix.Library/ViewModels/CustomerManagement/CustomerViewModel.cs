using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class CustomerViewModel : DevLogixDbMaintenanceViewModel<Customer>
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

        public RelayCommand PunchInCommand { get; private set; }

        public RelayCommand RecalcCommand { get; private set; }

        public CustomerViewModel()
        {
            PunchInCommand = new RelayCommand(PunchIn);

            RecalcCommand = new RelayCommand(Recalc);
        }

        protected override Customer PopulatePrimaryKeyControls(Customer newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            var result = table
                .Include(p => p.TimeZone)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = newEntity.Id;
            return result;
        }

        protected override void LoadFromEntity(Customer entity)
        {
            
        }

        protected override Customer GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;

        }

        protected override bool SaveEntity(Customer entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        private void PunchIn()
        {

        }

        private void Recalc()
        {

        }
    }
}
