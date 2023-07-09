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
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for TimeClockManualPunchOutWindow.xaml
    /// </summary>
    public partial class TimeClockManualPunchOutWindow : ITimeClockManualPunchOutView
    {
        public TimeClockManualPunchOutWindow()
        {
            InitializeComponent();
            LocalViewModel.Initialize(this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
