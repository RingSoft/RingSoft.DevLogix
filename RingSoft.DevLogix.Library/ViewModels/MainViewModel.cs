using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library.ViewModels
{
    public interface IMainView
    {
        bool ChangeOrganization();

        void CloseWindow();

        void ShowAdvancedFind();
    }
    public class MainViewModel
    {
        public IMainView MainView { get; set; }

        public RelayCommand ExitCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }

        public MainViewModel()
        {
            ExitCommand = new RelayCommand(Exit);

            AdvancedFindCommand = new RelayCommand(ShowAdvancedFind);

        }
        public void Initialize(IMainView view)
        {
            MainView = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInOrganization == null)
                loadVm = view.ChangeOrganization();

        }

        private void Exit()
        {
            MainView.CloseWindow();
        }

        private void ShowAdvancedFind()
        {
            MainView.ShowAdvancedFind();
        }
    }
}
