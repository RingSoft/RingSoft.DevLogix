using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMainView
    {
        bool ChangeOrganization();

        bool LoginUser();

        void CloseWindow();

        void ShowAdvancedFind();

        void ShowUserMaintenance();

        void ShowGroupMaintenance();
    }
    public class MainViewModel
    {
        public IMainView MainView { get; set; }

        public RelayCommand UserMaintenanceCommand { get; }
        public RelayCommand GroupsMaintenanceCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }

        public MainViewModel()
        {
            ExitCommand = new RelayCommand(Exit);

            UserMaintenanceCommand = new RelayCommand(ShowUserManagement);
            GroupsMaintenanceCommand = new RelayCommand(ShowGroupsManagement);
            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);

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
            }
        }

        private void Exit()
        {
            MainView.CloseWindow();
        }

        private void ShowUserManagement()
        {
            MainView.ShowUserMaintenance();
        }

        private void ShowGroupsManagement()
        {
            MainView.ShowGroupMaintenance();
        }
        private void ShowAdvancedFind()
        {
            MainView.ShowAdvancedFind();
        }
    }
}
