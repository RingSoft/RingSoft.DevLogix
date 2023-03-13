using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMainView
    {
        bool ChangeOrganization();

        bool LoginUser();

        void CloseWindow();

        void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition);

        void ShowAdvancedFindWindow();

        void MakeMenu();

        void PunchIn(Error error);

        void PunchIn(ProjectTask projectTask);

        bool PunchOut(bool clockOut, int userId = 0);

        void ShowMainChart(bool show = true);

        object GetOwnerWindow();
    }
    public class MainViewModel
    {
        public IMainView MainView { get; set; }

        public List<UserMaintenanceViewModel> UserViewModels { get; } = new List<UserMaintenanceViewModel>();
        public List<ProjectMaintenanceViewModel> ProjectViewModels { get; } = new List<ProjectMaintenanceViewModel>();
        public List<ProjectTaskViewModel> ProjectTaskViewModels { get; } = new List<ProjectTaskViewModel>();
        public List<ErrorViewModel> ErrorViewModels { get; } = new List<ErrorViewModel>();

        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceWindowCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        public RelayCommand RefreshChartCommand { get; }
        public RelayCommand EditChartCommand { get; }
        public RelayCommand ChangeOrgCommand { get; }
        public ChartBarsViewModel ChartViewModel { get; private set; }
        
        public MainViewModel()
        {
            ExitCommand = new RelayCommand(Exit);
            LogoutCommand = new RelayCommand(Logout);

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);
            ShowMaintenanceWindowCommand = new RelayCommand<TableDefinitionBase>(ShowMaintenanceWindow);
            RefreshChartCommand = new RelayCommand(RefreshChart);
            EditChartCommand = new RelayCommand(EditChart);
            ChangeOrgCommand = new RelayCommand((() =>
            {
                SetChartId(0);
                AppGlobals.LoggedInOrganization = null;
                Initialize(MainView);
            }));
        }
        public void Initialize(IMainView view)
        {
            MainView = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInOrganization == null)
                loadVm = view.ChangeOrganization();

            if (loadVm)
            {
                var query = AppGlobals.DataRepository.GetDataContext().GetTable<User>();
                if (query.Any())
                {
                    loadVm = view.LoginUser();
                    if (!loadVm)
                    {
                        AppGlobals.LoggedInOrganization = null;
                        Initialize(view);
                        //var message =
                        //    "If you don't login a user, the application will shut down. Are you sure this is what you want to do?";
                        //var caption = "User Login Failed";
                        //if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                        //    MessageBoxButtonsResult.Yes)
                        //{
                        //    AppGlobals.LoggedInOrganization = null;
                        //    Initialize(view);
                        //    //loadVm = view.ChangeOrganization();
                        //    //if (loadVm)
                        //    //{

                        //    //}
                        //}
                        //else
                        //{
                        //    while (!loadVm)
                        //    {
                        //        loadVm = view.LoginUser();
                        //    }
                        //}
                    }
                }
                else
                {
                    MainView.MakeMenu();
                }
            }
        }

        private void Exit()
        {
            MainView.CloseWindow();
        }

        private void ShowMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
            MainView.ShowDbMaintenanceWindow(tableDefinition);
        }

        private void ShowAdvancedFind()
        {
            MainView.ShowAdvancedFindWindow();
        }

        private void Logout()
        {
            if (MainView.LoginUser())
            {
                MainView.MakeMenu();
            }
        }

        public void InitChart(ChartBarsViewModel chartBarsViewModel)
        {
            ChartViewModel = chartBarsViewModel;
        }

        public void SetChartId(int chartId)
        {
            MainView.ShowMainChart(chartId != 0);
            if (chartId == 0)
            {
                ChartViewModel.Clear(true);
            }
            else
            {
                ChartViewModel.SetChartBars(chartId);
            }
        }

        private void RefreshChart()
        {
            ChartViewModel.SetChartBars(ChartViewModel.ChartId);
            var message = "Chart Refreshed.";
            ControlsGlobals.UserInterface.ShowMessageBox(message, message, RsMessageBoxIcons.Information);
        }

        private void EditChart()
        {
            var devLogixChart = new DevLogixChart
            {
                Id = ChartViewModel.ChartId
            };

            var primaryKey = AppGlobals.LookupContext.DevLogixCharts.GetPrimaryKeyValueFromEntity(devLogixChart);
            var lookup = AppGlobals.LookupContext.DevLogixChartLookup.Clone();
            lookup.ShowAddOnTheFlyWindow(primaryKey, null, MainView.GetOwnerWindow());
        }
    }
}
