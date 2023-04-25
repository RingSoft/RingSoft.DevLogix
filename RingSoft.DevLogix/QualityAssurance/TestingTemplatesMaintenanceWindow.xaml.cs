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
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.ProjectManagement;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingTemplateHeaderControl : DbMaintenanceCustomPanel
    {
        public Button UpdateOutlinesButton { get; set; }

        static TestingTemplateHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingTemplateHeaderControl), new FrameworkPropertyMetadata(typeof(TestingTemplateHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            UpdateOutlinesButton = GetTemplateChild(nameof(UpdateOutlinesButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for TestingTemplatesMaintenanceWindow.xaml
    /// </summary>
    public partial class TestingTemplatesMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Testing Template";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public TestingTemplatesMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is TestingTemplateHeaderControl templateHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        templateHeaderControl.UpdateOutlinesButton.Visibility = Visibility.Collapsed;
                    }
                    templateHeaderControl.UpdateOutlinesButton.Command = LocalViewModel.UpdateOutlinesCommand;
                }
            };

        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
