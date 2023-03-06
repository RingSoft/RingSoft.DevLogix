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
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for TaskLaborPartLtSelectorWindow.xaml
    /// </summary>
    public partial class TaskLaborPartLtSelectorWindow : ITaskLaborPartView
    {
        public TaskLaborPartLtSelectorWindow(string keyText)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, keyText);
            };
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
