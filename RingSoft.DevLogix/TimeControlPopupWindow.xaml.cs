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
using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for TimeControlPopupWindow.xaml
    /// </summary>
    public partial class TimeControlPopupWindow : ITimeControlPopupView
    {
        public TimeControlPopupWindow(double minutes)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, minutes);
                TimePartControl.TextBox.SelectAll();
            };
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
