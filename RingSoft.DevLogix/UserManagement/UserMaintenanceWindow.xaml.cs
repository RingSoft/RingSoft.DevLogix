using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.UserManagement
{
    /// <summary>
    /// Interaction logic for UserMaintenanceWindow.xaml
    /// </summary>
    public partial class UserMaintenanceWindow : IUserView
    {
        public UserMaintenanceWindow()
        {
            InitializeComponent();
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "User";
        public override DbMaintenanceViewModelBase ViewModel => UserMaintenanceViewModel;

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            RefreshView();
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NameControl.Focus();
            RefreshView();
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

        public void RefreshView()
        {
            if (UserMaintenanceViewModel.EmailAddress.IsNullOrEmpty())
            {
                SendEmailControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                SendEmailControl.Visibility = Visibility.Visible;
                SendEmailControl.Inlines.Clear();
                try
                {
                    var uri = new Uri($"mailto:{UserMaintenanceViewModel.EmailAddress}");
                    var hyperLink = new Hyperlink
                    {
                        NavigateUri = uri
                    };
                    hyperLink.Inlines.Add("Send Email");
                    hyperLink.RequestNavigate += (sender, args) =>
                    {
                        Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
                        args.Handled = true;
                    };
                    SendEmailControl.Inlines.Add(hyperLink);
                }
                catch (Exception e)
                {
                    SendEmailControl.Visibility = Visibility.Collapsed;
                }
            }

            GroupsTab.Visibility = !AppGlobals.LookupContext.Groups.HasRight(RightTypes.AllowView) ? Visibility.Collapsed : Visibility.Visible;
        }

        public void OnValGridFail()
        {
            TabControl.SelectedItem = GroupsTab;
        }


        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == UserMaintenanceViewModel.TableDefinition.GetFieldDefinition(p => p.DepartmentId))
            {
                DepartmentControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
