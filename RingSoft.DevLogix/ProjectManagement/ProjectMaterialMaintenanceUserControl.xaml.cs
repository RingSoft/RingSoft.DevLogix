using RingSoft.App.Controls;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;

namespace RingSoft.DevLogix.ProjectManagement
{
    public class ProjectMaterialHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton RecalcButton { get; set; }

        public DbMaintenanceButton PostButton { get; set; }

        static ProjectMaterialHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectMaterialHeaderControl), new FrameworkPropertyMetadata(typeof(ProjectMaterialHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            RecalcButton = GetTemplateChild(nameof(RecalcButton)) as DbMaintenanceButton;
            PostButton = GetTemplateChild(nameof(PostButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }
    /// <summary>
    /// Interaction logic for ProjectMaterialMaintenanceUserControl.xaml
    /// </summary>
    public partial class ProjectMaterialMaintenanceUserControl : IProjectMaterialView
    {
        public RecalcProcedure RecalcProcedure { get; set; }

        public ProjectMaterialMaintenanceUserControl()
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

                    projectMaterialHeaderControl.RecalcButton.ToolTip.HeaderText = "Recalculate Cost";
                    projectMaterialHeaderControl.RecalcButton.ToolTip.DescriptionText =
                        "Recalculate the cost values for a range of project materials.";

                    projectMaterialHeaderControl.PostButton.ToolTip.HeaderText = "Post Costs";
                    projectMaterialHeaderControl.PostButton.ToolTip.DescriptionText =
                        "Post actual costs for this project material.";

                }
            };
            RegisterFormKeyControl(KeyControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return LocalViewModel;
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
            return "Project Material";
        }

        public bool GetNewLineType(string text, out PrimaryKeyValue materialPartPkValue, out MaterialPartLineTypes lineType)
        {
            var result = MaterialPartLineTypes.MaterialPart;
            var window = new ProjectMaterialPartSelectorWindow(text);
            window.Owner = OwnerWindow;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            materialPartPkValue = window.ViewModel.NewMaterialPartPkValue;
            lineType = window.ViewModel.NewLineType;
            return window.ViewModel.Result;
        }

        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = OwnerWindow;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }

        public bool DoPostCosts(Project project)
        {
            var postCostWindow = new ProjectMaterialPostWindow(project);
            postCostWindow.Owner = OwnerWindow;
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
            genericWindow.Owner = OwnerWindow;
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

        public void GotoGrid()
        {
            TabControl.SelectedItem = MaterialPartsTabItem;
            MaterialPartsGrid.Focus();
        }
    }
}
