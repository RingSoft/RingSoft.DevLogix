using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.Printing.Interop;

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

        public double RemainingMinutes { get; set; }

        public int Priority { get; set; }

        public override string ToString()
        {
            if (ProjectTask != null)
            {
                return ProjectTask.Name;
            }
            return base.ToString();
        }
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

            foreach (var projectScheduleData in ScheduleData)
            {
                if (projectScheduleData.ProjectTask.SourceDependencies.Any())
                {
                    foreach (var projectTaskDependency in projectScheduleData.ProjectTask.SourceDependencies)
                    {
                        var scheduleDependency = ScheduleData.FirstOrDefault(p =>
                            p.ProjectTask == projectTaskDependency.DependsOnProjectTask);
                        scheduleDependency.Priority++;
                        projectScheduleData.Priority = scheduleDependency.Priority + 1;
                    }
                }
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

            if (!ViewModel.Project.ProjectTasks.Any())
            {
                return;
            }

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
            
            remainingMinutes = ViewModel.RemainingMinutes;
            foreach (var newRow in _newRows.OrderBy(p => p.Date))
            {
                var rowMinsRemaining = remainingMinutes - (newRow.HoursWorked * 60);
                newRow.HoursRemaining = (rowMinsRemaining / 60);
                remainingMinutes = rowMinsRemaining;
                AddRow(newRow);
            }

            if (Rows.Any())
            {
                var lastRow = Rows.OfType<ProjectScheduleGridRow>().Last();
                ViewModel.CalculatedDeadline = lastRow.Date;
            }
        }

        private void AddNewRow(ProjectScheduleGridRow row)
        {
            _newRows.Add(row);
        }
        
        private double ProcessProjectUser(DateTime date, ProjectUser projectUser, double remainingMinutes)
        {
            var dailyRemainingMinutes = ViewModel.GetMinutesForDay(date, projectUser);

            var userData = UserScheduleData.FirstOrDefault(p => p.ProjectUser.UserId == projectUser.UserId);
            if (userData.NextScheduleDate != null && userData.NextScheduleDate > date)
            {
                return remainingMinutes;
            }

            userData.NextScheduleDate = null;

            dailyRemainingMinutes = ProcessTimeOff(date, projectUser, dailyRemainingMinutes, userData);

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
                        if (!ValDependencies(scheduleData.ProjectTask))
                        {
                            continue;
                        }

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
                            dailyRemainingMinutes = 0;
                        }

                        var row = new ProjectScheduleGridRow(this);
                        row.Date = date;
                        row.Description = $"{projectUser.User.Name} / {$"{scheduleData.ProjectTask.Name}"}";
                        row.HoursWorked = minutesWorked / 60;
                        AddNewRow(row);

                        if (dailyRemainingMinutes <= 0)
                        {
                            return remainingMinutes;
                        }
                    }
                }
            }

            return remainingMinutes;
        }

        private bool ValDependencies(ProjectTask projectTask)
        {
            if (projectTask.SourceDependencies.Any())
            {
                var dependsProject = projectTask.SourceDependencies.FirstOrDefault(p =>
                    p.DependsOnProjectTask != projectTask);

                if (dependsProject != null)
                {
                    var userProjectData = ScheduleData.FirstOrDefault(p =>
                        p.ProjectTask == dependsProject.DependsOnProjectTask && p.RemainingMinutes > 0);
                    if (userProjectData != null)
                    {
                        return false;
                    }
                    else
                    {
                        return ValDependencies(dependsProject.DependsOnProjectTask);
                    }
                }
            }

            return true;
        }

        private double ProcessTimeOff(DateTime date, ProjectUser projectUser, double dailyRemainingMinutes,
            UserScheduleData userData)
        {
            var endDate = date.AddDays(1).AddSeconds(-1);
            var timeOff = projectUser.User.UserTimeOff
                .FirstOrDefault(p => p.StartDate.ToLocalTime().Ticks >= date.Ticks
                                     && p.StartDate.ToLocalTime().Ticks <= endDate.Ticks);

            var minutesOff = (double)0;
            if (timeOff != null)
            {
                var timeOffSpan = timeOff.EndDate - timeOff.StartDate;

                minutesOff = (double)timeOffSpan.TotalMinutes;

                dailyRemainingMinutes -= minutesOff;

                var rowsToAdd = (int)Math.Ceiling(timeOffSpan.TotalDays);
                var newDate = date;
                while (rowsToAdd > 0)
                {
                    var todaysWorkingMinutes = ViewModel.GetMinutesForDay(newDate, projectUser);
                    if (todaysWorkingMinutes > 0)
                    {
                        var row = new ProjectScheduleGridRow(this);
                        row.Date = newDate;
                        row.Description = $"{projectUser.User.Name} {timeOff.Description} Time Off";
                        if (dailyRemainingMinutes > 0)
                        {
                            row.Description += $" - {AppGlobals.MakeTimeSpent(minutesOff)}";
                        }

                        AddNewRow(row);
                    }

                    rowsToAdd--;
                    newDate = newDate.AddDays(1);
                    userData.NextScheduleDate = newDate;
                }
            }

            return dailyRemainingMinutes;
        }

        public void PrintDetails(PrinterSetupArgs setupArgs, PrintingInputHeaderRow headerRow)
        {
            var detailsChunk = new List<PrintingInputDetailsRow>();
            var rows = Rows.OfType<ProjectScheduleGridRow>();
            var counter = 0;
            var numberSetup = new DecimalEditControlSetup
            {
                FormatType = DecimalEditFormatTypes.Number,
            };

            foreach (var projectScheduleGridRow in rows)
            {
                var detail = new PrintingInputDetailsRow();
                detail.HeaderRowKey = headerRow.RowKey;
                detail.TablelId = 1;

                detail.StringField01 = projectScheduleGridRow.Date.FormatDateValue(DbDateTypes.DateOnly);
                detail.StringField02 = projectScheduleGridRow.Description;
                detail.NumberField01 = numberSetup.FormatValue(projectScheduleGridRow.HoursWorked);
                detail.NumberField02 = numberSetup.FormatValue(projectScheduleGridRow.HoursRemaining);

                detailsChunk.Add(detail);
                counter++;
                if (counter % 10 == 0)
                {
                    PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, setupArgs.PrintingProperties);
                    detailsChunk.Clear();
                }
            }

            if (detailsChunk.Any())
            {
                PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, setupArgs.PrintingProperties);
                detailsChunk.Clear();
            }
        }
    }
}
