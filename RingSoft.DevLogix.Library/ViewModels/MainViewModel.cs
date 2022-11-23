using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;

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
    }
    public class MainViewModel
    {
        public IMainView MainView { get; set; }

        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand<TableDefinitionBase> ShowMaintenanceWindowCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        
        public MainViewModel()
        {
            ExitCommand = new RelayCommand(Exit);
            LogoutCommand = new RelayCommand(Logout);

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);
            ShowMaintenanceWindowCommand = new RelayCommand<TableDefinitionBase>(ShowMaintenanceWindow);
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
                if (AppGlobals.DataRepository.UsersExist())
                {
                    loadVm = view.LoginUser();
                    if (!loadVm)
                    {
                        var message =
                            "If you don't login a user, the application will shut down. Are you sure this is what you want to do?";
                        var caption = "User Login Failed";
                        if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                            MessageBoxButtonsResult.Yes)
                        {
                            AppGlobals.LoggedInOrganization = null;
                            loadVm = view.ChangeOrganization();
                        }
                        else
                        {
                            while (!loadVm)
                            {
                                loadVm = view.LoginUser();
                            }
                        }
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
    }
}
