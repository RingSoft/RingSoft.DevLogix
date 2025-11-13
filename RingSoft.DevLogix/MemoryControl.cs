using RingSoft.DataEntryControls.WPF;
using RingSoft.DevLogix.Library.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    ///     <MyNamespace:MemoryControl/>
    ///
    /// </summary>
    public class MemoryControl : Control, IMemoryControlView
    {
        public static readonly DependencyProperty MemoryProperty =
            DependencyProperty.Register(nameof(Memory), typeof(double), typeof(MemoryControl),
                new FrameworkPropertyMetadata(MemoryChangedCallback));

        public double Memory
        {
            get { return (double)GetValue(MemoryProperty); }
            set { SetValue(MemoryProperty, value); }
        }

        private static void MemoryChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var memoryControl = (MemoryControl)obj;
            if (memoryControl.StringEditControl == null)
            {
                memoryControl._setMegabs = memoryControl.Memory;
            }
            else
            {
                memoryControl.ViewModel.LocalMemory = memoryControl.Memory;
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
        public MemoryControlViewModel ViewModel { get; private set; }
        public StringEditControl StringEditControl { get; private set; }
        public Button Button { get; private set; }

        private bool _setFocus;
        private double? _setMegabs;
        private Brush _setBrush;
        private Brush _setSelectionBrush;
        private Brush _setForegroundBrush;

        static MemoryControl()
        {
            IsTabStopProperty.OverrideMetadata(typeof(MemoryControl), new FrameworkPropertyMetadata(false));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(MemoryControl), new FrameworkPropertyMetadata(typeof(MemoryControl)));
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as MemoryControlViewModel;
            StringEditControl = GetTemplateChild(nameof(StringEditControl)) as StringEditControl;
            Button = GetTemplateChild(nameof(Button)) as Button;

            StringEditControl.KeyDown += (sender, args) =>
            {
                DevLogixAppStart.ProcessPartKeyDown(args, ViewModel.ShowPopupCommand);
            };


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
            if (_setMegabs != null)
            {
                ViewModel.LocalMemory = _setMegabs.Value;
            }
            else
            {
                ViewModel.LocalMemory = Memory;
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
            var popup = new MemoryControlPopupWindow(Memory);
            popup.Owner = Window.GetWindow(this);
            popup.ShowInTaskbar = false;
            popup.ShowDialog();
            if (popup.ViewModel.DialogResult)
            {
                Memory = popup.ViewModel.Memory;
                StringEditControl.SelectAll();
                ControlDirty?.Invoke(this, EventArgs.Empty);
            }

            return popup.ViewModel.DialogResult;
        }
    }
}
