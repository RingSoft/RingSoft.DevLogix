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
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectMaterialHeaderControl : DbMaintenanceCustomPanel
    {
        public Button RecalcButton { get; set; }

        public Button PostButton { get; set; }

        static ProjectMaterialHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectMaterialHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectMaterialHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as Button;
            PostButton = GetTemplateChild(nameof(PostButton)) as Button;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for ProjectMaterialMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectMaterialMaintenanceWindow : IProjectMaterialView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Material";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public RecalcProcedure RecalcProcedure { get; set; }


        public ProjectMaterialMaintenanceWindow()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is ProjectMaterialHeaderControl projectMaterialHeaderControl)
                {
                    if (!LocalViewModel.TableDefinition.HasRight(RightTypes.AllowEdit))
                    {
                        projectMaterialHeaderControl.RecalcButton.Visibility = Visibility.Collapsed;
                    }

                    if (!LocalViewModel.TableDefinition.HasSpecialRight((int)ProjectMaterialSpecialRights
                            .AllowMaterialPost))
                    {
                        projectMaterialHeaderControl.PostButton.Visibility = Visibility.Collapsed;
                    }
                    projectMaterialHeaderControl.RecalcButton.Command = LocalViewModel.RecalcCommand;
                    projectMaterialHeaderControl.PostButton.Command = LocalViewModel.PostCommand;
                }
            };

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(KeyControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            //TabControl.SelectedItem = DetailsTabItem;
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

        public bool DoPostCosts(Project project)
        {
            var postCostWindow = new ProjectMaterialPostWindow(project);
            postCostWindow.Owner = this;
            postCostWindow.ShowInTaskbar = false;
            postCostWindow.ShowDialog();
            return true;
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            var genericInput = new GenericReportLookupFilterInput
            {
                LookupDefinitionToFilter = lookupDefinition,
                CodeNameToFilter = "Project Task",
                KeyAutoFillValue = LocalViewModel.KeyAutoFillValue,
                ProcessText = "Recalculate"
            };
            var genericWindow = new GenericReportFilterWindow(genericInput);
            genericWindow.Owner = this;
            genericWindow.ShowInTaskbar = false;
            genericWindow.ShowDialog();
            return genericWindow.ViewModel.DialogReesult;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupDefinition)
        {
            var result = string.Empty;
            RecalcProcedure = new RecalcProcedure();
            RecalcProcedure.StartRecalculate += (sender, args) =>
            {
                result = LocalViewModel.StartRecalcProcedure(lookupDefinition);
            };
            RecalcProcedure.Start();
            return result;
        }

        public void UpdateRecalcProcedure(int currentProjectTask, int totalProjectTasks, string currentProjectTaskText)
        {
            var progress = $"Recalculating Project {currentProjectTaskText} {currentProjectTask} / {totalProjectTasks}";
            RecalcProcedure.SplashWindow.SetProgress(progress);
        }
    }
}
