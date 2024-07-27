using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for SystemPreferencesWindow.xaml
    /// </summary>
    public partial class SystemPreferencesWindow
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "System Preferences";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public SystemPreferencesWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                TopHeaderControl.Visibility = Visibility.Collapsed;
                DataEntryGrid.PreviewKeyDown += (o, eventArgs) =>
                {
                    if (eventArgs.Key == Key.Enter)
                    {
                        if (DataEntryGrid.EditingControlHost != null)
                        {
                            if (DataEntryGrid.EditingControlHost.HasDataChanged())
                            {
                                DataEntryGrid.EditingControlHost.Row.IsNew = false;
                                DataEntryGrid.EditingControlHost.Row.SetCellValue(DataEntryGrid.EditingControlHost
                                    .GetCellValue());
                            }
                        }

                        if (DataEntryGrid.EditingControlHost != null &&
                            !DataEntryGrid.EditingControlHost.IsDropDownOpen)
                        {
                            LocalViewModel.OkCommand.Execute(null);
                            eventArgs.Handled = true;
                        }
                    }
                };

            };
            ContentRendered += (sender, args) => DataEntryGrid.Focus();
        }
    }
}
