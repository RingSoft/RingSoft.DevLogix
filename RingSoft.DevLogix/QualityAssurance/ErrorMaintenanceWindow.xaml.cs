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
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using RingSoft.DevLogix.UserManagement;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class ErrorHeaderControl : DbMaintenanceCustomPanel
    {
        public Button PunchInButton { get; set; }

        static ErrorHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHeaderControl), new FrameworkPropertyMetadata(typeof(ErrorHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ErrorMaintenanceWindow.xaml
    /// </summary>
    public partial class ErrorMaintenanceWindow : IErrorView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Error";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ErrorMaintenanceWindow()
        {
            InitializeComponent();
            DetailsGrid.SizeChanged += DetailsGrid_SizeChanged;
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ErrorHeaderControl errorHeaderControl)
                {
                    errorHeaderControl.PunchInButton.Command =
                        LocalViewModel.PunchInCommand;

                    if (!AppGlobals.LookupContext.ProductVersions.HasRight(RightTypes.AllowAdd))
                    {
                        errorHeaderControl.PunchInButton.Visibility = Visibility.Collapsed;
                    }
                }
            };

        }

        private void DetailsGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DescriptionTextBox.MaxWidth = DetailsGrid.ActualWidth / 2;
            ResolutionTextBox.MaxWidth = DetailsGrid.ActualWidth / 2;
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(ErrorIdControl);
            
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            StatusControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void SetFocusAfterText(string text, bool descrioption, bool setFocus)
        {
            var index = text.Length;
            if (index >= 0)
            {
                if (descrioption)
                {
                    if (setFocus)
                    {
                        DescriptionTextBox.Focus();
                    }

                    DescriptionTextBox.SelectionStart = index;
                }
                else
                {
                    if (setFocus)
                    {
                        ResolutionTextBox.Focus();
                    }

                    ResolutionTextBox.SelectionStart = index;
                }
            }
        }

        public void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }

        public void PunchIn(Error error)
        {
            AppGlobals.MainViewModel.MainView.PunchIn(error);
        }
    }
}
