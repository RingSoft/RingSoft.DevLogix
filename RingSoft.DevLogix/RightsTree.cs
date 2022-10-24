using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DevLogix.Library;

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
    ///     <MyNamespace:RightsTree/>
    ///
    /// </summary>
    public class RightsTree : Control, IReadOnlyControl
    {
        public Border Border { get; set; }

        public RightsTreeViewModel ViewModel { get; set; }

        static RightsTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RightsTree), new FrameworkPropertyMetadata(typeof(RightsTree)));
        }

        public RightsTree()
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize();
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;

            if (Border == null)
            {
                throw new ApplicationException("Need to set Border");
            }

            ViewModel = Border.TryFindResource("RightsViewModel") as RightsTreeViewModel;
            SetReadOnlyMode(false);
            
            base.OnApplyTemplate();
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            ViewModel.SetReadOnlyMode(readOnlyValue);
        }
    }
}
