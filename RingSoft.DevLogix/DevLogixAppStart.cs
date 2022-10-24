using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.UserManagement;

namespace RingSoft.DevLogix
{
    public class DevLogixAppStart: AppStart
    {
        public DevLogixAppStart(Application application) : base(application, new MainWindow())
        {
            AppGlobals.InitSettings();

            LookupControlsGlobals.LookupControlContentTemplateFactory =
                new AppLookupContentTemplateFactory(application);
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            AppGlobals.Initialize();

            AppGlobals.LookupContext.LookupAddView += LookupContext_LookupAddView;

            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            LookupControlsGlobals.DbMaintenanceProcessorFactory = new DevLogixDbMaintenanceFactory();

            return base.DoProcess();
        }

        private void LookupContext_LookupAddView(object? sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.Users)
            {
                ShowAddOnTheFlyWindow(new UserMaintenanceWindow(), e);
            }

        }

        public void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            Window ownWindow = null;
            if (e.OwnerWindow is Window ownerWindow)
            {
                ownWindow = ownerWindow;
                maintenanceWindow.Owner = ownerWindow;
            }

            maintenanceWindow.ShowInTaskbar = false;

            maintenanceWindow.ViewModel.InitializeFromLookupData(e);

            maintenanceWindow.Loaded += (sender, args) =>
            {
                var processor = maintenanceWindow.ViewModel.Processor as AppDbMaintenanceWindowProcessor;
                processor.CheckAddOnFlyAfterLoaded();
            };
            maintenanceWindow.Show();
            maintenanceWindow.Activate();
            maintenanceWindow.Closed += (sender, args) => ownWindow?.Activate();
        }


        private void AppGlobals_AppSplashProgress(object? sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
