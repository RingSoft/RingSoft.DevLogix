using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IChartBarsView
    {
        void RefreshChart();

        void SetAlertLevel(AlertLevels level, ChartBarViewModel viewModel);

        void UpdateBar(ChartBarViewModel bar);
    }

    public class ChartBarViewModel
    {
        public ChartBarsViewModel MainViewModel { get; private set; }

        public LookupDefinitionBase LookupDefinition { get; private set; }

        public LookupRefresher LookupRefresher { get; private set; }

        public DevLogixChartBar ChartBar { get; private set; }

        public LookupDataMauiBase LookupData { get; private set; }

        public int Count { get; private set; }

        public bool OrigDisableBalloon { get; private set; }

        public bool DoGetRecordCount { get; private set; }

        public ChartBarViewModel(ChartBarsViewModel mainViewModel, DevLogixChartBar chartBar)
        {
            MainViewModel = mainViewModel;
            ChartBar = chartBar;
            DoGetRecordCount = true;
            Initialize();
        }

        private void Initialize()
        {
            if (LookupRefresher != null)
            {
                LookupRefresher.SetAlertLevelEvent -= LookupRefresher_SetAlertLevelEvent;

                LookupRefresher.RefreshRecordCountEvent -= LookupRefresher_RefreshRecordCountEvent;
            }
            LookupRefresher = new LookupRefresher();
            LookupDefinition = new LookupDefinitionBase(ChartBar.AdvancedFindId, LookupRefresher);
            OrigDisableBalloon = LookupRefresher.Disabled;

            LookupData = LookupDefinition.TableDefinition.LookupDefinition.GetLookupDataMaui(LookupDefinition, false);

            LookupRefresher.SetAlertLevelEvent += LookupRefresher_SetAlertLevelEvent;

            LookupRefresher.RefreshRecordCountEvent += LookupRefresher_RefreshRecordCountEvent;
            GetRecordCount();
            LookupRefresher.StartRefresh();
        }

        private void LookupRefresher_RefreshRecordCountEvent(object? sender, EventArgs e)
        {
            GetRecordCount();
            MainViewModel.View.UpdateBar(this);
        }

        private void LookupRefresher_SetAlertLevelEvent(object? sender, RefreshAlertLevelArgs e)
        {
            MainViewModel.View.SetAlertLevel(e.AlertLevel, this);
        }

        private void GetRecordCount()
        {
            Count = LookupData.GetRecordCount();

            if (DoGetRecordCount)
            {
                if (LookupRefresher.RefreshRate != RefreshRate.None)
                {
                    LookupRefresher.UpdateRecordCount(Count);
                }
            }

            //if (LookupRefresher.RefreshRate != RefreshRate.None)
            //{
            //    LookupRefresher.UpdateRecordCount(Count);
            //}
        }

        public void ShowAddOnFly(object ownerWindow)
        {
            var advFindId = ChartBar.AdvancedFindId;
            var advFind = new AdvancedFind
            {
                Id = advFindId,
            };
            var primaryKey = AppGlobals.LookupContext.AdvancedFinds.GetPrimaryKeyValueFromEntity(advFind);
            var advFindLookup = AppGlobals.LookupContext.AdvancedFindLookup.Clone();
            advFindLookup.WindowClosed += (sender, args) =>
            {
                //GetRecordCount();
                Initialize();
                MainViewModel.View.RefreshChart();
                DoGetRecordCount = true;
            };
            DoGetRecordCount = false;
            advFindLookup.ShowAddOnTheFlyWindow(primaryKey, null, ownerWindow);
        }

        public int PageSize => 1;
        public LookupSearchTypes SearchType => LookupSearchTypes.Equals;
        public string SearchText => string.Empty;
        public int SelectedIndex => 0;
        public void SetLookupIndex(int index)
        {
            
        }
    }
    public class ChartBarsViewModel : INotifyPropertyChanged
    {
        public IChartBarsView View { get; set; }

        public List<ChartBarViewModel> Bars { get; private set; } = new List<ChartBarViewModel>();

        public int ChartId { get; private set; }

        public void Initialize(IChartBarsView view)
        {
            View = view;
        }

        public void Clear(bool clearChart)
        {
            foreach (var chartBarViewModel in Bars)
            {
                chartBarViewModel.LookupRefresher.Dispose();
            }
            Bars.Clear();
            if (clearChart)
            {
                View.RefreshChart();
            }
        }

        public void DisableBalloons(bool disableBalloons = true)
        {
            foreach (var chartBarViewModel in Bars)
            {
                if (disableBalloons)
                {
                    chartBarViewModel.LookupRefresher.Disabled = true;
                }
                else
                {
                    chartBarViewModel.LookupRefresher.Disabled = chartBarViewModel.OrigDisableBalloon;
                }
            }
        }

        public void SetChartBars(int chartId)
        {
            ChartId = chartId;
            var context = AppGlobals.DataRepository.GetDataContext();
            var chartTable = context.GetTable<DevLogixChart>();
            var chart = chartTable.Include(p => p.ChartBars)
                .FirstOrDefault(p => p.Id == chartId);

            if (chart != null)
            {
                SetChartBars(chart.ChartBars.ToList());
            }
        }
        public void SetChartBars(List<DevLogixChartBar> bars)
        {
            if (Bars != null)
            {
                foreach (var chartBarViewModel in Bars)
                {
                    chartBarViewModel.LookupRefresher.Dispose();
                }
            }
            Bars = new List<ChartBarViewModel>();
            foreach (var devLogixChartBar in bars)
            {
                if (devLogixChartBar.AdvancedFindId > 0)
                {
                    var chartBarViewModel = new ChartBarViewModel(this, devLogixChartBar);
                    Bars.Add(chartBarViewModel);
                }
            }
            View.RefreshChart();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
