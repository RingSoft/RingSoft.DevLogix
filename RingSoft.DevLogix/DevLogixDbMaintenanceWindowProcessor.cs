using System.Windows;
using RingSoft.App.Controls;
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
        public override IDbMaintenanceProcessor GetProcessor()
        {
            return new DevLogixDbMaintenanceWindowProcessor();
        }
    }

    public class DevLogixDbMaintenanceWindowProcessor : AppDbMaintenanceWindowProcessor
    {
        public override void SetupControl(IDbMaintenanceView view)
        {
            base.SetupControl(view);

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

            if (!ViewModel.TableDefinitionBase.HasRight(RightTypes.AllowEdit))
            {
                if (SelectButton.Visibility == Visibility.Collapsed)
                {
                    if (MaintenanceButtonsControl is DbMaintenanceTopHeaderControl topHeaderControl)
                    {
                        topHeaderControl.SetWindowReadOnlyMode();
                    }
                }
                View.SetReadOnlyMode(true);
            }
            else
            {
                SaveButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
