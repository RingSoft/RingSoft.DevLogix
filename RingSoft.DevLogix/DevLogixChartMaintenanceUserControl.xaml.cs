using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
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
