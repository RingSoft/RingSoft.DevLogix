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
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.QualityAssurance;
using RingSoft.DevLogix.Sqlite.Migrations;
using Customer = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.Customer;
using SupportTicket = RingSoft.DevLogix.DataAccess.Model.CustomerManagement.SupportTicket;

namespace RingSoft.DevLogix.UserManagement
{
    public class TimeClockHeaderControl : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton PunchOutButton { get; private set; }

        public DbMaintenanceButton ManualPunchOutButton { get; private set; }

        static TimeClockHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeClockHeaderControl), new FrameworkPropertyMetadata(typeof(TimeClockHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PunchOutButton = GetTemplateChild(nameof(PunchOutButton)) as DbMaintenanceButton;
            ManualPunchOutButton = GetTemplateChild(nameof(ManualPunchOutButton)) as DbMaintenanceButton;

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

        public TimeClockHeaderControl TimeClockHeaderControl { get; private set; }

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

        public TimeClockMaintenanceWindow(Customer customer)
        {
            InitializeComponent();
            SetupControl();
            Loaded += (sender, args) =>
            {
                LocalViewModel.PunchIn(customer);
            };
        }
        
        public TimeClockMaintenanceWindow(SupportTicket ticket)
        {
            InitializeComponent();
            SetupControl();
            Loaded += (sender, args) =>
            {
                LocalViewModel.PunchIn(ticket);
            };
        }

        public TimeClockMaintenanceWindow()
        {
            InitializeComponent();
            SetupControl();
        }

        private void SetupControl()
        {
            PreviewKeyDown += (sender, args) =>
            {
                var ctrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                if (ctrlKeyDown)
                {
                    switch (args.Key)
                    {
                        case Key.L:
                            if (ErrorControl.Visibility == Visibility.Visible)
                            {
                                ErrorControl.ShowLookupWindow();
                            }
                            if (TestingOutlineControl.Visibility == Visibility.Visible)
                            {
                                TestingOutlineControl.ShowLookupWindow();
                            }
                            if (ProjectTaskControl.Visibility == Visibility.Visible)
                            {
                                ProjectTaskControl.ShowLookupWindow();
                            }
                            if (CustomerControl.Visibility == Visibility.Visible)
                            {
                                CustomerControl.ShowLookupWindow();
                            }
                            if (SupportTicketControl.Visibility == Visibility.Visible)
                            {
                                SupportTicketControl.ShowLookupWindow();
                            }
                            break;
                        case Key.U:
                            UserControl.ShowLookupWindow();
                            break;
                    }
                }
            };
            TopHeaderControl.Loaded += (sender, args) =>
            {
                TopHeaderControl.PrintButton.Visibility = Visibility.Collapsed;
                TopHeaderControl.NewButton.Visibility = Visibility.Collapsed;
                if (TopHeaderControl.CustomPanel is TimeClockHeaderControl timeClockHeaderControl)
                {
                    TimeClockHeaderControl = timeClockHeaderControl;
                    timeClockHeaderControl.PunchOutButton.Command =
                        LocalViewModel.PunchOutCommand;
                    timeClockHeaderControl.PunchOutButton.ToolTip.HeaderText = "Punch Out (Alt + P)";
                    timeClockHeaderControl.PunchOutButton.ToolTip.DescriptionText =
                        "Punch Out of the current Time Clock record.";

                    timeClockHeaderControl.ManualPunchOutButton.Command =
                        LocalViewModel.ManualPunchOutCommand;
                    timeClockHeaderControl.PunchOutButton.ToolTip.HeaderText = "Manual Punch Out (Alt + M)";
                    timeClockHeaderControl.PunchOutButton.ToolTip.DescriptionText =
                        "Punch Out via Manual Time Entry.";
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
            KeyLabel.Visibility = Visibility.Visible;
            ErrorControl.Visibility = Visibility.Collapsed;
            ProjectTaskControl.Visibility = Visibility.Collapsed;
            TestingOutlineControl.Visibility = Visibility.Collapsed;
            CustomerControl.Visibility = Visibility.Collapsed;
            SupportTicketLabel.Visibility = Visibility.Collapsed;
            SupportTicketControl.Visibility = Visibility.Collapsed;
            CustomerTimeRemLabel.Visibility = Visibility.Collapsed;
            CustomerTimeRemControl.Visibility = Visibility.Collapsed;

            var keyText = string.Empty;
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
                case TimeClockModes.Customer:
                    KeyLabel.Content = "Customer";
                    CustomerControl.Visibility = Visibility.Visible;
                    break;
                case TimeClockModes.SupportTicket:
                    KeyLabel.Visibility = Visibility.Collapsed;
                    SupportTicketLabel.Visibility = Visibility.Visible;
                    SupportTicketControl.Visibility = Visibility.Visible;
                    keyText = "Support Ticket";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeClockMode), timeClockMode, null);
            }

            if (keyText.IsNullOrEmpty())
            {
                keyText = KeyLabel.Content.ToString();
            }
            var message = $"(Ctrl + L to Launch the {keyText} Lookup)\r\n";
            message += "(Ctrl + U to Launch the User Lookup)";
            KeyShortcutLabel.Content = message;
        }

        public void SetElapsedTime()
        {
            if (_isActive)
            {
                Dispatcher?.Invoke(() =>
                {
                    ElapsedTimeBox.Text = LocalViewModel.ElapsedTime;
                    if (SupportTicketControl.Visibility == Visibility.Visible
                        && LocalViewModel.PunchOutDate == null)
                    {
                        CustomerTimeRemControl.SetTimeRemaining(
                            CustomerTimeRemLabel
                            , LocalViewModel.SupportTimeLeft
                            , LocalViewModel.SupportMinutesLeft);
                    }
                });
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

        public bool GetManualPunchOutDate(out DateTime? punchInDate, out DateTime? punchOutDate)
        {
            var punchOutDateWindow = new TimeClockManualPunchOutWindow();
            punchOutDateWindow.Owner = this;
            punchOutDateWindow.ShowInTaskbar = false;
            punchOutDateWindow.ShowDialog();
            var result = punchOutDateWindow.LocalViewModel.DialogResult;
            punchInDate = punchOutDateWindow.LocalViewModel.PunchInDate;
            punchOutDate = punchOutDateWindow.LocalViewModel.PunchOutDate;
            return result;
        }
    }
}
