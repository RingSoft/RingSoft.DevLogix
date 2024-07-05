using System.ComponentModel;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IChartWindowView : IDbMaintenanceView
    {
        public void OnValGridFail();
    }

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

        public new IChartWindowView View { get; set; }

        public ChartBarsViewModel ChartViewModel { get; set; }

        public DevLogixChartViewModel()
        {
            BarsManager = new DevLogixChartBarManager(this);
            TablesToDelete.Add(AppGlobals.LookupContext.DevLogixChartBars);
        }

        protected override void Initialize()
        {
            if (base.View is IChartWindowView view)
            {
                View = view;
            }
            base.Initialize();
        }

        public void SetChartViewModel(ChartBarsViewModel chartBarsViewModel)
        {
            if (ChartViewModel != null)
            {
                return;
            }
            ChartViewModel = chartBarsViewModel;
            if (MaintenanceMode == DbMaintenanceModes.EditMode)
            {
                RefreshChart();
            }
        }

        public void RefreshChart()
        {
            if (ChartViewModel == null)
            {
                return;
            }
            ChartViewModel.SetChartBars(BarsManager.GetRowsList());
            if (AppGlobals.MainViewModel.ChartViewModel.ChartId == Id)
            {
                AppGlobals.MainViewModel.ChartViewModel.SetChartBars(Id);
            }
        }

        protected override void PopulatePrimaryKeyControls(DevLogixChart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            AppGlobals.MainViewModel.ChartViewModel.DisableBalloons(AppGlobals.MainViewModel.ChartViewModel.ChartId == Id);
        }

        protected override DevLogixChart GetEntityFromDb(DevLogixChart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<DevLogixChart> query = AppGlobals.DataRepository.GetDataContext().GetTable<DevLogixChart>();

            var result = query.Include(p => p.ChartBars)
                .FirstOrDefault(p => p.Id == newEntity.Id);
            Id = newEntity.Id;//Peter Ringering - 07/05/2024 03:13:28 PM - E-60
            return result;
        }

        protected override void LoadFromEntity(DevLogixChart entity)
        {
            BarsManager.LoadGrid(entity.ChartBars);

            RefreshChart();
        }

        protected override DevLogixChart GetEntityData()
        {
            return new DevLogixChart()
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            BarsManager.SetupForNewRecord();
            ChartViewModel?.Clear(true);
        }

        protected override bool ValidateEntity(DevLogixChart entity)
        {
            if (!BarsManager.ValidateGrid())
            {
                return false;
            }

            return base.ValidateEntity(entity);
        }

        protected override bool SaveEntity(DevLogixChart entity)
        {
            var bars = BarsManager.GetRowsList();

            var context = AppGlobals.DataRepository.GetDataContext();
            if (context.SaveEntity(entity, "Saving Chart"))
            {
                var barsQuery = context.GetTable<DevLogixChartBar>();
                var oldBars = barsQuery.Where(p => p.ChartId == Id).ToList();
                context.RemoveRange(oldBars);

                foreach (var devLogixChartBar in bars)
                {
                    devLogixChartBar.ChartId = entity.Id;
                }
                context.AddRange(bars);
                if (context.Commit("Saving Bars"))
                {
                    if (AppGlobals.MainViewModel.ChartViewModel.ChartId == Id)
                    {
                        AppGlobals.MainViewModel.SetChartId(Id);
                    }
                    return true;
                }
            }

            return false;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var bars = context.GetTable<DevLogixChartBar>().Where(p => p.ChartId == Id).ToList();
            context.RemoveRange(bars);

            var chart = context.GetTable<DevLogixChart>().FirstOrDefault(p => p.Id == Id);
            if (context.DeleteNoCommitEntity(chart, "Deleting Chart"))
            {
                var result= context.Commit("Deleting Chart");
                if (result)
                {
                    if (chart.Id == AppGlobals.MainViewModel.ChartViewModel.ChartId)
                    {
                        AppGlobals.MainViewModel.SetChartId(0);
                    }
                }

                return result;
            }

            return false;
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            AppGlobals.MainViewModel.ChartViewModel.DisableBalloons(false);
            base.OnWindowClosing(e);
        }
    }
}
