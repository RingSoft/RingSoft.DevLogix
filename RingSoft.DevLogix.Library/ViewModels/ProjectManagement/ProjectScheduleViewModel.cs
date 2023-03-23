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

        public decimal GetMinutesForDay(DayOfWeek dayOfWeek)
        {
            var result = (decimal)0;

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    result = Users.Sum(p => p.SundayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Monday:
                    result = Users.Sum(p => p.MondayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Tuesday:
                    result = Users.Sum(p => p.TuesdayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Wednesday:
                    result = Users.Sum(p => p.WednesdayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Thursday:
                    result = Users.Sum(p => p.ThursdayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Friday:
                    result = Users.Sum(p => p.FridayMinutes).GetValueOrDefault();
                    break;
                case DayOfWeek.Saturday:
                    result = Users.Sum(p => p.SaturdayMinutes).GetValueOrDefault();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
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
