using System.Windows;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DevLogix.Library;

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
                ViewModel.Initialize(this);
            };
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
    }
}
