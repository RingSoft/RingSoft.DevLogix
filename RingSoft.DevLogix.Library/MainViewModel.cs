using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library
{
    public interface IMainView
    {
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
