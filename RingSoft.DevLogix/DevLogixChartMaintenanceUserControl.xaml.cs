using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.QualityAssurance;

namespace RingSoft.DevLogix
{
    public class ChartHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton RefreshButton { get; set; }

        static ChartHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartHeaderControl), new FrameworkPropertyMetadata(typeof(ChartHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RefreshButton = GetTemplateChild(nameof(RefreshButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for DevLogixChartMaintenanceUserControl.xaml
    /// </summary>
    public partial class DevLogixChartMaintenanceUserControl : IChartWindowView
    {
        public DevLogixChartMaintenanceUserControl()
        {
            InitializeComponent();
            ChartBarsControl.Loaded += (sender, args) =>
            {
                LocalViewModel.SetChartViewModel(ChartBarsControl.ViewModel);
            };

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ChartHeaderControl chartHeaderControl)
                {
                    chartHeaderControl.RefreshButton.Command =
                        LocalViewModel.RefreshChartCommand;

                    chartHeaderControl.RefreshButton.ToolTip.HeaderText = "Refresh Chart (Ctrl + H, Ctrl + R)";
                    chartHeaderControl.RefreshButton.ToolTip.DescriptionText = "Refresh this chart.";
                }
            };

            var hotKey = new HotKey(LocalViewModel.RefreshChartCommand);
            hotKey.AddKey(Key.H);
            hotKey.AddKey(Key.R);
            AddHotKey(hotKey);

            RegisterFormKeyControl(NameControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return LocalViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Chart";
        }

        public void OnValGridFail()
        {
            TabControl.SelectedItem = BarsTab;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (TabControl.SelectedIndex == 0)
            {
                ChartBarsControl.ProcessKeyDown(e);
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
