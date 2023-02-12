using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public class DevLogixChartViewModel : DevLogixDbMaintenanceViewModel<DevLogixChart>
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

        private DevLogixChartBarManager _barsManager;

        public DevLogixChartBarManager BarsManager
        {
            get => _barsManager;
            set
            {
                if (_barsManager == value)
                {
                    return;
                }
                _barsManager = value;
                OnPropertyChanged();
            }
        }

        public override TableDefinition<DevLogixChart> TableDefinition => AppGlobals.LookupContext.DevLogixCharts;

        public DevLogixChartViewModel()
        {
            BarsManager = new DevLogixChartBarManager(this);
        }

        protected override DevLogixChart PopulatePrimaryKeyControls(DevLogixChart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<DevLogixChart> query = AppGlobals.DataRepository.GetDataContext().GetTable<DevLogixChart>();

            var result = query.Include(p => p.ChartBars)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = result.Id;
            KeyAutoFillValue = AppGlobals.LookupContext.OnAutoFillTextRequest(TableDefinition, Id.ToString());
            
            return result;

        }

        protected override void LoadFromEntity(DevLogixChart entity)
        {
            BarsManager.LoadGrid(entity.ChartBars);
        }

        protected override DevLogixChart GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            BarsManager.SetupForNewRecord();
        }

        protected override bool SaveEntity(DevLogixChart entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
