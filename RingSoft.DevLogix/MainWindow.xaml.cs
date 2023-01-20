using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Google.Protobuf.WellKnownTypes;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.UserManagement;
using ScottPlot;
using ScottPlot.Plottable;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.Forms.MessageBox;
using Window = System.Windows.Window;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
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
            };
            WpfPlot.Visibility = Visibility.Collapsed;
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
                        Header = $"{categoryItem.Description}...",
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
                        Header = $"{categoryItem.Description}...",
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
                        Header = $"{categoryItem.Description}...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.Departments,
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
                        Header = $"{categoryItem.Description}...",
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
                        Header = $"{categoryItem.Description}...",
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
                        Header = $"{categoryItem.Description}...",
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
                        Header = $"{categoryItem.Description}...",
                        Command = ViewModel.ShowMaintenanceWindowCommand,
                        CommandParameter = AppGlobals.LookupContext.Errors,
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
            MainMenu.Items.Add(new MenuItem()
            {
                Header = $"E_xit",
                Command = ViewModel.ExitCommand
            });


            MainMenu.Items.Add(new MenuItem()
            {
                Header = $"_Logout",
                Command = ViewModel.LogoutCommand
            });

            MakeUserMenu();
            MakeQaMenu();
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
            var timeClockWindow = new TimeClockMaintenanceWindow();
            timeClockWindow.GetTimeClockError += (sender, args) =>
            {
                args.Error = error;
            };
            ShowTimeClockWindow(timeClockWindow);
        }

        private void ShowTimeClockWindow(TimeClockMaintenanceWindow timeClockWindow)
        {
            var activeWindow = LookupControlsGlobals.ActiveWindow;
            var closed = false;
            if (activeWindow != null)
            {
                activeWindow.Closed += (sender, args) => closed = true;
            }
            timeClockWindow.Closed += (sender, args) =>
            {
                if (!closed)
                {
                    activeWindow.Activate();
                }
            };
            var context = AppGlobals.DataRepository.GetDataContext();
            var tableQuery = context.GetTable<TimeClock>();
            var activeTimeCard =
                tableQuery.FirstOrDefault(p => p.PunchOutDate == null && p.UserId == AppGlobals.LoggedInUser.Id);
            if (activeTimeCard != null)
            {
                var message = "You currently are punched into an active time card. Do you wish to load that time card instead?";
                var caption = "Already Punched in";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    AppGlobals.LookupContext.TimeClocks.LookupDefinition.ShowAddOnTheFlyWindow(AppGlobals.LookupContext.TimeClocks.GetPrimaryKeyValueFromEntity(activeTimeCard));
                }
                
                return;
            }
            timeClockWindow.Owner = this;
            timeClockWindow.ShowInTaskbar = false;
            timeClockWindow.Show();
        }
    }
}
