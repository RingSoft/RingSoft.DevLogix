using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.UserManagement;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Customer = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.Customer;
using Error = RingSoft.DevLogix.DataAccess.Model.Error;
using SupportTicket = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.SupportTicket;
using TimeClock = RingSoft.DevLogix.DataAccess.Model.TimeClock;
using User = RingSoft.DevLogix.DataAccess.Model.User;
using Window = System.Windows.Window;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView, ICheckVersionView
    {
        public event EventHandler TimeClockClosed;
        private bool _isActive = true;

        public MainWindow()
        {
            InitializeComponent();

            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
            TabControl.SetDestionationAsFirstTab = false;

            SetupToolbar();

            ContentRendered += (sender, args) =>
            {
#if DEBUG
                ViewModel.Initialize(this);
#else
                try
                {
                    ViewModel.Initialize(this);
                }
                catch (Exception e)
                {
                    RingSoft.App.Library.RingSoftAppGlobals.HandleError(e);
                }
#endif
                //ViewModel.SetChartId(AppGlobals.LoggedInOrganization.DefaultChartId);
            };
            //MainChart.Loaded += (s, e) =>
            //{
            //    ViewModel.InitChart(MainChart.ViewModel);
            //    ChartGrid.Visibility = Visibility.Collapsed;
            //};

            Closing += (sender, args) =>
            {
                _isActive = false;
            };

            KeyDown += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (args.Key == Key.C)
                    {
                        ViewModel.PunchOut(true, AppGlobals.LoggedInUser);
                        args.Handled = true;
                    }
                }
            };
            //var bars = RedrawBars();
            //var barPlot = WpfPlot.Plot.AddBarSeries(bars);
            //WpfPlot.Plot.AxisAutoY();
            //WpfPlot.Refresh();
            //var timer = new Timer(1000);
            //Closing += (sender, args) =>
            //{
            //    timer.Stop();
            //    timer.Enabled = false;
            //    timer.Dispose();
            //};
            //timer.Start();
            //timer.Elapsed += (sender, args) =>
            //{
            //    Dispatcher.Invoke(() =>
            //    {
            //        WpfPlot.Reset();
            //        bars = RedrawBars();
            //        barPlot = WpfPlot.Plot.AddBarSeries(bars);
            //        WpfPlot.Refresh();
            //    });
            //};

            //WpfPlot.LeftClicked += (sender, args) =>
            //{
            //    (double mouseCoordX, double mouseCoordY) = WpfPlot.GetMouseCoordinates();
            //    var newBar = barPlot.GetBar(new Coordinate(mouseCoordX, mouseCoordY));

            //    if (newBar != null)
            //    {
            //        MessageBox.Show(newBar.Position.ToString(), "");
            //    }
            //};

        }

        private static List<Bar> RedrawBars()
        {
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = new Random().Next(),
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = i.ToString(),
                    LineWidth = 2,
                };
                bars.Add(bar);
            }

            return bars;
        }

        private void MakeCustomersMenu()
        {
            var customerCategory =
                SystemGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategoryId == (int)MenuCategories.Customers);

            var items = customerCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_Customer Management" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.Customer.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Customers...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Customer,
                    });
                }

                if (AppGlobals.LookupContext.Order.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Orders...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Order,
                    });
                }

                if (AppGlobals.LookupContext.TimeZone.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Time Zones...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.TimeZone,
                    });
                }

                if (AppGlobals.LookupContext.Territory.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit T_erritories...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Territory,
                    });
                }

                if (AppGlobals.LookupContext.CustomerStatus.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Customer St_atuses...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.CustomerStatus,
                    });
                }

                if (AppGlobals.LookupContext.CustomerComputer.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit C_ustomer Computers...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.CustomerComputer,
                    });
                }

                if (AppGlobals.LookupContext.SupportTicket.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Support Tickets...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.SupportTicket,
                    });
                }

                if (AppGlobals.LookupContext.SupportTicketStatus.HasRight(RightTypes.AllowView))
                {
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Su_pport Ticket Statuses...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.SupportTicketStatus,
                    });
                }

            }
        }

            private void MakeUserMenu()
        {
            var userCategory =
                SystemGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategoryId == (int)MenuCategories.UserManagement);

            var items = userCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_User Management" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Users);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Users...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Users,
                    });
                }

                if (AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Groups);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Groups...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Groups,
                    });
                }

                if (AppGlobals.LookupContext.Departments.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Departments);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Departments...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Departments,
                    });
                }
                if (AppGlobals.LookupContext.UserTracker.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.UserTracker);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit U_ser Tracker...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.UserTracker,
                    });
                }

            }
        }

        private void MakeQaMenu()
        {
            var qaCategory =
                SystemGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategoryId == (int)MenuCategories.Qa);

            var items = qaCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_Quality Assurance" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.ErrorStatuses.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.ErrorStatuses);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Error _Statuses...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.ErrorStatuses,
                    });
                }

                if (AppGlobals.LookupContext.ErrorPriorities.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.ErrorPriorities);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = $"Add/Edit E_rror Priorities...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.ErrorPriorities,
                    });
                }

                if (AppGlobals.LookupContext.Products.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.Products);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Products...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Products,
                    });
                }

                if (AppGlobals.LookupContext.Errors.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.Errors);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Product _Errors...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Errors,
                    });
                }

                if (AppGlobals.LookupContext.TestingTemplates.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.TestingTemplates);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Testing Templates...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.TestingTemplates,
                    });
                }

                if (AppGlobals.LookupContext.TestingOutlines.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        qaCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.TestingOutlines);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Testing _Outlines...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.TestingOutlines,
                    });
                }

            }
        }

        private void MakeProjectMenu()
        {
            var projectCategory =
                SystemGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategoryId == (int)MenuCategories.Projects);

            var items = projectCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_Project Management" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.Projects.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        projectCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.Projects);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Projects...",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.Projects,
                    });
                }
                if (AppGlobals.LookupContext.ProjectTasks.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        projectCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.ProjectTasks);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Project _Tasks...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.ProjectTasks,
                    });
                }
                if (AppGlobals.LookupContext.ProjectMaterials.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        projectCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.ProjectMaterials);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit Pro_ject Materials...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.ProjectMaterials,
                    });
                }

                if (AppGlobals.LookupContext.LaborParts.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        projectCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.LaborParts);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Labor Parts...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.LaborParts,
                    });
                }

                if (AppGlobals.LookupContext.MaterialParts.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        projectCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.MaterialParts);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Material Parts...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.MaterialParts,
                    });
                }

            }
        }

        public bool ChangeOrganization() 
        {
            var loginWindow = new LoginWindow { Owner = this };

            var result = false;
            var loginResult = loginWindow.ShowDialog();

            if (loginResult != null && loginResult.Value == true)
            {
                result = (bool)loginResult;
                ViewModel.SetOrganizationProps();
            }

            return result;
        }

        public bool LoginUser(int userId = 0)
        {
            var userLoginWindow = new UserLoginWindow(userId) { Owner = this };
            userLoginWindow.ShowDialog();
            if (userLoginWindow.ViewModel.DialogResult)
            {
                if (userId > 0)
                {
                    return true;
                }
                
                MakeMenu();
                if (AppGlobals.LoggedInUser != null)
                {
                    if (AppGlobals.LoggedInUser.DefaultChartId.HasValue)
                    {
                        ViewModel.ShowChartId(AppGlobals.LoggedInUser.DefaultChartId.Value);
                    }
                    else
                    {
                        ViewModel.ShowChartId(0);
                    }
                }
            }
            return userLoginWindow.ViewModel.DialogResult;
        }

        public void ShowDbMaintenanceDialog(TableDefinitionBase tableDefinition)
        {
            LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(tableDefinition);
        }

        public void ShowMaintenanceTab(TableDefinitionBase tableDefinition)
        {
            TabControl.ShowTableControl(tableDefinition, false);
        }

        public void ShowAdvancedFindWindow()
        {
            ShowWindow(new AdvancedFindWindow());
        }

        private void ProcessButton(DbMaintenanceButton button, TableDefinitionBase tableDefinition)
        {
            if (tableDefinition.HasRight(RightTypes.AllowView))
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Collapsed;
            }
        }

        public void MakeMenu()
        {
            MainMenu.Items.Clear();

            SetupToolbar();

            var fileMenu = new MenuItem()
            {
                Header = "_File"
            };
            fileMenu.Items.Add(new MenuItem()
            {
                Header = $"_Change Organization",
                Command = ViewModel.ChangeOrgCommand
            });

            fileMenu.Items.Add(new MenuItem()
            {
                Header = $"_Logout",
                Command = ViewModel.LogoutCommand
            });

            if (AppGlobals.LoggedInUser.Id == 1)
            {
                fileMenu.Items.Add(new MenuItem()
                {
                    Header = $"_Repair Dates",
                    Command = ViewModel.RepairDatesCommand
                });
            }

            fileMenu.Items.Add(new MenuItem()
            {
                Header = $"E_xit",
                Command = ViewModel.ExitCommand
            });

            MainMenu.Items.Add(fileMenu);

            MakeUserMenu();
            MakeCustomersMenu();
            MakeQaMenu();
            MakeProjectMenu();

            var toolsCategory =
                SystemGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategoryId == (int)MenuCategories.Tools);

            var items = toolsCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_Tools" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.DevLogixCharts.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        toolsCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.DevLogixCharts);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "_Charts",
                        Command = ViewModel.ShowMaintenanceTabCommand,
                        CommandParameter = AppGlobals.LookupContext.DevLogixCharts,
                    });
                }

                if (AppGlobals.LookupContext.SystemPreferences.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        toolsCategory.Items.FirstOrDefault(
                            p => p.TableDefinition == AppGlobals.LookupContext.SystemPreferences);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "_System Preferences",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.SystemPreferences,
                    });
                }

            }

            MainMenu.Items.Add(new MenuItem()
            {
                Header = "A_bout DevLogix...",
                Command = ViewModel.AboutCommand,
            });

            MainMenu.Items.Add(new MenuItem()
            {
                Header = "Upgrade _Version...",
                Command = ViewModel.UpgradeCommand,
            });
        }

        private void SetupToolbar()
        {
            ChangeOrgButton.ToolTip.HeaderText = "Change Organization (Alt + Z)";
            ChangeOrgButton.ToolTip.DescriptionText =
                "Change to a different organization.";

            LogoutButton.ToolTip.HeaderText = "Logout Current User (Alt + L)";
            LogoutButton.ToolTip.DescriptionText =
                "Log out of the current user and log into a different user.";

            if (AppGlobals.LookupContext == null)
            {
                UsersButton.Visibility = Visibility.Collapsed;
                UserTrackerButton.Visibility = Visibility.Collapsed;
                ProductsButton.Visibility = Visibility.Collapsed;
                ErrorsButton.Visibility = Visibility.Collapsed;
                AdvancedFindButton.Visibility = Visibility.Collapsed;
                ChartButton.Visibility = Visibility.Collapsed;
                OutlinesButton.Visibility = Visibility.Collapsed;
                ProjectsButton.Visibility = Visibility.Collapsed;
                CustomersButton.Visibility = Visibility.Collapsed;
                OrdersButton.Visibility = Visibility.Collapsed;
                SupportTicketsButton.Visibility = Visibility.Collapsed;
                return;
            }

            ProcessButton(ProductsButton, AppGlobals.LookupContext.Products);
            ProductsButton.ToolTip.HeaderText = "Show the Product Maintenance Window (Alt + D)";
            ProductsButton.ToolTip.DescriptionText =
                "Add or edit Products.";

            ProcessButton(ErrorsButton, AppGlobals.LookupContext.Errors);
            ErrorsButton.ToolTip.HeaderText = "Show the Product Error Maintenance Window (Alt + E)";
            ErrorsButton.ToolTip.DescriptionText =
                "Add or edit Product Errors.";

            ProcessButton(OutlinesButton, AppGlobals.LookupContext.TestingOutlines);
            OutlinesButton.ToolTip.HeaderText = "Show the Testing Outlines Maintenance Window (Alt + T)";
            OutlinesButton.ToolTip.DescriptionText =
                "Add or edit Testing Outlines.";

            ProcessButton(ProjectsButton, AppGlobals.LookupContext.Projects);
            ProjectsButton.ToolTip.HeaderText = "Show the Projects Maintenance Window (Alt + J)";
            ProjectsButton.ToolTip.DescriptionText =
                "Add or edit Projects.";

            ProcessButton(UsersButton, AppGlobals.LookupContext.Users);
            UsersButton.ToolTip.HeaderText = "Show the Users Maintenance Window (Alt + U)";
            UsersButton.ToolTip.DescriptionText =
                "Add or edit Users.";

            ProcessButton(UserTrackerButton, AppGlobals.LookupContext.UserTracker);
            UserTrackerButton.ToolTip.HeaderText = "Show the User Tracker Maintenance Window (Alt + R)";
            UserTrackerButton.ToolTip.DescriptionText =
                "Add or edit User Trackers.";

            ProcessButton(CustomersButton, AppGlobals.LookupContext.Customer);
            CustomersButton.ToolTip.HeaderText = "Show the Customer Maintenance Window (Alt + C)";
            CustomersButton.ToolTip.DescriptionText =
                "Add or edit Customers.";

            ProcessButton(OrdersButton, AppGlobals.LookupContext.Order);
            OrdersButton.ToolTip.HeaderText = "Show the Orders Window (Alt + O)";
            OrdersButton.ToolTip.DescriptionText =
                "Add or edit Orders.";

            ProcessButton(SupportTicketsButton, AppGlobals.LookupContext.SupportTicket);
            SupportTicketsButton.ToolTip.HeaderText = "Show the Support Tickets Window (Alt + U)";
            SupportTicketsButton.ToolTip.DescriptionText =
                "Add or edit Support Tickets.";

            ProcessButton(AdvancedFindButton, AppGlobals.LookupContext.AdvancedFinds);
            AdvancedFindButton.ToolTip.HeaderText = "Advanced Find (Alt + A)";
            AdvancedFindButton.ToolTip.DescriptionText =
                "Search any table in the database for information you're looking for.";


            ProcessButton(ChartButton, AppGlobals.LookupContext.DevLogixCharts);
            ChartButton.ToolTip.HeaderText = "Dashboard Charts (Alt + H)";
            ChartButton.ToolTip.DescriptionText =
                "Manage Charts that you can use as the program dashboard. Click on a chart bar to see its associated Advanced Find Lookup.";
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ShowWindow(Window window)
        {
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.Closed += (sender, args) => Activate();
            window.Show();

        }

        public void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(tableDefinition);
        }

        public void PunchIn(Error error)
        {
            var timeClockWindow = new TimeClockMaintenanceWindow(error);
            ShowTimeClockWindow(timeClockWindow);
        }

        public void PunchIn(ProjectTask projectTask)
        {
            var timeClockWindow = new TimeClockMaintenanceWindow(projectTask);
            ShowTimeClockWindow(timeClockWindow);
        }

        public void PunchIn(TestingOutline testingOutline)
        {
            var timeClockWindow = new TimeClockMaintenanceWindow(testingOutline);
            ShowTimeClockWindow(timeClockWindow);
        }

        public void PunchIn(Customer customer)
        {
            var timeClockWindow = new TimeClockMaintenanceWindow(customer);
            ShowTimeClockWindow(timeClockWindow);
        }

        public void PunchIn(SupportTicket ticket)
        {
            var timeClockWindow = new TimeClockMaintenanceWindow(ticket);
            ShowTimeClockWindow(timeClockWindow);
        }

        public void ShowChart(DevLogixChart chart)
        {
            if (TabControl.ShowTableControl(AppGlobals.LookupContext.DevLogixCharts) is
                DevLogixChartMaintenanceUserControl
                devLogixChart)
            {
                devLogixChart.Loaded += (sender, args) =>
                {
                    if (chart != null)
                    {
                        var pkValue = AppGlobals.LookupContext.DevLogixCharts
                            .GetPrimaryKeyValueFromEntity(chart);
                        devLogixChart.LocalViewModel.OnRecordSelected(pkValue);
                    }
                };
            }
        }

        public object GetOwnerWindow()
        {
            return this;
        }

        public void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack)
        {
            var window = new HistoryPrintFilterWindow(callBack);
            window.Owner = WPFControlsGlobals.ActiveWindow;
            window.ShowInTaskbar = false;
            window.ShowDialog();

        }

        public void SetElapsedTime()
        {
            if (_isActive)
            {
                Dispatcher?.Invoke(() =>
                {
                    ElapsedTimeBox.Text = ViewModel.ElapsedTime;
                    SupportTimePurchasedControl.SetTimeRemaining(SupportTimeLeftLabel, AppGlobals.MainViewModel.SupportTimeLeft
                        , AppGlobals.MainViewModel.SupportMinutesLeft);


                    if (AppGlobals.MainViewModel.SupportMinutesLeft <= 10)
                    {
                        var timeLeft = AppGlobals.MakeTimeSpent(
                            ViewModel
                                .SupportMinutesLeft
                                .GetValueOrDefault());

                        var message = $"{ViewModel.ActiveCustomerName} has {timeLeft} of support time left.";
                        var alertLevel = AlertLevels.Yellow;
                        if (ViewModel.SecondsElapsed % 60 == 0)
                        {
                            if (ViewModel.SupportMinutesLeft <= 0)
                            {
                                alertLevel = AlertLevels.Red;
                            }
                            LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(alertLevel, false
                                , this, message);
                        }
                    }
                });
            }
        }


        public void ShowTimeClock(bool show = true)
        {
            if (show)
            {
                TimeClockPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TimeClockPanel.Visibility = Visibility.Collapsed;
            }
        }

        public void LaunchTimeClock(TimeClock activeTimeCard)
        {
            var activeWindow = LookupControlsGlobals.ActiveWindow;
            var lookupDefinition = AppGlobals.LookupContext.TimeClockLookup.Clone();
            lookupDefinition.WindowClosed += (sender, args) =>
            {
                //Peter Ringering - 05/25/2023 11:26:03 AM - E-37
                activeWindow.Activate();
            };

            lookupDefinition.ShowAddOnTheFlyWindow(
                AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(activeTimeCard));
        }

        public UserClockReasonViewModel GetUserClockReason()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = table.FirstOrDefault(p => p.Id == AppGlobals.LoggedInUser.Id);
            return GetUserClockReason(user);
        }

        public UserClockReasonViewModel GetUserClockReason(User user)
        {
            var clockReasonDialog = new UserClockReasonWindow(user);
            clockReasonDialog.ShowDialog();
            return clockReasonDialog.LocalViewModel;
        }

        private void ShowTimeClockWindow(TimeClockMaintenanceWindow timeClockWindow)
        {
            var activeWindow = LookupControlsGlobals.ActiveWindow;
            timeClockWindow.Closed += (sender, args) =>
            {
                activeWindow.Activate();
                //TimeClockClosed?.Invoke(this, EventArgs.Empty);
            };
            timeClockWindow.Owner = activeWindow;
            timeClockWindow.ShowInTaskbar = false;
            timeClockWindow.Show();
        }

        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    MainChart.ProcessKeyDown(e);
        //    base.OnPreviewKeyDown(e);
        //}

        protected async override void OnClosing(CancelEventArgs e)
        {
            if (!await ViewModel.ValidateWindowClose())
            {
                e.Cancel = true;
                return;
            }

            if (!TabControl.CloseAllTabs())
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }

        public bool UpgradeVersion()
        {
            return AppStart.CheckVersion(this, true);
        }

        public void ShutDownApp()
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public void ShowAbout()
        {
            var splashWindow = new AppSplashWindow(false);
            splashWindow.Title = "About DevLogix";
            splashWindow.Owner = this;
            splashWindow.ShowInTaskbar = false;
            splashWindow.ShowDialog();
        }

        public void RepairDates()
        {
            var procedure = new TwoTierProcedure();
            procedure.DoProcedure += (sender, result) =>
            {
                AppGlobals.FixAllDateTimes(procedure);
            };
            procedure.Start();
        }

        public bool CloseAllTabs()
        {
            return TabControl.CloseAllTabs();
        }
    }
}
