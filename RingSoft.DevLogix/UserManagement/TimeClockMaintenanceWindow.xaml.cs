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
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
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

    public class TimeClockHeaderControl : DbMaintenanceCustomPanel
    {
        public Button PunchOutButton { get; set; }

        static TimeClockHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeClockHeaderControl), new FrameworkPropertyMetadata(typeof(TimeClockHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchOutButton = GetTemplateChild(nameof(PunchOutButton)) as Button;

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

        public event EventHandler<GetErrorEventArgs> GetTimeClockError;
        public event EventHandler<GetProjectTaskEventArgs> GetTimeClockProjectTask;

        public TimeClockMaintenanceWindow()
        {
            InitializeComponent();
            TopHeaderControl.Loaded += (sender, args) =>
            {
                TopHeaderControl.PrintButton.Visibility = Visibility.Collapsed;
                TopHeaderControl.NewButton.Visibility = Visibility.Collapsed;
                if (TopHeaderControl.CustomPanel is TimeClockHeaderControl timeClockHeaderControl)
                {
                    timeClockHeaderControl.PunchOutButton.Command =
                        LocalViewModel.PunchOutCommand;
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
        }

        public override void ResetViewForNewRecord()
        {
            NotesControl.Focus();
            base.ResetViewForNewRecord();
        }

        public Error GetError()
        {
            var args = new GetErrorEventArgs();
            GetTimeClockError?.Invoke(this, args);
            return args.Error;
        }

        public ProjectTask GetProjectTask()
        {
            var args = new GetProjectTaskEventArgs();
            GetTimeClockProjectTask?.Invoke(this, args);
            return args.ProjectTask;
        }

        public void SetTimeClockMode(TimeClockModes timeClockMode)
        {
            ErrorControl.Visibility = Visibility.Collapsed;
            ProjectTaskControl.Visibility = Visibility.Collapsed;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeClockMode), timeClockMode, null);
            }
        }

        public void SetElapsedTime()
        {
            Dispatcher?.Invoke(() =>
            {
                ElapsedTimeBox.Text = LocalViewModel.ElapsedTime;
            });
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
