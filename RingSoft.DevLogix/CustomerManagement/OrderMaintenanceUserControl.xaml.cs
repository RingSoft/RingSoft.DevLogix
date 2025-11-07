using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;

namespace RingSoft.DevLogix.CustomerManagement
{
    /// <summary>
    /// Interaction logic for OrderMaintenanceUserControl.xaml
    /// </summary>
    public partial class OrderMaintenanceUserControl : IOrderView
    {
        public OrderMaintenanceUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(OrderIdControl);

            Loaded += (sender, args) =>
            {
                TopHeaderControl.PrintButton.Content = "_Print Invoice";
            };
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
            return "Order";
        }

        protected override void ShowRecordTitle()
        {
            Host.ChangeTitle($"Order - {LocalViewModel.CustomerAutoFillValue.Text}");
        }

        public void GotoProductsTab()
        {
            TabControl.SelectedItem = ProductsTabItem;
        }
    }
}
