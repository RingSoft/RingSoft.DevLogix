using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for GroupsMaintenanceUserControl.xaml
    /// </summary>
    public partial class GroupsMaintenanceUserControl : IGroupView
    {
        public GroupsMaintenanceUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return GroupMaintenanceViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Group";
        }

        public string GetRights()
        {
            return RightsTree.ViewModel.Rights.GetRightsString();
        }

        public void LoadRights(string rightsString)
        {
            RightsTree.ViewModel.LoadRights(rightsString);
        }

        public void ResetRights()
        {
            RightsTree.ViewModel.Reset();
        }
    }
}
