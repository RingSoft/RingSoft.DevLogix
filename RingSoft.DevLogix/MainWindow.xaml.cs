using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.UserManagement;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using RingSoft.DevLogix.DataAccess;
using Error = RingSoft.DevLogix.DataAccess.Model.Error;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.Forms.MessageBox;
using Window = System.Windows.Window;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
    {
        public RelayCommand AboutCommand { get; private set; }

        public event EventHandler TimeClockClosed;

        public MainWindow()
        {
            InitializeComponent();

            AboutCommand = new RelayCommand(() =>
            {
                var splashWindow = new AppSplashWindow(false);
                splashWindow.Title = "About DevLogix";
                splashWindow.Owner = this;
                splashWindow.ShowInTaskbar = false;
                splashWindow.ShowDialog();
            });
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
            MainChart.Loaded += (s, e) =>
            {
                ViewModel.InitChart(MainChart.ViewModel);
                ChartGrid.Visibility = Visibility.Collapsed;
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

        private void MakeUserMenu()
        {
            var userCategory =
                AppGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategory == MenuCategories.UserManagement);

            var items = userCategory.Items.Where(p => p.TableDefinition.HasRight(RightTypes.AllowView));
            if (items.Any())
            {
                var menuItem = new MenuItem() { Header = "_User  Management" };
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Users);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = "Add/Edit _Users...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.UserTracker,
                    });
                }

            }
        }

        private void MakeQaMenu()
        {
            var qaCategory =
                AppGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategory == MenuCategories.Qa);

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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.TestingTemplates,
                    });
                }
            }
        }

        private void MakeProjectMenu()
        {
            var projectCategory =
                AppGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategory == MenuCategories.Projects);

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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                result = (bool)loginResult;

            return result;
        }

        public bool LoginUser()
        {
            var userLoginWindow = new UserLoginWindow { Owner = this };
            userLoginWindow.ShowDialog();
            if (userLoginWindow.ViewModel.DialogResult)
            {
                MakeMenu();
                if (AppGlobals.LoggedInUser != null)
                {
                    if (AppGlobals.LoggedInUser.DefaultChartId.HasValue)
                    {
                        ViewModel.SetChartId(AppGlobals.LoggedInUser.DefaultChartId.Value);
                    }
                    else
                    {
                        ViewModel.SetChartId(0);
                    }
                }
            }
            return userLoginWindow.ViewModel.DialogResult;
        }

        public void ShowAdvancedFindWindow()
        {
            ShowWindow(new AdvancedFindWindow());
        }

        public void MakeMenu()
        {
            MainMenu.Items.Clear();

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

            fileMenu.Items.Add(new MenuItem()
            {
                Header = $"E_xit",
                Command = ViewModel.ExitCommand
            });

            MainMenu.Items.Add(fileMenu);

            MakeUserMenu();
            MakeQaMenu();
            MakeProjectMenu();

            var toolsCategory =
                AppGlobals.Rights.UserRights.Categories.FirstOrDefault(p =>
                    p.MenuCategory == MenuCategories.Tools);

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
                        Command = ViewModel.ShowMaintenanceWindowCommand,
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
                Command = AboutCommand,
            });
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
            var dbMaintenanceWindow = WindowRegistry.GetMaintenanceWindow(tableDefinition);
            if (dbMaintenanceWindow != null)
            {
                ShowWindow(dbMaintenanceWindow);
            }
        }

        public void PunchIn(Error error)
        {
            var timeClockWindow = GetTimeClockWindow();
            if (timeClockWindow != null)
            {
                timeClockWindow.GetTimeClockError += (sender, args) =>
                {
                    args.Error = error;
                };
                ShowTimeClockWindow(timeClockWindow);
            }
        }

        public void PunchIn(ProjectTask projectTask)
        {
            var timeClockWindow = GetTimeClockWindow();
            if (timeClockWindow != null)
            {
                timeClockWindow.GetTimeClockProjectTask += (sender, args) =>
                {
                    args.ProjectTask = projectTask;
                };
                ShowTimeClockWindow(timeClockWindow);
            }

        }

        public bool PunchOut(bool clockOut, int userId)
        {
            if (userId == 0)
            {
                userId = AppGlobals.LoggedInUser.Id;
            }

            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = table.FirstOrDefault(p => p.Id == userId);

            return PunchOut(clockOut, user, context);
        }

        public bool PunchOut(bool clockOut, User user, IDbContext context = null)
        {
            if (context == null)
            {
                context = AppGlobals.DataRepository.GetDataContext();
            }

            var tableQuery = context.GetTable<TimeClock>();

            var activeTimeCard =
                tableQuery.FirstOrDefault(p => p.PunchOutDate == null && p.UserId == user.Id && p.PunchOutDate == null);

            var result = true;

            var lookupDefinition = AppGlobals.LookupContext.TimeClockLookup.Clone();

            var dialogInput = new DialogInput();

            if (activeTimeCard != null)
            {
                lookupDefinition.ShowAddOnTheFlyWindow(
                    AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(activeTimeCard), dialogInput);
            }
            else
            {
                dialogInput.DialogResult = true;
            }

            if (!clockOut)
            {
                return true;
            }

            if (dialogInput.DialogResult)
            {
                var clockReasonDialog = new UserClockReasonWindow(user);
                clockReasonDialog.ShowDialog();
                if (!clockReasonDialog.LocalViewModel.DialogResult)
                {
                    return false;
                }

                if (user != null)
                {
                    user.ClockDate = DateTime.Now.ToUniversalTime();
                    user.ClockOutReason = (byte)clockReasonDialog.LocalViewModel.ClockOutReason;
                    if (clockReasonDialog.LocalViewModel.ClockOutReason == ClockOutReasons.Other)
                    {
                        user.OtherClockOutReason = clockReasonDialog.LocalViewModel.OtherReason;
                    }
                    else
                    {
                        user.OtherClockOutReason = null;
                    }

                    var table = AppGlobals.LookupContext.Users;
                    var clockDateField = table.GetFieldDefinition(p => p.ClockDate);
                    var clockReasonField = table.GetFieldDefinition(p => p.ClockOutReason);
                    var otherReasonField = table.GetFieldDefinition(p => p.OtherClockOutReason);

                    var sqlData = new SqlData(clockDateField.FieldName
                        , clockReasonField.FormatValue(user.ClockDate.ToString())
                        , ValueTypes.DateTime
                        , DbDateTypes.DateTime);
                    var updateStatement = new UpdateDataStatement(table.GetPrimaryKeyValueFromEntity(user));
                    updateStatement.AddSqlData(sqlData);

                    sqlData = new SqlData(clockReasonField.FieldName
                        , user.ClockOutReason.ToString()
                        , ValueTypes.Numeric);
                    updateStatement.AddSqlData(sqlData);

                    if (user.OtherClockOutReason.IsNullOrEmpty())
                    {
                        sqlData = new SqlData(otherReasonField.FieldName
                            , ""
                            , ValueTypes.String);
                    }
                    else
                    {
                        sqlData = new SqlData(clockReasonField.FieldName
                            , user.OtherClockOutReason
                            , ValueTypes.String);
                    }
                    updateStatement.AddSqlData(sqlData);

                    var sql = AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateUpdateSql(updateStatement);
                    var dataResult = AppGlobals.LookupContext.DataProcessor.ExecuteSql(sql);
                    return dataResult.ResultCode == GetDataResultCodes.Success;

                    //return context.SaveEntity(user, "Clocking Out");
                }

            }
            return false;
        }

        public void ShowMainChart(bool show = true)
        {
            ChartGrid.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
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

        private TimeClockMaintenanceWindow GetTimeClockWindow()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var tableQuery = context.GetTable<TimeClock>();
            var activeTimeCard =
                tableQuery.FirstOrDefault(p => p.PunchOutDate == null && p.UserId == AppGlobals.LoggedInUser.Id);
            if (activeTimeCard != null)
            {
                var message = "You currently are punched into an active time card. Do you wish to load that time card instead?";
                var caption = "Already Punched in";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    System.Windows.Forms.DialogResult.Yes)
                {
                    LaunchTimeClock(activeTimeCard);
                }

                return null;
            }

            return new TimeClockMaintenanceWindow();
        }

        private static void LaunchTimeClock(TimeClock activeTimeCard)
        {
            var activeWindow = LookupControlsGlobals.ActiveWindow;
            var lookupDefinition = AppGlobals.LookupContext.TimeClockLookup.Clone();
            lookupDefinition.WindowClosed += (sender, args) =>
            {
                activeWindow.Activate();
            };

            lookupDefinition.ShowAddOnTheFlyWindow(
                AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(activeTimeCard));
        }

        private void ShowTimeClockWindow(TimeClockMaintenanceWindow timeClockWindow)
        {
            var activeWindow = LookupControlsGlobals.ActiveWindow;
            timeClockWindow.Closed += (sender, args) =>
            {
                TimeClockClosed?.Invoke(this, EventArgs.Empty);
            };
            timeClockWindow.Owner = activeWindow;
            timeClockWindow.ShowInTaskbar = false;
            timeClockWindow.Show();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            MainChart.ProcessKeyDown(e);
            base.OnPreviewKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (AppGlobals.LoggedInUser == null)
            {
                base.OnClosing(e);
                return;
            }
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var userQuery = context.GetTable<User>();
                if (userQuery != null)
                {
                    var user = userQuery.FirstOrDefault(p => p.Id == AppGlobals.LoggedInUser.Id);
                    if (user != null && (ClockOutReasons)user.ClockOutReason == ClockOutReasons.ClockedIn)
                    {
                        var message = "Do you want to clock out before you exit the application?";
                        var caption = "Clock Out?";
                        var clockOut = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption);
                        if (clockOut == MessageBoxButtonsResult.Yes)
                        {
                            if (!PunchOut(true, user.Id))
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
            base.OnClosing(e);
        }
    }
}
