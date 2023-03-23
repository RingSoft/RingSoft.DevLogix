using System;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectScheduleWindow.xaml
    /// </summary>
    public partial class ProjectScheduleWindow
    {
        public ProjectScheduleWindow(Project project, DateTime? startDate)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(project, startDate);
                StartDateControl.TextBox.SelectAll();
            };
        }
    }
}
