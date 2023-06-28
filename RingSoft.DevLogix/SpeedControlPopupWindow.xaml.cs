using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for SpeedControlPopupWindow.xaml
    /// </summary>
    public partial class SpeedControlPopupWindow : ISpeedControlPopupView
    {
        public SpeedControlPopupWindow(double speed)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, speed);
                TimePartControl.TextBox.SelectAll();
            };
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
