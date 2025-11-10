using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using TimeZone = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.TimeZone;

namespace RingSoft.DevLogix
{
    public class DevLogixAppStart: AppStart
    {
        public DevLogixAppStart(Application application) : base(application, new MainWindow())
        {
            AppGlobals.InitSettings();

            var appLookupContentTemplateFactory = new AppLookupContentTemplateFactory(application);
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

            LookupControlsGlobals.WindowRegistry.RegisterUserControl<AdvancedFindUserControl>(AppGlobals.LookupContext.AdvancedFinds);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<DevLogixChartMaintenanceWindow>(AppGlobals.LookupContext.DevLogixCharts);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<DevLogixChartMaintenanceWindow>(AppGlobals.LookupContext.DevLogixChartBars);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<SystemPreferencesWindow>(AppGlobals.LookupContext.SystemPreferences);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.Users);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.UsersTimeOff);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<UserMaintenanceWindow>(AppGlobals.LookupContext.UseerMonthlySales);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<GroupsMaintenanceWindow>(AppGlobals.LookupContext.Groups);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<DepartmentMaintenanceWindow>(AppGlobals.LookupContext.Departments);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TimeClockMaintenanceWindow>(AppGlobals.LookupContext.TimeClocks);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<UserTrackerMaintenanceWindow>(AppGlobals.LookupContext.UserTracker);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProductMaintenanceWindow>(AppGlobals.LookupContext.Products);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersions);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProductVersionMaintenanceWindow>(AppGlobals.LookupContext.ProductVersionDepartments);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingTemplatesMaintenanceWindow>(AppGlobals.LookupContext.TestingTemplates);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingOutlineMaintenanceWindow>(AppGlobals.LookupContext.TestingOutlines);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingOutlineMaintenanceWindow>(AppGlobals.LookupContext.TestingOutlineDetails);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingOutlineMaintenanceWindow>(AppGlobals.LookupContext.TestingOutlineTemplates);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingOutlineMaintenanceWindow>(AppGlobals.LookupContext.TestingOutlineCosts);
            LookupControlsGlobals.WindowRegistry.RegisterWindow<TestingTemplatesMaintenanceWindow>(AppGlobals.LookupContext.TestingTemplatesItems);

            LookupControlsGlobals.WindowRegistry.RegisterWindow<ProjectMaterialHistoryWindow>(AppGlobals.LookupContext.ProjectMaterialHistory);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <DevLogixChartMaintenanceUserControl, DevLogixChart>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <DevLogixChartMaintenanceUserControl, DevLogixChartBar>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <CustomerMaintenanceUserControl, Customer>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl<CustomerMaintenanceUserControl, CustomerProduct>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl<CustomerMaintenanceUserControl, CustomerUser>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <OrderMaintenanceUserControl, Order>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl<OrderMaintenanceUserControl, OrderDetail>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TimeZoneMaintenanceUserControl, TimeZone>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TerritoryMaintenanceUserControl, Territory>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <CustomerStatusMaintenanceUserControl, CustomerStatus>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <CustomerComputerMaintenanceUserControl, CustomerComputer>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <SupportTicketMaintenanceUserControl, SupportTicket>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <SupportTicketMaintenanceUserControl, SupportTicketUser>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <SupportTicketMaintenanceUserControl, SupportTicketError>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <SupportTicketStatusMaintenanceUserControl, SupportTicketStatus>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserMaintenanceUserControl, User>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserMaintenanceUserControl, UserTimeOff>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserMaintenanceUserControl, UserMonthlySales>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserMaintenanceUserControl, UsersGroup>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <GroupsMaintenanceUserControl, Group>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <DepartmentMaintenanceUserControl, Department>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserTrackerMaintenanceUserControl, UserTracker>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <UserTrackerMaintenanceUserControl, UserTrackerUser>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorStatusMaintenanceUserControl, ErrorStatus>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorPriorityMaintenanceUserControl, ErrorPriority>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProductMaintenanceUserControl, Product>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorMaintenanceUserControl, Error>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorMaintenanceUserControl>(AppGlobals.LookupContext.ErrorTesters);
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorMaintenanceUserControl>(AppGlobals.LookupContext.ErrorDevelopers);
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ErrorMaintenanceUserControl>(AppGlobals.LookupContext.ErrorUsers);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingTemplatesMaintenanceUserControl, TestingTemplate>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingTemplatesMaintenanceUserControl>(AppGlobals.LookupContext.TestingTemplatesItems);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingOutlineMaintenanceUserControl, TestingOutline>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingOutlineMaintenanceUserControl>(AppGlobals.LookupContext.TestingOutlineCosts);
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingOutlineMaintenanceUserControl>(AppGlobals.LookupContext.TestingOutlineDetails);
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <TestingOutlineMaintenanceUserControl>(AppGlobals.LookupContext.TestingOutlineTemplates);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectMaintenanceUserControl, Project>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectMaintenanceUserControl>(AppGlobals.LookupContext.ProjectUsers);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectTaskMaintenanceUserControl, ProjectTask>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectTaskMaintenanceUserControl>(AppGlobals.LookupContext.ProjectTaskLaborParts);
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectTaskMaintenanceUserControl>(AppGlobals.LookupContext.ProjectTaskDependency);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectMaterialMaintenanceUserControl, ProjectMaterial>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <ProjectMaterialMaintenanceUserControl, ProjectMaterialPart>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <LaborPartMaintenanceUserControl, LaborPart>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <MaterialPartMaintenanceUserControl, MaterialPart>();

            AppGlobals.LookupContext.FormatSearchForEvent += (sender, args) =>
            {
                if (args.SearchForHostId == DevLogixLookupContext.TimeSpentHostId)
                {
                    args.Value = AppGlobals.MakeTimeSpent(args.RawValue.ToDecimal());
                }

                if (args.SearchForHostId == DevLogixLookupContext.SpeedHostId)
                {
                    args.Value = AppGlobals.MakeSpeed(args.RawValue.ToDecimal());
                }

                if (args.SearchForHostId == DevLogixLookupContext.MemoryHostId)
                {
                    args.Value = AppGlobals.MakeSpace(args.RawValue.ToDecimal());
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

            //var devLogixDbMaintenanceFactory = new DevLogixDbMaintenanceFactory();
            var devLogixLookupSearchFactory = new DevLogixLookupSearchFactory();
            var devLogixGridCellFactory = new DevLogixGridCellFactory();

            return base.DoProcess();
        }

        public void ShowAddOnTheFlyWindow(RingSoft.App.Controls.DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
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

            //maintenanceWindow.Loaded += (sender, args) =>
            //{
            //    var processor = maintenanceWindow.ViewModel.Processor as AppDbMaintenanceWindowProcessor;
            //    processor.CheckAddOnFlyAfterLoaded();
            //};
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
