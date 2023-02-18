using System.Windows;
using System.Windows.Input;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for DevLogixChartMaintenanceWindow.xaml
    /// </summary>
    public partial class DevLogixChartMaintenanceWindow : IChartWindowView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Chart";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

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
