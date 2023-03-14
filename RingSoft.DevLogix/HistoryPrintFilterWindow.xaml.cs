using RingSoft.DevLogix.Library.ViewModels;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for HistoryPrintFilterWindow.xaml
    /// </summary>
    public partial class HistoryPrintFilterWindow : IHistoryFilterView
    {
        public HistoryPrintFilterWindow(HistoryPrintFilterCallBack callback)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, callback);
                BeginDateControl.SelectAll();
            };
        }

        public void CloseWindow()
        {
            Close();
        }

        public void SetValFailFocus()
        {
            BeginDateControl.Focus();
        }

    }


}
