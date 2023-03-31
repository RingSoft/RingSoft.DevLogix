using System;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectScheduleWindow.xaml
    /// </summary>
    public partial class ProjectScheduleWindow : IProjectScheduleView
    {
        public ProjectScheduleWindow(Project project, DateTime? startDate)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(this, project, startDate);
                StartDateControl.TextBox.SelectAll();
            };
        }

        public void PrintOutput(PrinterSetupArgs printerSetup)
        {
            LookupControlsGlobals.PrintDocument(printerSetup);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
