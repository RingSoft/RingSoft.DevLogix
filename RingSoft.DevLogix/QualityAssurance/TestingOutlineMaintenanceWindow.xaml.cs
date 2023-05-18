using RingSoft.App.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance.Testing;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingOutlineHeaderControl : DbMaintenanceCustomPanel
    {
        public Button GenerateDetailsButton { get; set; }

        public Button RetestButton { get; set; }

        public Button PunchInButton { get; set; }

        static TestingOutlineHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingOutlineHeaderControl), new FrameworkPropertyMetadata(typeof(TestingOutlineHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            GenerateDetailsButton = GetTemplateChild(nameof(GenerateDetailsButton)) as Button;
            RetestButton = GetTemplateChild(nameof(RetestButton)) as Button;
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as Button;

            base.OnApplyTemplate();
        }
    }
    /// <summary>
    /// Interaction logic for TestingOutlineMaintenanceWindow.xaml
    /// </summary>
    public partial class TestingOutlineMaintenanceWindow : ITestingOutlineView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Testing Outline";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public TestingOutlineMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is TestingOutlineHeaderControl templateHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        templateHeaderControl.GenerateDetailsButton.Visibility = Visibility.Collapsed;
                        templateHeaderControl.RetestButton.Visibility = Visibility.Collapsed;
                    }
                    templateHeaderControl.GenerateDetailsButton.Command = LocalViewModel.GenerateDetailsCommand;
                    templateHeaderControl.RetestButton.Command = LocalViewModel.RetestCommand;
                    templateHeaderControl.PunchInButton.Command = LocalViewModel.PunchInCommand;
                }
            };

        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void PunchIn(TestingOutline testingOutline)
        {
            AppGlobals.MainViewModel.MainView.PunchIn(testingOutline);
        }
    }
}
