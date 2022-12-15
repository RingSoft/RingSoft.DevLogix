using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for ProductUpdateVersionsWindow.xaml
    /// </summary>
    public partial class ProductUpdateVersionsWindow : IProductUpdateVersionsView
    {
        public ProductUpdateVersionsWindow(ProductViewModel viewModel)
        {
            InitializeComponent();

            ProductUpdateVersionsViewModel.Initialize(this, viewModel);
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ValidationFailed(ValidationFailControls validationFailControls, string message, string caption)
        {
            ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            switch (validationFailControls)
            {
                case ValidationFailControls.Existing:
                    ExistingControl.Focus();
                    break;
                case ValidationFailControls.New:
                    NewControl.Focus();
                    break;
                case ValidationFailControls.Version:
                    UpdateControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(validationFailControls), validationFailControls, null);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult = ProductUpdateVersionsViewModel.DialogResult;
            base.OnClosing(e);
        }
    }
}
