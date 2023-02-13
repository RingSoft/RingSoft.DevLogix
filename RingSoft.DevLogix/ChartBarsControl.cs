using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.Devices;
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

        private int _yLimit;

        static ChartBarsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartBarsControl), new FrameworkPropertyMetadata(typeof(ChartBarsControl)));
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

            base.OnApplyTemplate();
        }

        private void WpfPlot_LeftClicked(object? sender, EventArgs e)
        {
            (double mouseCoordX, double mouseCoordY) = WpfPlot.GetMouseCoordinates();
            var newBar = BarPlot.GetBar(new Coordinate(mouseCoordX, mouseCoordY));

            if (newBar != null)
            {
                MessageBox.Show(newBar.Label, "");
            }

        }

        public void UpdateBars(List<DevLogixChartBar> rows)
        {
            _yLimit = 0;
            var bars = RedrawBars();
            WpfPlot.Plot.Clear();
            BarPlot = WpfPlot.Plot.AddBarSeries(bars);
            //WpfPlot.Plot.AxisAutoY(0.4);
            WpfPlot.Plot.SetAxisLimitsY(0, _yLimit * 1.2);
            //WpfPlot.Plot.YAxis.SetSizeLimit(0);
            WpfPlot.Plot.XAxis.IsVisible = false;
            WpfPlot.Refresh();
        }

        private List<Bar> RedrawBars()
        {
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 3; i++)
            {
                var barValue = new Random().Next();
                if (barValue > _yLimit)
                {
                    _yLimit = barValue;
                }
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = barValue,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = barValue.ToString() + $"\r\nPerson {i}",
                    LineWidth = 2,
                };
                var limits = bar.GetLimits();
                bars.Add(bar);
            }

            return bars;
        }

    }
}
