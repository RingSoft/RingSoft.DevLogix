using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectHeaderControl : DbMaintenanceCustomPanel
    {
        public Button PunchInButton { get; set; }

        public Button RecalcButton { get; set; }

        static ProjectHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as Button;
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProjectMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaintenanceWindow : IProjectView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProjectMaintenanceWindow()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProjectHeaderControl projectHeaderControl)
                {
                    projectHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        projectHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }
                    projectHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                }
            };

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);

            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();

            base.ResetViewForNewRecord();
        }

        public void PunchIn(Project project)
        {
            AppGlobals.MainViewModel.MainView.PunchIn(project);
        }
    }
}
