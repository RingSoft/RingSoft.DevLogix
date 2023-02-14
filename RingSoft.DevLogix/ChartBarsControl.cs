using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.Devices;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using ScottPlot;
using ScottPlot.Plottable;
using Mouse = System.Windows.Input.Mouse;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix;assembly=RingSoft.DevLogix"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChartBarsControl/>
    ///
    /// </summary>
    public class ChartBarsControl : Control, IChartBarsView
    {
        public ScottPlot.WpfPlot WpfPlot { get; private set; }

        public BarSeries BarPlot { get; private set; }

        public Border Border { get; private set; }

        public ChartBarsViewModel ViewModel { get; private set; }

        public List<DevLogixChartBar> DevLogixChartBars { get; private set; }

        private int _yLimit;
        private bool _closed;

        static ChartBarsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartBarsControl), new FrameworkPropertyMetadata(typeof(ChartBarsControl)));
        }

        public ChartBarsControl()
        {
        }

        public override void OnApplyTemplate()
        {
            WpfPlot = GetTemplateChild(nameof(WpfPlot)) as ScottPlot.WpfPlot;
            Border = GetTemplateChild(nameof(Border)) as Border;

            if (Border == null)
            {
                throw new ApplicationException("Need to set Border");
            }

            ViewModel = Border.TryFindResource("ViewModel") as ChartBarsViewModel;

            ViewModel.Initialize(this);

            WpfPlot.LeftClicked += WpfPlot_LeftClicked;

            var window = Window.GetWindow(this);
            window.Closed += (sender, args) =>
            {
                _closed = true;
                ViewModel.Clear(false);
            };
            UpdateBars();
            base.OnApplyTemplate();
        }

        private void WpfPlot_LeftClicked(object? sender, EventArgs e)
        {
            (double mouseCoordX, double mouseCoordY) = WpfPlot.GetMouseCoordinates();
            var newBar = BarPlot.GetBar(new Coordinate(mouseCoordX, mouseCoordY));

            if (newBar != null)
            {
                var index = BarPlot.Bars.IndexOf(newBar);
                if (index >= 0)
                {
                    var viewModel = ViewModel.Bars[index];
                    viewModel.ShowAddOnFly(Window.GetWindow(this));
                }
            }

        }

        public void UpdateBars()
        {
            _yLimit = 0;
            var bars = RedrawBars();
            WpfPlot.Plot.Clear();
            BarPlot = WpfPlot.Plot.AddBarSeries(bars);
            SetYLimit();
            //WpfPlot.Plot.AxisAutoY(0.4);
            //WpfPlot.Plot.YAxis.SetSizeLimit(0);
            WpfPlot.Plot.XAxis.IsVisible = false;
            WpfPlot.Plot.Style(dataBackground: Color.Transparent, figureBackground: Color.Transparent);
            RefreshPlot();
        }

        private List<Bar> RedrawBars()
        {
            List<Bar> bars = new();
            var position = 0;
            
            foreach (var barViewModel in ViewModel.Bars)
            {
                Bar bar = new()
                {
                };

                UpdateBar(bar, barViewModel, position);
                bars.Add(bar);
                position++;
            }
            return bars;
        }

        private void UpdateBar(Bar bar, ChartBarViewModel barViewModel, int position)
        {
            var numFormat = GblMethods.GetNumFormat(0, false);
            bar.Value = barViewModel.Count;
            bar.FillColor = ScottPlot.Palette.Category10.GetColor(position);
            bar.Label = barViewModel.Count.ToString(numFormat) + $"\r\n{barViewModel.ChartBar.Name}";
            bar.LineWidth = 2;
            bar.Position = position;
        }
        private void SetYLimit()
        {
            _yLimit = 0;
            foreach (var barViewModel in ViewModel.Bars)
            {
                if (barViewModel.Count > _yLimit)
                {
                    _yLimit = barViewModel.Count;
                }
            }
            WpfPlot.Plot.SetAxisLimitsY(0, _yLimit * 1.2);
            RefreshPlot();
        }

        public void RefreshChart()
        {
            UpdateBars();
        }

        private void RefreshPlot()
        {
            Dispatcher.Invoke(() =>
            {
                WpfPlot.Refresh();
            });
        }

        public void SetAlertLevel(AlertLevels level, ChartBarViewModel viewModel)
        {
            if (_closed)
            {
                return;
            }
            Dispatcher.Invoke(() =>
            {
                var message = viewModel.LookupRefresher.GetRecordCountMessage(viewModel.Count, viewModel.ChartBar.Name);
                LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(level, viewModel.LookupRefresher.Disabled
                    , Window.GetWindow(this), message);
            });
        }

        public void UpdateBar(ChartBarViewModel bar)
        {
            var position = bar.MainViewModel.Bars.IndexOf(bar);
            if (position == -1)
                return;

            var chartBar = BarPlot.Bars[position];
            UpdateBar(chartBar, bar, position);
            SetYLimit();
            RefreshPlot();
        }
    }
}
