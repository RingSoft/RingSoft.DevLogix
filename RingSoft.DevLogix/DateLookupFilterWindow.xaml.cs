using RingSoft.DbLookup;
using System.Windows;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for DateLookupFilterWindow.xaml
    /// </summary>
    public partial class DateLookupFilterWindow : IGenericReportFilterView
    {
        public DateLookupFilterWindow(GenericReportLookupFilterInput input)
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize(this, input);
                Title = $"{input.CodeNameToFilter} {input.ProcessText} Filter Options";
            };
        }

        public void RefreshView()
        {
            
        }

        public void CloseWindow()
        {
            Close();
        }

        public void PrintOutput()
        {
            
        }

        public void FocusControl(GenericFocusControls control)
        {
            
        }
    }
}
