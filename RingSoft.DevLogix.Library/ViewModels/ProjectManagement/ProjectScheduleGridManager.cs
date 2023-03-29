using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public enum ProjectScheduleColumns
    {
        Date = 1,
        Description = 2,
        HoursWorked = 3,
        HoursRemaining = 4,
    }

    public class ProjectScheduleData
    {
        public ProjectTask ProjectTask { get; set; }

        public decimal RemainingMinutes { get; set; }

        public int Priority { get; set; }
    }

    public class UserScheduleData
    {
        public ProjectUser ProjectUser { get; set; }

        public DateTime? NextScheduleDate { get; set; }
    }

    public class ProjectScheduleGridManager : DataEntryGridManager
    {
        public const int DateColumnId = (int)ProjectScheduleColumns.Date;
        public const int DescriptionColumnId = (int)ProjectScheduleColumns.Description;
        public const int HoursWorkedColumnId = (int)ProjectScheduleColumns.HoursWorked;
        public const int HoursRemainingColumnId = (int)ProjectScheduleColumns.HoursRemaining;

        public ProjectScheduleViewModel ViewModel { get; private set; }

        public List<ProjectScheduleData> ScheduleData { get; private set; } = new List<ProjectScheduleData>();

        public List<UserScheduleData> UserScheduleData { get; private set; } = new List<UserScheduleData>();

        private List<ProjectScheduleGridRow> _newRows = new List<ProjectScheduleGridRow>();

        public ProjectScheduleGridManager(ProjectScheduleViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        public void SetupData(Project project)
        {
            ScheduleData.Clear();
            foreach (var projectTask in project.ProjectTasks)
            {
                var taskRemainingMinutes = projectTask.MinutesCost * (1 - projectTask.PercentComplete);
                var projectData = new ProjectScheduleData
                {
                    ProjectTask = projectTask,
                    RemainingMinutes = taskRemainingMinutes,
                };
                ScheduleData.Add(projectData);
            }

            UserScheduleData.Clear();
            foreach (var projectUser in project.ProjectUsers)
            {
                var userData = new UserScheduleData
                {
                    ProjectUser = projectUser,
                };
                UserScheduleData.Add(userData);
            }
        }

        public void CalcSchedule()
        {
            SetupData(ViewModel.Project);
            _newRows.Clear();
            var date = ViewModel.StartDate;
            var remainingMinutes = ViewModel.RemainingMinutes;

            while (remainingMinutes > 0)
            {
                var holidayText = date.GetHolidayText();
                if (holidayText.IsNullOrEmpty())
                {
                    foreach (var projectUser in ViewModel.Project.ProjectUsers)
                    {
                        remainingMinutes = ProcessProjectUser(date, projectUser, remainingMinutes);
                    }
                }
                else
                {
                    var row = new ProjectScheduleGridRow(this);
                    row.Date = date;
                    row.Description = holidayText;
                    AddNewRow(row);
                }

                date = date.AddDays(1);
            }

            ClearRows();
            foreach (var newRow in _newRows.OrderBy(p => p.Date))
            {
                AddRow(newRow);
            }
        }

        private void AddNewRow(ProjectScheduleGridRow row)
        {
            _newRows.Add(row);
        }
        private decimal ProcessProjectUser(DateTime date, ProjectUser projectUser, decimal remainingMinutes)
        {
            var dailyRemainingMinutes = ViewModel.GetMinutesForDay(date, projectUser);
            if (dailyRemainingMinutes > 0)
            {
                var userData = UserScheduleData.FirstOrDefault(p => p.ProjectUser.UserId == projectUser.UserId);
                if (userData.NextScheduleDate != null && userData.NextScheduleDate > date)
                {
                    return remainingMinutes;
                }

                userData.NextScheduleDate = null;
                var endDate = date.AddDays(1).AddSeconds(-1);
                var timeOff = projectUser.User.UserTimeOff
                    .FirstOrDefault(p => p.StartDate.ToLocalTime().Ticks >= date.Ticks
                    && p.StartDate.ToLocalTime().Ticks <= endDate.Ticks);

                var minutesOff = (decimal)0;
                if (timeOff != null)
                {
                    var timeOffSpan = timeOff.EndDate - timeOff.StartDate;
                    
                    minutesOff = (decimal)timeOffSpan.TotalMinutes;

                    dailyRemainingMinutes -= minutesOff;

                    var rowsToAdd = (int)Math.Ceiling(timeOffSpan.TotalDays);
                    var newDate = date;
                    while (rowsToAdd > 0)
                    {
                        var row = new ProjectScheduleGridRow(this);
                        row.Date = newDate;
                        row.Description = $"{projectUser.User.Name} {timeOff.Description} Time Off";
                        if (dailyRemainingMinutes > 0)
                        {
                            row.Description += $" - {AppGlobals.MakeTimeSpent(minutesOff)}";
                        }
                        AddNewRow(row);
                        rowsToAdd--;
                        newDate = newDate.AddDays(1);
                        userData.NextScheduleDate = newDate;
                    }
                }

                if (dailyRemainingMinutes > 0)
                {
                    var originalDailyMinutesOff = dailyRemainingMinutes;
                    var userScheduleData = ScheduleData
                        .Where(p => p.ProjectTask.UserId == projectUser.UserId
                                    && p.RemainingMinutes > 0).OrderBy(p => p.Priority)
                        .ToList();
                    if (userScheduleData != null)
                    {
                        foreach (var scheduleData in userScheduleData)
                        {
                            var minutesWorked = scheduleData.RemainingMinutes;
                            if (scheduleData.RemainingMinutes - dailyRemainingMinutes < 0)
                            {
                                dailyRemainingMinutes -= scheduleData.RemainingMinutes;
                                remainingMinutes -= scheduleData.RemainingMinutes;
                                scheduleData.RemainingMinutes = 0;
                            }
                            else
                            {
                                scheduleData.RemainingMinutes -= dailyRemainingMinutes;
                                remainingMinutes -= dailyRemainingMinutes;
                                minutesWorked = dailyRemainingMinutes;
                            }

                            var row = new ProjectScheduleGridRow(this);
                            row.Date = date;
                            row.Description = $"{projectUser.User.Name} / {$"{scheduleData.ProjectTask.Name}"}";
                            row.HoursWorked = minutesWorked / 60;
                            AddNewRow(row);
                        }
                    }
                }
            }

            return remainingMinutes;
        }
    }
}
