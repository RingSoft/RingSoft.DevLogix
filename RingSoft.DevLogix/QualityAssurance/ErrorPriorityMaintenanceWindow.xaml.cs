using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
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
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DevLogix.Library;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DevLogix.QualityAssurance
{
    /// <summary>
    /// Interaction logic for ErrorPriorityMaintenanceWindow.xaml
    /// </summary>
    public partial class ErrorPriorityMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Error Priority";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ErrorPriorityMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == AppGlobals.LookupContext.ErrorPriorities.GetFieldDefinition(p => p.Level))
            {
                LevelControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
