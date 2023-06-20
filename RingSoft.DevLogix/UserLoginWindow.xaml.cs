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
    /// Interaction logic for UserLoginWindow.xaml
    /// </summary>
    public partial class UserLoginWindow : IUserLoginView
    {
        public UserLoginWindow(int userId)
        {
            InitializeComponent();
            Loaded += (sender, args) => ViewModel.Initialize(this, userId);
            if (userId > 0)
            {
                UserControl.IsEnabled = false;
            }
        }

        public void CloseWindow()
        {
            Close();
        }

        public string GetPassword()
        {
            return PasswordBox.Password;
        }

        public void EnablePassword(bool enable)
        {
            PasswordBox.IsEnabled = enable;
        }
    }
}
