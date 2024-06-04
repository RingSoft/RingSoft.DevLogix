using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialPartSelectorWindow.xaml
    /// </summary>
    public partial class ProjectMaterialPartSelectorWindow : IMaterialPartView
    {
        public ProjectMaterialPartSelectorWindow(string keyText)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, keyText);
            };

        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
