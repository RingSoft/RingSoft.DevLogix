using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.CustomerManagement;
using RingSoft.DevLogix.DataAccess;
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
            WindowRegistry.RegisterWindow<SystemPreferencesWindow>(AppGlobals.LookupContext.SystemPreferences);

            WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.Users);
            WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.UsersTimeOff);
            WindowRegistry.RegisterWindow<GroupsMaintenanceWindow>(AppGlobals.LookupContext.Groups);
            WindowRegistry.RegisterWindow<DepartmentMaintenanceWindow>(AppGlobals.LookupContext.Departments);
            WindowRegistry.RegisterWindow<TimeClockMaintenanceWindow>(AppGlobals.LookupContext.TimeClocks);
            WindowRegistry.RegisterWindow<UserTrackerMaintenanceWindow>(AppGlobals.LookupContext.UserTracker);

            WindowRegistry.RegisterWindow<ErrorStatusMaintenanceWindow>(AppGlobals.LookupContext.ErrorStatuses);
            WindowRegistry.RegisterWindow<ErrorPriorityMaintenanceWindow>(AppGlobals.LookupContext.ErrorPriorities);
            WindowRegistry.RegisterWindow<ProductMaintenanceWindow>(AppGlobals.LookupContext.Products);
            WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersions);
            WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersionDepartments);
            WindowRegistry.RegisterWindow<ErrorMaintenanceWindow>(AppGlobals.LookupContext.Errors);
            WindowRegistry.RegisterWindow<ErrorMaintenanceWindow>(AppGlobals.LookupContext.ErrorTesters);
            WindowRegistry.RegisterWindow<TestingTemplatesMaintenanceWindow>(AppGlobals.LookupContext.TestingTemplates);
            WindowRegistry.RegisterWindow<TestingOutlineMaintenanceWindow>(AppGlobals.LookupContext.TestingOutlines);

            WindowRegistry.RegisterWindow<ProjectMaintenanceWindow>(AppGlobals.LookupContext.Projects);
            WindowRegistry.RegisterWindow<ProjectMaintenanceWindow>(AppGlobals.LookupContext.ProjectUsers);
            WindowRegistry.RegisterWindow<ProjectTaskMaintenanceWindow>(AppGlobals.LookupContext.ProjectTasks);
            WindowRegistry.RegisterWindow<ProjectTaskMaintenanceWindow>(AppGlobals.LookupContext.ProjectTaskLaborParts);
            WindowRegistry.RegisterWindow<ProjectTaskMaintenanceWindow>(AppGlobals.LookupContext.ProjectTaskDependency);
            WindowRegistry.RegisterWindow<ProjectMaterialMaintenanceWindow>(AppGlobals.LookupContext.ProjectMaterials);
            WindowRegistry.RegisterWindow<LaborPartMaintenanceWindow>(AppGlobals.LookupContext.LaborParts);
            WindowRegistry.RegisterWindow<MaterialPartMaintenanceWindow>(AppGlobals.LookupContext.MaterialParts);
            WindowRegistry.RegisterWindow<ProjectMaterialHistoryWindow>(AppGlobals.LookupContext.ProjectMaterialHistory);

            WindowRegistry.RegisterWindow<CustomerMaintenanceWindow>(AppGlobals.LookupContext.Customer);
            WindowRegistry.RegisterWindow<TimeZoneMaintenanceWindow>(AppGlobals.LookupContext.TimeZone);
            WindowRegistry.RegisterWindow<TerritoryMaintenanceWindow>(AppGlobals.LookupContext.Territory);
            WindowRegistry.RegisterWindow<CustomerMaintenanceWindow>(AppGlobals.LookupContext.CustomerProduct);
            WindowRegistry.RegisterWindow<OrderMaintenanceWindow>(AppGlobals.LookupContext.Order);
            WindowRegistry.RegisterWindow<OrderMaintenanceWindow>(AppGlobals.LookupContext.OrderDetail);
            WindowRegistry.RegisterWindow<CustomerComputerMaintenanceWindow>(AppGlobals.LookupContext.CustomerComputer);
            WindowRegistry.RegisterWindow<SupportTicketMaintenanceWindow>(AppGlobals.LookupContext.SupportTicket);

            AppGlobals.LookupContext.FormatSearchForEvent += (sender, args) =>
            {
                if (args.SearchForHostId == DevLogixLookupContext.TimeSpentHostId)
                {
                    args.Value = AppGlobals.MakeTimeSpent(args.RawValue.ToDecimal().ToDouble());
                }

                if (args.SearchForHostId == DevLogixLookupContext.SpeedHostId)
                {
                    args.Value = AppGlobals.MakeSpeed(args.RawValue.ToDecimal().ToDouble());
                }

                if (args.SearchForHostId == DevLogixLookupContext.MemoryHostId)
                {
                    args.Value = AppGlobals.MakeSpace(args.RawValue.ToDecimal().ToDouble());
                }

            };
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

                if (args.DeleteMode)
                {
                    if (!args.TableDefinition.HasRight(RightTypes.AllowDelete))
                    {
                        args.AllowDelete = false;
                    }

                    if (args.LookupDefinition != null && args.LookupDefinition.TableDefinition == AppGlobals.LookupContext.Users)
                    {
                        var lookupToCheck = args.LookupDefinition.Clone();
                        var context = AppGlobals.DataRepository.GetDataContext();
                        var table = context.GetTable<User>();
                        var masterUser = table.FirstOrDefault();
                        if (masterUser != null)
                        {
                            var field = AppGlobals.LookupContext.Users
                                .GetFieldDefinition(p => p.Id);
                            lookupToCheck.FilterDefinition.AddFixedFilter(field, Conditions.Equals, masterUser.Id);
                            //var lookupUi = new LookupUserInterface()
                            //{
                            //    PageSize = 10,
                            //};
                            var lookupData =
                                lookupToCheck.TableDefinition.LookupDefinition.GetLookupDataMaui(lookupToCheck, false);
                            var count = lookupData.GetRecordCount();
                            if (count > 0)
                            {
                                args.AllowDelete = false;
                                var message = $"Deleting the master user '{masterUser.Name}' is not allowed.";
                                var caption = "Delete Denied!";
                                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                            }
                        }
                    }
                }

                if (!args.TableDefinition.HasRight(RightTypes.AllowAdd))
                {
                    args.AllowAdd = false;
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
            LookupControlsGlobals.LookupControlSearchForFactory = new DevLogixLookupSearchFactory();
            WPFControlsGlobals.DataEntryGridHostFactory = new DevLogixGridCellFactory();

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
