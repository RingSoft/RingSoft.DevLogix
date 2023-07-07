using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DevLogix
{
    public class SupportTimePurchasedControl : StringReadOnlyBox
    {
        public void SetTimeRemaining(Label controlLabel, string? timeRemaining, double? supportMinutesLeft = null)
        {
            if (timeRemaining == null)
            {
                controlLabel.Visibility = Visibility.Collapsed;
                Visibility = Visibility.Collapsed;
            }
            else
            {
                controlLabel.Visibility = Visibility.Visible;
                Visibility = Visibility.Visible;
                Background = new SolidColorBrush(Colors.Red);
                Foreground = new SolidColorBrush(Colors.White);
                Text = timeRemaining;
            }
        }
    }
}
