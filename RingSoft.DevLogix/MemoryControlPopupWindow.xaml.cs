using ScottPlot.Drawing.Colormaps;
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
    /// Interaction logic for MemoryControlPopupWindow.xaml
    /// </summary>
    public partial class MemoryControlPopupWindow : IMemoryControlPopupView
    {
        public MemoryControlPopupWindow(double memory)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, memory);
                TimePartControl.TextBox.SelectAll();
            };

        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
