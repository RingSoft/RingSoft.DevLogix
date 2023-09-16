using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.App.Controls;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Library.ViewModels.CustomerManagement;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;
using RingSoft.DevLogix.ProjectManagement;
using RingSoft.DevLogix.UserManagement;
using Clipboard = System.Windows.Clipboard;
using Control = System.Windows.Controls.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace RingSoft.DevLogix.QualityAssurance
{
    public class ErrorHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchInButton { get; set; }

        public DbMaintenanceButton RecalculateButton { get; set; }

        static ErrorHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHeaderControl), new FrameworkPropertyMetadata(typeof(ErrorHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchInButton = GetTemplateChild(nameof(PunchInButton)) as DbMaintenanceButton;
            RecalculateButton = GetTemplateChild(nameof(RecalculateButton)) as DbMaintenanceButton;

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
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
        public RecalcProcedure RecalcProcedure { get; set; }

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
                    errorHeaderControl.RecalculateButton.Command =
                    LocalViewModel.RecalcCommand;

                    errorHeaderControl.PunchInButton.ToolTip.HeaderText = "Punch In (Alt + U)";
                    errorHeaderControl.PunchInButton.ToolTip.DescriptionText = "Punch into this Product Error. ";

                    errorHeaderControl.RecalculateButton.ToolTip.HeaderText = "Recalculate Cost (Alt + R)";
                    errorHeaderControl.RecalculateButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of product errors.";

                    if (!AppGlobals.LookupContext.TimeClocks.HasRight(RightTypes.AllowAdd))
                    {
                        errorHeaderControl.PunchInButton.Visibility = Visibility.Collapsed;
                    }
                    if (!AppGlobals.LookupContext.TimeClocks.HasRight(RightTypes.AllowAdd))
                    {
                        errorHeaderControl.PunchInButton.Visibility = Visibility.Collapsed;
                    }
                    if (!AppGlobals.LookupContext.Errors.HasRight(RightTypes.AllowEdit))
                    {
                        errorHeaderControl.RecalculateButton.Visibility = Visibility.Collapsed;
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
            AppGlobals.MainViewModel.PunchIn(error);
        }

        public bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup)
        {
            var genericInput = new DateFilterInput()
            {
                LookupDefinitionToFilter = lookup,
                CodeNameToFilter = "Error",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate",
                DateFieldToFilter = AppGlobals.LookupContext.Errors.GetFieldDefinition(p => p.ErrorDate),
            };
            var dateFilterWindow = new DateLookupFilterWindow(genericInput);
            dateFilterWindow.Owner = this;
            dateFilterWindow.ShowInTaskbar = false;
            dateFilterWindow.ShowDialog();
            return dateFilterWindow.LocalViewModel.DialogReesult;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookup)
        {
            var result = string.Empty;
            RecalcProcedure = new RecalcProcedure();
            RecalcProcedure.StartRecalculate += (sender, args) =>
            {
                result = LocalViewModel.StartRecalculateProcedure(lookup);
            };
            RecalcProcedure.Start();
            return result;

        }

        public void UpdateRecalcProcedure(int currentError, int totalErrors, string currentErrorText)
        {
            var progress = $"Recalculating Error {currentErrorText} {currentError} / {totalErrors}";
            RecalcProcedure.SplashWindow.SetProgress(progress);

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var ctrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (ctrlKeyDown)
            {
                switch (e.Key)
                {
                    case Key.T:
                        TabControl.Focus();
                        if (TabControl.SelectedItem is TabItem tabItem)
                        {
                            tabItem.Focus();
                            e.Handled = true;
                        }
                        break;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == PdGroupBox)
            {
                PdAutoFillControl.SetReadOnlyMode(readOnlyValue);

                PdButtonsPanel.IsEnabled = !readOnlyValue;
                return;
            }
            if (control == QaGroupBox)
            {
                QaAutoFillControl.SetReadOnlyMode(readOnlyValue);

                QaButtonsPanel.IsEnabled = !readOnlyValue;
                return;
            }

            if (control == DescriptionTextBox)
            {
                DescriptionTextBox.IsReadOnly = readOnlyValue;
                DescriptionTextBox.IsReadOnlyCaretVisible = readOnlyValue;
                return;
            }
            if (control == ResolutionTextBox)
            {
                ResolutionTextBox.IsReadOnly = readOnlyValue;
                ResolutionTextBox.IsReadOnlyCaretVisible = readOnlyValue;
                return;
            }

            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
