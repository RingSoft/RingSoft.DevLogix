using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for TestingOutlineRetestFilterWindow.xaml
    /// </summary>
    public partial class TestingOutlineRetestFilterWindow : IRetestView
    {
        public TestingOutlineRetestFilterWindow(RetestInput input)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, input);
            };
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
