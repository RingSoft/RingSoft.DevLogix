using System;
using RingSoft.DbLookup;
using RingSoft.DevLogix.Library;
using System.Windows;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for DateLookupFilterWindow.xaml
    /// </summary>
    public partial class DateLookupFilterWindow : IDateFilterView
    {
        public DateLookupFilterWindow(DateFilterInput input)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(this, input);
                Title = $"{input.CodeNameToFilter} {input.ProcessText} Filter Options";
            };
        }

        public void RefreshView()
        {
            CurrentControl.IsEnabled = false;
            StartEndGrid.IsEnabled = false;

            if (LocalViewModel.IsCurrentOnly)
            {
                CurrentControl.IsEnabled = true;
            }
            else
            {
                StartEndGrid.IsEnabled = true;
            }
        }

        public void CloseWindow()
        {
            Close();
        }

        public void PrintOutput()
        {
            
        }

        public void FocusControl(GenericFocusControls control)
        {
            switch (control)
            {
                case GenericFocusControls.Current:
                    CurrentControl.Focus();
                    break;
                case GenericFocusControls.Start:
                    BeginningControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }

        public void FocusDateControl(DateFilterFocusControls focusControl)
        {
            StartDateControl.Focus();
        }
    }
}
