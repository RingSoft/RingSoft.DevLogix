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
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for UserClockReasonWindow.xaml
    /// </summary>
    public partial class UserClockReasonWindow : IUserClockReasonView
    {
        public UserClockReasonWindow(User user)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(this, user);
            };
        }

        public void CloseWindow()
        {
            Close();
        }

        public void EnableOther(bool enable)
        {   
            OtherStringControl.IsEnabled = enable;
        }

        public void SetFocusToOther()
        {
            OtherStringControl.Focus();
        }
    }
}
