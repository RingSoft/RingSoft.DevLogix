using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using System.Windows;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for DepartmentMaintenanceWindow.xaml
    /// </summary>
    public partial class DepartmentMaintenanceWindow : IDepartmentView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Department";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public string FtpPassword
        {
            get
            {
                return FtpPasswordBox.Password;
            }
            set
            {
                FtpPasswordBox.Password = value;
            }
        }

        public DepartmentMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);
        }

        protected override void OnLoaded()
        {
            if (!AppGlobals.LookupContext.Users.HasRight(RightTypes.AllowAdd))
            {
                AddModifyUserButton.Visibility = Visibility.Collapsed;
            }
            base.OnLoaded();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.Description))
            {
                DescriptionControl.Focus();
            }
            if (fieldDefinition == AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.ErrorFixStatusId))
            {
                TabControl.SelectedIndex = 0;
                TabControl.UpdateLayout();
                FixStatusControl.Focus();
            }

            if (fieldDefinition == AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.ErrorPassStatusId))
            {
                TabControl.SelectedIndex = 0;
                TabControl.UpdateLayout();
                PassStatusControl.Focus();
            }
            if (fieldDefinition == AppGlobals.LookupContext.Departments.GetFieldDefinition(p => p.ErrorFailStatusId))
            {
                TabControl.SelectedIndex = 0;
                TabControl.UpdateLayout();
                FailStatusControl.Focus();
            }

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
