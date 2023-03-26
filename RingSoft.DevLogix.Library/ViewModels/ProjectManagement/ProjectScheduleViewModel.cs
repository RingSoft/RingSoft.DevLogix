using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectScheduleViewModel : INotifyPropertyChanged
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


        private decimal _remainingHours;

        public decimal RemainingHours
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


        public decimal RemainingMinutes
        {
            get => RemainingHours * 60;
            private set => RemainingHours = value / 60;
        }

        public Project Project { get; private set; }

        public List<ProjectUser> Users { get; private set; }

        public RelayCommand CalculateCommand { get; private set; }

        public ProjectScheduleViewModel()
        {
            ScheduleManager = new ProjectScheduleGridManager(this);

            CalculateCommand = new RelayCommand((() =>
            {
                ScheduleManager.CalcSchedule();
            }));
        }

        public void Initialize(Project project, DateTime? startDate)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Project>();
            project = table
                .Include(p => p.ProjectTasks)
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.User)
                .ThenInclude(p => p.UserTimeOff)
                .FirstOrDefault(p => p.Id == project.Id);

            Project = project;
            Users = project.ProjectUsers.ToList();
            ProjectName = project.Name;
            if (startDate == null)
            {
                startDate = DateTime.Today;
            }

            StartDate = startDate.GetValueOrDefault();
            
            ScheduleManager.SetupData(project);
            RemainingMinutes = ScheduleManager.ScheduleData.Sum(p => p.RemainingMinutes);
        }

        public decimal GetMinutesForDay(DateTime date, ProjectUser projectUser)
        {
            var result = (decimal)0;

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
    }
}
