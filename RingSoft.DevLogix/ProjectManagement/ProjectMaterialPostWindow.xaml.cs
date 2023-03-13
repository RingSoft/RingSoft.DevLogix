using System;
using System.Windows;
using System.Windows.Input;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialPostWindow.xaml
    /// </summary>
    public partial class ProjectMaterialPostWindow : IProjectMaterialPostView
    {
        private Size _oldSize;

        public ProjectMaterialPostWindow(Project project)
        {
            var loaded = false;
            InitializeComponent();
            ContentRendered += (sender, args) => DataEntryGrid.Focus();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(project, this);
                DataEntryGrid.PreviewKeyDown += (o, eventArgs) =>
                {
                    if (eventArgs.Key == Key.Enter)
                    {
                        if (DataEntryGrid.EditingControlHost != null)
                        {
                            if (DataEntryGrid.EditingControlHost.HasDataChanged())
                            {
                                DataEntryGrid.EditingControlHost.Row.IsNew = false;
                                DataEntryGrid.EditingControlHost.Row.SetCellValue(DataEntryGrid.EditingControlHost
                                    .GetCellValue());
                            }
                        }

                        if (DataEntryGrid.EditingControlHost != null &&
                            !DataEntryGrid.EditingControlHost.IsDropDownOpen)
                        {
                            ViewModel.OkCommand.Execute(null);
                            eventArgs.Handled = true;
                        }
                    }
                };

                loaded = true;
            };
            SizeChanged += (sender, args) =>
            {
                if (DataEntryGrid != null && loaded)
                {
                    var widthDif = Width - _oldSize.Width;
                    var heightDif = Height - _oldSize.Height;
                    if (Math.Round(widthDif) > 1)
                    {
                        DataEntryGrid.Width = DataEntryGrid.ActualWidth + widthDif;
                    }

                    if (Math.Round(heightDif) > 1)
                    {
                        DataEntryGrid.Height = DataEntryGrid.ActualHeight + heightDif;
                    }
                }

                _oldSize = args.NewSize;
            };

        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
