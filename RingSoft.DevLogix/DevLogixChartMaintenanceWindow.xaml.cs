using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for DevLogixChartMaintenanceWindow.xaml
    /// </summary>
    public partial class DevLogixChartMaintenanceWindow : IChartWindowView
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Chart";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public DevLogixChartMaintenanceWindow()
        {
            InitializeComponent();

            ChartBarsControl.Loaded += (sender, args) =>
            {
                LocalViewModel.SetChartViewModel(ChartBarsControl.ViewModel);
            };
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void GotoGrid()
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
