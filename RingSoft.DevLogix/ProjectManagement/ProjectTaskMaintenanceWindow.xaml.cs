﻿using System;
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
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;

namespace RingSoft.DevLogix.ProjectManagement
{
    /// <summary>
    /// Interaction logic for ProjectTaskMaintenanceWindow.xaml
    /// </summary>
    public partial class ProjectTaskMaintenanceWindow : IProjectTaskView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Project Task";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;

        private bool _settingUserFocus;

        public ProjectTaskMaintenanceWindow()
        {
            InitializeComponent();

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

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(KeyControl);
            base.OnLoaded();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.ProjectId))
            {
                SetFocusToTab(DetailsTabItem);
                ProjectControl.Focus();
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.UserId))
            {
                SetFocusToTab(DetailsTabItem);
                _settingUserFocus = true;
                UserControl.Focus();
                _settingUserFocus = false;
            }
            else if (fieldDefinition == LocalViewModel.TableDefinition.GetFieldDefinition(p => p.Name))
            {
                KeyControl.Focus();
            }

            base.OnValidationFail(fieldDefinition, text, caption);
        }

        private void SetFocusToTab(TabItem tabItem)
        {
            TabControl.SelectedItem = tabItem;
            tabItem.UpdateLayout();
        }

    public override void ResetViewForNewRecord()
        {
            TabControl.SelectedItem = DetailsTabItem;
            KeyControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void GetNewLineType(string text, out PrimaryKeyValue laborPartPkValue, out LaborPartLineTypes lineType)
        {
            var result = LaborPartLineTypes.LaborPart;
            var window = new TaskLaborPartLtSelectorWindow(text);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            laborPartPkValue = window.ViewModel.NewLaborPartPkValue;
            lineType = window.ViewModel.NewLineType;

        }
        public bool ShowCommentEditor(DataEntryGridMemoValue comment)
        {
            var memoEditor = new DataEntryGridMemoEditor(comment);
            memoEditor.Owner = this;
            memoEditor.Title = "Edit Comment";
            return memoEditor.ShowDialog();
        }

        public void SetTaskReadOnlyMode(bool value)
        {
            UserControl.SetReadOnlyMode(true);
            ProjectControl.SetReadOnlyMode(true);
            HourlyRateControl.IsEnabled = false;
            TimeControl.IsEnabled = false;
            LaborPartsGrid.SetReadOnlyMode(true);
            PercentCompleteControl.IsEnabled = !value;
            NotesControl.SetReadOnlyMode(value);
        }
    }
}