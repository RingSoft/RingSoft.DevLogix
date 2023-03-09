using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix;assembly=RingSoft.DevLogix"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TimeControl/>
    ///
    /// </summary>
    public class TimeControl : Control, ITimeControlView
    {
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register(nameof(Minutes), typeof(decimal), typeof(TimeControl),
                new FrameworkPropertyMetadata(MinutesChangedCallback));

        public decimal Minutes
        {
            get { return (decimal)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        private static void MinutesChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var timeControl = (TimeControl)obj;
            if (timeControl.StringEditControl == null)
            {
                timeControl._setMinutes = timeControl.Minutes;
            }
            else
            {
                timeControl.ViewModel.Minutes = timeControl.Minutes;
            }
        }

        private Brush _controlBrush;
        public Brush ControlBrush
        {
            get => _controlBrush;
            set
            {
                if (StringEditControl == null)
                {
                    _setBrush = value;
                }
                else
                {
                    StringEditControl.Background = value;
                }
                _controlBrush = value;
            }
        }

        private Brush _selectionBrush;

        public Brush SelectionBrush
        {
            get => _selectionBrush;
            set
            {
                if (StringEditControl == null)
                {
                    _setSelectionBrush = value;
                }
                else
                {
                    StringEditControl.SelectionBrush = value;
                }
                _selectionBrush = value;
            }
        }

        private Brush _foreground;

        public Brush Foreground
        {
            get => _foreground;
            set
            {
                if (StringEditControl == null)
                {
                    _setForegroundBrush = value;
                }
                else
                {
                    StringEditControl.Foreground = value;
                }
                _foreground = value;
            }
        }


        public event EventHandler ControlDirty;

        public Border Border { get; private set; }
        public TimeControlViewModel ViewModel { get; private set; }
        public StringEditControl StringEditControl { get; private set; }
        public Button Button { get; private set; }

        private bool _setFocus;
        private decimal? _setMinutes;
        private Brush _setBrush;
        private Brush _setSelectionBrush;
        private Brush _setForegroundBrush;

        static TimeControl()
        {
            IsTabStopProperty.OverrideMetadata(typeof(TimeControl), new FrameworkPropertyMetadata(false));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeControl), new FrameworkPropertyMetadata(typeof(TimeControl)));
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as TimeControlViewModel;
            StringEditControl = GetTemplateChild(nameof(StringEditControl)) as StringEditControl;
            Button = GetTemplateChild(nameof(Button)) as Button;

            if (_setFocus)
            {
                StringEditControl.Focus();
                StringEditControl.SelectAll();
                _setFocus = false;
            }

            if (_setBrush != null)
            {
                StringEditControl.Background = _setBrush;
                _setBrush = null;
            }

            if (_setSelectionBrush != null)
            {
                StringEditControl.SelectionBrush = _setSelectionBrush;
                _setSelectionBrush = null;
            }

            if (_setForegroundBrush != null)
            {
                StringEditControl.Foreground = _setForegroundBrush;
                _setForegroundBrush = null;
            }
            ViewModel.Initialize(this);
            if (_setMinutes != null)
            {
                ViewModel.Minutes = _setMinutes.Value;
            }
            else
            {
                ViewModel.Minutes = Minutes;
            }
            base.OnApplyTemplate();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (StringEditControl == null)
            {
                _setFocus = true;
            }
            else
            {
                StringEditControl.Focus();
                StringEditControl.SelectAll();
            }
            base.OnGotFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ViewModel.ShowPopupCommand.Execute(null);
            }
            base.OnKeyDown(e);
        }

        public bool LaunchPopup()
        {
            var popup = new TimeControlPopupWindow(Minutes);
            popup.Owner = Window.GetWindow(this);
            popup.ShowInTaskbar = false;
            popup.ShowDialog();
            if (popup.ViewModel.DialogResult)
            {
                Minutes = popup.ViewModel.Minutes;
                StringEditControl.SelectAll();
                ControlDirty?.Invoke(this, EventArgs.Empty);
            }

            return popup.ViewModel.DialogResult;
        }
    }
}
