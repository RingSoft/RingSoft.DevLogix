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
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for GroupsMaintenanceWindow.xaml
    /// </summary>
    public partial class GroupsMaintenanceWindow : IGroupView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "User";
        public override DbMaintenanceViewModelBase ViewModel => GroupMaintenanceViewModel;


        public GroupsMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            base.ResetViewForNewRecord();
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
