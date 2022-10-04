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
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this);
            };
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
