using RingSoft.App.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class TestingTemplateHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton UpdateOutlinesButton { get; set; }

        static TestingTemplateHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestingTemplateHeaderControl), new FrameworkPropertyMetadata(typeof(TestingTemplateHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            UpdateOutlinesButton = GetTemplateChild(nameof(UpdateOutlinesButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for TestingTemplatesMaintenanceUserControl.xaml
    /// </summary>
    public partial class TestingTemplatesMaintenanceUserControl
    {
        public TestingTemplatesMaintenanceUserControl()
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
                    templateHeaderControl.UpdateOutlinesButton.ToolTip.HeaderText = "Update Testing Outlines (Ctrl + T,  Ctrl + U)";
                    templateHeaderControl.UpdateOutlinesButton.ToolTip.DescriptionText =
                        "Generate Steps for the attached Testing Outlines";
                }

                var hotKey = new HotKey(LocalViewModel.UpdateOutlinesCommand);
                hotKey.AddKey(Key.T);
                hotKey.AddKey(Key.U);
                AddHotKey(hotKey);
            };
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return LocalViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Testing Template";
        }
    }
}
