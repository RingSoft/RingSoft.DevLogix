using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Google.Protobuf.WellKnownTypes;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.UserManagement;
using ScottPlot;
using ScottPlot.Plottable;
using MessageBox = System.Windows.Forms.MessageBox;

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
                var menuItem = new MenuItem() {Header = "_User Management"};
                MainMenu.Items.Add(menuItem);

                if (AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Users);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = $"{categoryItem.Description}...",
                        Command = ViewModel.UserMaintenanceCommand
                    });
                }

                if (AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowView))
                {
                    var categoryItem =
                        userCategory.Items.FirstOrDefault(p => p.TableDefinition == AppGlobals.LookupContext.Groups);
                    menuItem.Items.Add(new MenuItem()
                    {
                        Header = $"{categoryItem.Description}...",
                        Command = ViewModel.GroupsMaintenanceCommand
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

        public void MakeMenu()
        {
            MainMenu.Items.Clear();
            MakeUserMenu();
        }


        public void CloseWindow()
        {
            Close();
        }

        public void ShowAdvancedFind()
        {
            var window = new AdvancedFindWindow();
            window.Owner = this;
            window.Closed += (sender, args) => Activate();

            window.Show();
        }

        public void ShowUserMaintenance()
        {
            var window = new UserMaintenanceWindow
            {
                Owner = this,
                ShowInTaskbar = true
            };

            window.Closed += (sender, args) => Activate();
            window.Show();
        }

        public void ShowGroupMaintenance()
        {
            var window = new GroupsMaintenanceWindow()
            {
                Owner = this,
                ShowInTaskbar = true
            };

            window.Closed += (sender, args) => Activate();
            window.Show();
        }
    }
}
