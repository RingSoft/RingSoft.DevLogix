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
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectTaskMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectTaskMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Task";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        private bool _settingUserFocus;

        public ProjectTaskMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(KeyControl);
            UserControl.GotFocus += (sender, args) =>
            {
                if (!LocalViewModel.ProjectAutoFillValue.IsValid() && !_settingUserFocus)
                {
                    _settingUserFocus = true;
                    var message = "You must first select a valid project";
                    var caption = "Invalid Project";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    _settingUserFocus = false;
                    ProjectControl.Focus();
                }
            };
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.ProjectId))
            {
                ProjectControl.Focus();
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.UserId))
            {
                UserControl.Focus();
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.Name))
            {
                KeyControl.Focus();
            }
        base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
