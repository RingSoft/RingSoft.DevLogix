using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class SystemPreferencesViewModel : DevLogixDbMaintenanceViewModel<SystemPreferences>
    {
        public override TableDefinition<SystemPreferences> TableDefinition =>
            AppGlobals.LookupContext.SystemPreferences;

        private SystemPreferencesHolidaysManager  _holidaysManager;

        public SystemPreferencesHolidaysManager  HolidaysManager
        {
            get => _holidaysManager;
            set
            {
                if (_holidaysManager == value)
                {
                    return;
                }
                _holidaysManager = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public SystemPreferencesViewModel()
        {
            HolidaysManager = new SystemPreferencesHolidaysManager(this);
            OkCommand = new RelayCommand((() =>
            {
                OnOk();
            }));
            CancelCommand = new RelayCommand((() =>
            {
                OnCancel();
            }));
        }
        protected override void Initialize()
        {
            base.Initialize();
            OnGotoNextButton();
        }

        protected override SystemPreferences PopulatePrimaryKeyControls(SystemPreferences newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<SystemPreferences>();
            var result = table
                .Include(p => p.Holidays)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            if (result != null)
            {
                Id = result.Id;
            }
            return result;
        }

        protected override void LoadFromEntity(SystemPreferences entity)
        {
            HolidaysManager.LoadGrid(entity.Holidays);
        }

        protected override SystemPreferences GetEntityData()
        {
            return new SystemPreferences
            {
                Id = Id,
            };
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(SystemPreferences entity)
        {
            var newHolidays = HolidaysManager.GetEntityList();
            var context = AppGlobals.DataRepository.GetDataContext();
            var existHolidays = context.GetTable<SystemPreferencesHolidays>()
                .Where(p => p.SystemPreferencesId == Id).ToList();
            if (existHolidays != null)
            {
                context.RemoveRange(existHolidays);
            }
            context.AddRange(newHolidays);
            return context.Commit("Saving Holidays");
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        private void OnOk()
        {
            if (DoSave() == DbMaintenanceResults.Success)
            {
                Processor.CloseWindow();
            }
        }

        private void OnCancel()
        {
            Processor.CloseWindow();
        }
    }
}
