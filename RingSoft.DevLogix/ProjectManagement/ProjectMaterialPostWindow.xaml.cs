using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialPostWindow.xaml
    /// </summary>
    public partial class ProjectMaterialPostWindow
    {
        public ProjectMaterialPostWindow(Project project)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(project);
            };
        }
    }
}
