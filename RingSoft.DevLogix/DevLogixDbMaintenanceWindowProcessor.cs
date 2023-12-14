using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix
{
    public class DevLogixDbMaintenanceFactory : AppDbMaintenanceProcessorFactory
    {
        public DevLogixDbMaintenanceFactory()
        {
            
        }
        public override DbMaintenanceWindowProcessor GetProcessor()
        {
            return new DevLogixDbMaintenanceWindowProcessor();
        }
    }

    public class DevLogixDbMaintenanceWindowProcessor : AppDbMaintenanceWindowProcessor
    {
        private bool _rightsSet;

        public override void Initialize(BaseWindow window, Control buttonsControl, DbMaintenanceViewModelBase viewModel,
            IDbMaintenanceView view, DbMaintenanceStatusBar statusBar)
        {
            base.Initialize(window, buttonsControl, viewModel, view, statusBar);
        }

        protected override void SetupControl()
        {
            base.SetupControl();
            MaintenanceButtonsControl.Loaded += (sender, args) =>
            {
                if (!_rightsSet)
                {
                    _rightsSet = true;
                    SetRights();
                }
            };
            if (DeleteButton != null)
            {
                _rightsSet = true;
                SetRights();
            }
        }

        private void SetRights()
        {
            if (!ViewModel.TableDefinitionBase.HasRight(RightTypes.AllowDelete))
            {
                DeleteButton.Visibility = Visibility.Collapsed;
            }

            if (!ViewModel.TableDefinitionBase.HasRight(RightTypes.AllowAdd))
            {
                NewButton.Visibility = Visibility.Collapsed;
                if (ViewModel.LookupAddViewArgs == null ||
                    ViewModel.LookupAddViewArgs.LookupFormMode != LookupFormModes.View)
                {
                    ViewModel.OnGotoNextButton();
                }
            }
        }
    }
}
