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
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectMaterialMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaterialMaintenanceWindow : IProjectMaterialView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Material";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        public ProjectMaterialMaintenanceWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(KeyControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedItem = DetailsTabItem;
            KeyControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void GetNewLineType(string text, out PrimaryKeyValue materialPartPkValue, out MaterialPartLineTypes lineType)
        {
            var result = MaterialPartLineTypes.MaterialPart;
            var window = new ProjectMaterialPartSelectorWindow(text);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            materialPartPkValue = window.ViewModel.NewMaterialPartPkValue;
            lineType = window.ViewModel.NewLineType;

        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }
    }
}
