using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.Printing.Interop;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectScheduleView
    {
        void PrintOutput(PrinterSetupArgs printerSetup);

        void CloseWindow();
    }
    public class ProjectScheduleViewModel : INotifyPropertyChanged, IPrintProcessor
    {
        private DateTime _startDate;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                {
                    return;
                }
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private string _projectName;

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName == value)
                    return;

                _projectName = value;
                OnPropertyChanged();
            }
        }


        private double _remainingHours;

        public double RemainingHours
        {
            get => _remainingHours;
            set
            {
                if (_remainingHours == value)
                {
                    return;
                }
                _remainingHours = value;
                OnPropertyChanged();
            }
        }

        private ProjectScheduleGridManager _scheduleManager;

        public ProjectScheduleGridManager ScheduleManager
        {
            get => _scheduleManager;
            set
            {
                if (_scheduleManager == value)
                    return;

                _scheduleManager = value;
                OnPropertyChanged();
            }
        }


        public double RemainingMinutes
        {
            get => RemainingHours * 60;
            private set => RemainingHours = value / 60;
        }

        private DateTime? _calculatedDeadline;

        public DateTime? CalculatedDeadline
        {
            get => _calculatedDeadline;
            set
            {
                if (_calculatedDeadline == value)
                    return;

                _calculatedDeadline = value;
                OnPropertyChanged();
            }
        }


        public Project Project { get; private set; }

        public List<ProjectUser> Users { get; private set; }

        public PrinterSetupArgs PrinterSetup { get; private set; } = new PrinterSetupArgs();

        public IProjectScheduleView View { get; private set; }

        public RelayCommand CalculateCommand { get; private set; }

        public RelayCommand PrintCommand { get; private set; }

        public RelayCommand ApplyCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public bool DialogResult { get; private set; }

        public ProjectScheduleViewModel()
        {
            ScheduleManager = new ProjectScheduleGridManager(this);

            CalculateCommand = new RelayCommand((() =>
            {
                ScheduleManager.CalcSchedule();
                if (ScheduleManager.Rows.Any())
                {
                    PrintCommand.IsEnabled = true;
                    ApplyCommand.IsEnabled = true;
                }
            }));

            PrintCommand = new RelayCommand((() =>
            {
                View.PrintOutput(PrinterSetup);
            }));

            ApplyCommand = new RelayCommand((() =>
            {
                DialogResult = true;
                View.CloseWindow();
            }));

            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));

            PrinterSetup.DataProcessor = this;
            PrinterSetup.PrintingProperties.ReportType = ReportTypes.Custom;
            PrinterSetup.PrintingProperties.CustomReportPathFileName =
                $"{RingSoftAppGlobals.AssemblyDirectory}\\ProjectManagement\\ProjectSchedule.rpt";
            PrinterSetup.PrintingProperties.ReportTitle = "Project Schedule Report";

            PrintCommand.IsEnabled = false;
            ApplyCommand.IsEnabled = false;
        }

        public void Initialize(IProjectScheduleView view, Project project, DateTime? startDate)
        {
            View = view;
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Project>();
            var project1 = table
                .Include(p => p.ProjectTasks)
                .ThenInclude(p => p.SourceDependencies)
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.User)
                .ThenInclude(p => p.UserTimeOff)
                .FirstOrDefault(p => p.Id == project.Id);

            if (project1 != null)
            {
                Project = project;
                Users = project.ProjectUsers.ToList();
                ProjectName = project.Name;
                if (startDate == null)
                {
                    startDate = DateTime.Today;
                }

                StartDate = startDate.GetValueOrDefault();

                ScheduleManager.SetupData(project);
            }

            RemainingMinutes = ScheduleManager.ScheduleData.Sum(p => p.RemainingMinutes);
        }

        public double GetMinutesForDay(DateTime date, ProjectUser projectUser)
        {
            var result = (double)0;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    result = projectUser.SundayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Monday:
                    result = projectUser.MondayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Tuesday:
                    result = projectUser.TuesdayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Wednesday:
                    result = projectUser.WednesdayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Thursday:
                    result = projectUser.ThursdayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Friday:
                    result = projectUser.FridayMinutes.GetValueOrDefault();
                    break;
                case DayOfWeek.Saturday:
                    result = projectUser.SaturdayMinutes.GetValueOrDefault();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(date.DayOfWeek), date.DayOfWeek, null);
            }

            return result;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void ProcessPrintOutputData(PrinterSetupArgs setupArgs)
        {
            var numberSetup = new DecimalEditControlSetup
            {
                FormatType = DecimalEditFormatTypes.Number,
            };

            var headerChunk = new List<PrintingInputHeaderRow>();
            var headerRow = new PrintingInputHeaderRow()
            {
                RowKey = "Header1",
                StringField01 = ProjectName,
                StringField02 = StartDate.FormatDateValue(DbDateTypes.DateOnly),
                StringField03 = CalculatedDeadline.GetValueOrDefault().FormatDateValue(DbDateTypes.DateOnly),
                NumberField01 = numberSetup.FormatValue(RemainingHours),
            };
            headerChunk.Add(headerRow);
            PrintingInteropGlobals.HeaderProcessor.AddChunk(headerChunk, PrinterSetup.PrintingProperties);
            
            ScheduleManager.PrintDetails(setupArgs, headerRow);
        }

        public event EventHandler<PrinterDataProcessedEventArgs>? PrintProcessingHeader;
        public void NotifyProcessingHeader(PrinterDataProcessedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
