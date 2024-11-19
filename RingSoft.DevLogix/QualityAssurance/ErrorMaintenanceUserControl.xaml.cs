using RingSoft.App.Controls;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

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
    /// Interaction logic for ErrorMaintenanceUserControl.xaml
    /// </summary>
    public partial class ErrorMaintenanceUserControl : IErrorView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        private VmUiControl _descriptionUiControl;
        private VmUiControl _resolutionUiControl;

        public ErrorMaintenanceUserControl()
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

            RegisterFormKeyControl(ErrorIdControl);
            _descriptionUiControl = new VmUiControl(DescriptionTextBox, LocalViewModel.DescriptionUiCommand);
            _resolutionUiControl = new VmUiControl(ResolutionTextBox, LocalViewModel.ResolutionUiCommand);
        }

        private void DetailsGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DescriptionTextBox.MaxWidth = DetailsGrid.ActualWidth / 2;
            ResolutionTextBox.MaxWidth = DetailsGrid.ActualWidth / 2;
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
            return "Error";
        }

        public override void ResetViewForNewRecord()
        {
            StatusControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void SetFocusAfterText(string text, bool description, bool setFocus)
        {
            var index = text.Length;
            if (index >= 0)
            {
                if (description)
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
            dateFilterWindow.Owner = OwnerWindow;
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
