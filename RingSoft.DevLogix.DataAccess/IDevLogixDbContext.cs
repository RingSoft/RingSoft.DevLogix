﻿using Microsoft.EntityFrameworkCore;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDevLogixDbContext : IDbContext, DbLookup.IDbContext
    {
        DbContext DbContext { get; }

        DbSet<SystemMaster> SystemMaster { get; set; }
        DbSet<DevLogixChart> DevLogixCharts { get; set; }
        DbSet<DevLogixChartBar> DevLogixChartsBars { get; set;}
        DbSet<SystemPreferences> SystemPreferences { get; set; }
        DbSet<SystemPreferencesHolidays> SystemPreferencesHolidays { get; set; }

        DbSet<User> Users { get; set; }
        DbSet<UserTimeOff> UsersTimeOff { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<UsersGroup> UsersGroups { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<TimeClock> TimeClocks { get; set; }
        DbSet<UserTracker>  UserTracker { get; set; }
        DbSet<UserTrackerUser> UserTrackerUsers { get; set; }
        DbSet<UserMonthlySales> UserMonthlySales { get; set; }

        DbSet<ErrorStatus> ErrorStatuses { get; set; }
        DbSet<ErrorPriority> ErrorPriorities { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductVersion> ProductVersions { get; set; }
        DbSet<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        DbSet<Error> Errors { get; set; }
        DbSet<ErrorDeveloper> ErrorDevelopers { get; set; }
        DbSet<ErrorQa> ErrorTesters { get; set; }
        DbSet<ErrorUser> ErrorUsers { get; set; }
        DbSet<TestingTemplate> TestingTemplates { get; set; }
        DbSet<TestingTemplateItem> TestingTemplateItems { get; set; }
        DbSet<TestingOutline> TestingOutlines { get; set; }
        DbSet<TestingOutlineDetails> TestingOutlineDetails { get; set; }
        DbSet<TestingOutlineTemplate> TestingOutlineTemplates { get; set; }
        DbSet<TestingOutlineCost> TestingOutlineCosts { get; set; }

        DbSet<Project> Projects { get; set; }
        DbSet<ProjectUser> ProjectUsers { get; set; }
        DbSet<LaborPart> LaborParts { get; set; }
        DbSet<ProjectTask> ProjectTasks { get; set; }
        DbSet<ProjectTaskLaborPart> ProjectTaskLaborParts { get; set; }
        DbSet<MaterialPart> MaterialParts { get; set; }
        DbSet<ProjectMaterial> ProjectMaterials { get; set; }
        DbSet<ProjectMaterialPart> ProjectMaterialParts { get; set; }
        DbSet<ProjectMaterialHistory> ProjectMaterialHistory { get; set; }
        DbSet<ProjectTaskDependency> ProjectTaskDependency { get; set; }

        DbSet<TimeZone> TimeZone { get; set; }
        DbSet<Territory> Territory { get; set; }
        DbSet<Customer> Customer { get; set; }
        DbSet<CustomerProduct> CustomerProduct { get; set; }
        DbSet<Order> Order { get; set; }
        DbSet<OrderDetail> OrderDetail { get; set; }
        DbSet<CustomerComputer> CustomerComputer { get; set; }
        DbSet<SupportTicket> SupportTicket { get; set; }
        DbSet<SupportTicketUser > SupportTicketUser { get; set; }
        DbSet<CustomerUser> CustomerUser { get; set; }
        DbSet<SupportTicketError> SupportTicketError { get; set; }
        DbSet<CustomerStatus> CustomerStatus { get; set; }
        DbSet<SupportTicketStatus> SupportTicketStatus { get; set; }

        void SetLookupContext(DevLogixLookupContext lookupContext);
    }
}
