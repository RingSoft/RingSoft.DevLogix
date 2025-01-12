namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for TestingOutlineRetestFilterWindow.xaml
    /// </summary>
    public partial class TestingOutlineRetestFilterWindow
    {
        public TestingOutlineRetestFilterWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                ViewModel.Initialize();
            };
        }
    }
}
