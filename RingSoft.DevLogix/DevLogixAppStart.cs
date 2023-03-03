using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.ProjectManagement;
using RingSoft.DevLogix.QualityAssurance;
using RingSoft.DevLogix.UserManagement;

namespace RingSoft.DevLogix
{
    public class WindowRegistryItem
    {
        public TableDefinitionBase TableDefinition { get; set; }

        public Type MaintenanceWindow { get; set; }
    }
    public static class WindowRegistry
    {
        public static List<WindowRegistryItem> Items  { get; private set; } = new List<WindowRegistryItem>();

        public static void RegisterWindow<TWindow>(TableDefinitionBase tableDefinition) where TWindow : DbMaintenanceWindow, new()
        {
            Items.Add(new WindowRegistryItem
            {
                MaintenanceWindow = typeof(TWindow),
                TableDefinition = tableDefinition
            });
        }

        public static  DbMaintenanceWindow GetMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            var item = Items.FirstOrDefault(p => p.TableDefinition == tableDefinition);
            if (item != null)
            {
                var window = (DbMaintenanceWindow)Activator.CreateInstance(item.MaintenanceWindow);
                return window;
            }

            return null;
        }
    }
    public class DevLogixAppStart: AppStart
    {
        public DevLogixAppStart(Application application) : base(application, new MainWindow())
        {
            AppGlobals.InitSettings();

            LookupControlsGlobals.LookupControlContentTemplateFactory =
                new AppLookupContentTemplateFactory(application);
        }

        protected override void CheckVersion()
        {
#if DEBUG
            var app = RingSoftAppGlobals.IsAppVersionOld();
            if (app != null)
            {
                RingSoftAppGlobals.UserVersion = app.VersionName;
            }

#else
            base.CheckVersion();
#endif
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            AppGlobals.Initialize();

            AppGlobals.LookupContext.LookupAddView += LookupContext_LookupAddView;

            WindowRegistry.RegisterWindow<DevLogixChartMaintenanceWindow>(AppGlobals.LookupContext.DevLogixCharts);

            WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.Users);
            WindowRegistry.RegisterWindow<GroupsMaintenanceWindow>(AppGlobals.LookupContext.Groups);
            WindowRegistry.RegisterWindow<DepartmentMaintenanceWindow>(AppGlobals.LookupContext.Departments);
            WindowRegistry.RegisterWindow<TimeClockMaintenanceWindow>(AppGlobals.LookupContext.TimeClocks);

            WindowRegistry.RegisterWindow<ErrorStatusMaintenanceWindow>(AppGlobals.LookupContext.ErrorStatuses);
            WindowRegistry.RegisterWindow<ErrorPriorityMaintenanceWindow>(AppGlobals.LookupContext.ErrorPriorities);
            WindowRegistry.RegisterWindow<ProductMaintenanceWindow>(AppGlobals.LookupContext.Products);
            WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersions);
            WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersionDepartments);
            WindowRegistry.RegisterWindow<ErrorMaintenanceWindow>(AppGlobals.LookupContext.Errors);
            WindowRegistry.RegisterWindow<ErrorMaintenanceWindow>(AppGlobals.LookupContext.ErrorTesters);

            WindowRegistry.RegisterWindow<ProjectMaintenanceWindow>(AppGlobals.LookupContext.Projects);
            WindowRegistry.RegisterWindow<ProjectTaskMaintenanceWindow>(AppGlobals.LookupContext.ProjectTasks);
            WindowRegistry.RegisterWindow<LaborPartMaintenanceWindow>(AppGlobals.LookupContext.LaborParts);

            AppGlobals.LookupContext.CanProcessTableEvent += (sender, args) =>
            {
                if (!args.TableDefinition.HasRight(RightTypes.AllowView))
                {
                    args.AllowView = false;
                }

                if (!args.TableDefinition.HasRight(RightTypes.AllowEdit))
                {
                    args.AllowEdit = false;
                }

                if (!args.TableDefinition.HasRight(RightTypes.AllowDelete))
                {
                    args.AllowDelete = false;
                }
            };

            AppGlobals.LookupContext.GetUserAutoFillEvent += (sender, fill) =>
            {
                fill.UserAutoFill.AutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
                var query = AppGlobals.DataRepository.GetDataContext().GetTable<User>();
                var user = query.FirstOrDefault(p => p.Name == fill.UserName);
                if (user != null)
                {
                    fill.UserAutoFill.AutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.Users, user.Id.ToString());
                }
            };
            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            LookupControlsGlobals.DbMaintenanceProcessorFactory = new DevLogixDbMaintenanceFactory();

            return base.DoProcess();
        }

        private void LookupContext_LookupAddView(object? sender, DbLookup.Lookup.LookupAddViewArgs e)
        {
            var maintenanceWindow = WindowRegistry.GetMaintenanceWindow(e.LookupData.LookupDefinition.TableDefinition);
            if (maintenanceWindow != null) ShowAddOnTheFlyWindow(maintenanceWindow, e);
        }

        public void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            Window ownWindow = null;
            if (e.OwnerWindow is Window ownerWindow)
            {
                ownWindow = ownerWindow;
                maintenanceWindow.Owner = ownerWindow;
            }
            else
            {
                maintenanceWindow.Owner = Application.Current.MainWindow;
            }

            //maintenanceWindow.ShowInTaskbar = false;

            maintenanceWindow.ViewModel.InitializeFromLookupData(e);

            maintenanceWindow.Loaded += (sender, args) =>
            {
                var processor = maintenanceWindow.ViewModel.Processor as AppDbMaintenanceWindowProcessor;
                processor.CheckAddOnFlyAfterLoaded();
            };
            if (e.InputParameter is DialogInput dialogInput)
            {
                var inputResult = maintenanceWindow.ShowDialog();
                if (inputResult != null && inputResult.Value)
                {
                    dialogInput.DialogResult = inputResult.Value;
                }
            }
            else
            {
                maintenanceWindow.Show();
                maintenanceWindow.Activate();
                maintenanceWindow.Closed += (sender, args) => ownWindow?.Activate();
            }
        }


        private void AppGlobals_AppSplashProgress(object? sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
