using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class ProjectScheduleGridManager : DataEntryGridManager
    {
        public const int DateColumnId = (int)ProjectScheduleColumns.Date;
        public const int DescriptionColumnId = (int)ProjectScheduleColumns.Description;
        public const int HoursWorkedColumnId = (int)ProjectScheduleColumns.HoursWorked;
        public const int HoursRemainingColumnId = (int)ProjectScheduleColumns.HoursRemaining;

        public ProjectScheduleViewModel ViewModel { get; private set; }

        public List<ProjectScheduleData> ScheduleData { get; private set; } = new List<ProjectScheduleData>();

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
        }

        public void CalcSchedule()
        {
            SetupData(ViewModel.Project);
            ClearRows(false);
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
                    AddRow(row);
                }

                date = date.AddDays(1);
            }
        }

        private decimal ProcessProjectUser(DateTime date, ProjectUser projectUser, decimal remainingMinutes)
        {
            var dailyRemainingMinutes = ViewModel.GetMinutesForDay(date, projectUser);
            if (dailyRemainingMinutes > 0)
            {
                var endDate = date.AddDays(1).AddSeconds(-1);
                var timeOff = projectUser.User.UserTimeOff.FirstOrDefault(p => p.StartDate.ToLocalTime() >= date);
                if (timeOff != null)
                {
                    var timeOffSpan = timeOff.EndDate - timeOff.StartDate;
                    var minutesOff = (decimal)timeOffSpan.TotalMinutes;
                    if (minutesOff > dailyRemainingMinutes)
                    {
                        
                    }
                    dailyRemainingMinutes -= minutesOff;
                    var row = new ProjectScheduleGridRow(this);
                    row.Date = date;
                    row.Description = $"{projectUser.User.Name} {timeOff.Description} Time Off";
                    AddRow(row);
                }

                if (dailyRemainingMinutes > 0)
                {
                    var originalDailyMinutesOff = dailyRemainingMinutes;
                    var userScheduleData = ScheduleData
                        .Where(p => p.ProjectTask.UserId == projectUser.UserId
                                    && p.RemainingMinutes > 0)
                        .ToList();
                    if (userScheduleData != null)
                    {
                        foreach (var scheduleData in userScheduleData)
                        {
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
                            }

                            var row = new ProjectScheduleGridRow(this);
                            row.Date = date;
                            row.Description = $"{projectUser.User.Name} / {$"{scheduleData.ProjectTask.Name}"}";
                            row.HoursWorked = originalDailyMinutesOff / 60;
                            AddRow(row);
                        }
                    }
                }
            }

            return remainingMinutes;
        }
    }
}
