﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.QualityAssurance;

namespace RingSoft.DevLogix.UserManagement
{
    public class GetErrorEventArgs
    {
        public Error Error { get; set; }
    }

    public class GetProjectTaskEventArgs
    {
        public ProjectTask ProjectTask { get; set; }
    }

    public class GetTestingOutlineEventArgs
    {
        public TestingOutline TestingOutline { get; set; }
    }

    public class TimeClockHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchOutButton { get; set; }

        static TimeClockHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeClockHeaderControl), new FrameworkPropertyMetadata(typeof(TimeClockHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchOutButton = GetTemplateChild(nameof(PunchOutButton)) as DbMaintenanceButton;

            base.OnApplyTemplate();
        }
    }

    /// <summary>
    /// Interaction logic for TimeClockMaintenanceWindow.xaml
    /// </summary>
    public partial class TimeClockMaintenanceWindow : ITimeClockView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Time Clock Entry";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public event EventHandler<GetErrorEventArgs> GetTimeClockError;
        public event EventHandler<GetProjectTaskEventArgs> GetTimeClockProjectTask;
        public event EventHandler<GetTestingOutlineEventArgs> GetTimeClockTestingOutline;

        private bool _isActive = true;

        public TimeClockMaintenanceWindow(Error error)
        {
            InitializeComponent();
            SetupControl();
            Loaded += (sender, args) =>
            {
                LocalViewModel.PunchIn(error);
            };
        }

        public TimeClockMaintenanceWindow(ProjectTask task)
        {
            InitializeComponent();
            SetupControl();
            Loaded += (sender, args) =>
            {
                LocalViewModel.PunchIn(task);
            };
        }

        public TimeClockMaintenanceWindow(TestingOutline outline)
        {
            InitializeComponent();
            SetupControl();
            Loaded += (sender, args) =>
            {
                LocalViewModel.PunchIn(outline);
            };
        }


        public TimeClockMaintenanceWindow()
        {
            InitializeComponent();
            SetupControl();
        }

        private void SetupControl()
        {
            TopHeaderControl.Loaded += (sender, args) =>
            {
                TopHeaderControl.PrintButton.Visibility = Visibility.Collapsed;
                TopHeaderControl.NewButton.Visibility = Visibility.Collapsed;
                if (TopHeaderControl.CustomPanel is TimeClockHeaderControl timeClockHeaderControl)
                {
                    timeClockHeaderControl.PunchOutButton.Command =
                        LocalViewModel.PunchOutCommand;
                    timeClockHeaderControl.PunchOutButton.ToolTip.HeaderText = "Punch Out (Alt + P)";
                    timeClockHeaderControl.PunchOutButton.ToolTip.DescriptionText =
                        "Punch Out of the current Time Clock record.";
                }

                if (Processor is AppDbMaintenanceWindowProcessor processor)
                {
                    if (processor.MaintenanceButtonsControl is DbMaintenanceTopHeaderControl buttonsControl)
                    {
                        buttonsControl.SaveSelectButton.Visibility = Visibility.Collapsed;
                    }
                }
            };
            NotesControl.Loaded += (sender, args) =>
            {
                NotesControl.TextBox.Focus();
                NotesControl.MaxWidth = NotesControl.ActualWidth;
                NotesControl.MaxHeight = NotesControl.ActualHeight;
            };

            Closed += (sender, args) => { _isActive = false; };
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(NameControl);
            base.OnLoaded();
        }

        public override void ResetViewForNewRecord()
        {
            NotesControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == AppGlobals.LookupContext.TimeClocks.GetFieldDefinition(p => p.PunchOutDate))
            {
                PunchOutControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
        }

        public void SetTimeClockMode(TimeClockModes timeClockMode)
        {
            ErrorControl.Visibility = Visibility.Collapsed;
            ProjectTaskControl.Visibility = Visibility.Collapsed;
            TestingOutlineControl.Visibility = Visibility.Collapsed;

            switch (timeClockMode)
            {
                case TimeClockModes.Error:
                    KeyLabel.Content = "Error";
                    ErrorControl.Visibility = Visibility.Visible;
                    break;
                case TimeClockModes.ProjectTask:
                    KeyLabel.Content = "Project Task";
                    ProjectTaskControl.Visibility = Visibility.Visible;
                    break;
                case TimeClockModes.TestingOutline:
                    KeyLabel.Content = "Testing Outline";
                    TestingOutlineControl.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeClockMode), timeClockMode, null);
            }
        }

        public void SetElapsedTime()
        {
            if (_isActive)
            {
                Dispatcher?.Invoke(() => { ElapsedTimeBox.Text = LocalViewModel.ElapsedTime; });
            }
        }

        public void FocusNotes()
        {
            if (NotesControl != null)
            {
                NotesControl.TextBox.Focus();
            }
        }

        public void SetDialogMode()
        {
            TopHeaderControl.FindButton.Visibility = Visibility.Collapsed;
            TopHeaderControl.NextButton.Visibility = Visibility.Collapsed;
            TopHeaderControl.PreviousButton.Visibility = Visibility.Collapsed;
            TopHeaderControl.DeleteButton.Visibility = Visibility.Collapsed;
            TopHeaderControl.NewButton.Visibility = Visibility.Collapsed;
        }

    }
}
