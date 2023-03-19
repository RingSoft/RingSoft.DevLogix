using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.DataAccess
{
    public interface IDevLogixDbContext : IAdvancedFindDbContextEfCore, IDbContext
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

        DbSet<ErrorStatus> ErrorStatuses { get; set; }
        DbSet<ErrorPriority> ErrorPriorities { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductVersion> ProductVersions { get; set; }
        DbSet<ProductVersionDepartment> ProductVersionDepartments { get; set; }
        DbSet<Error> Errors { get; set; }
        DbSet<ErrorDeveloper> ErrorDevelopers { get; set; }
        DbSet<ErrorQa> ErrorTesters { get; set; }
        DbSet<ErrorUser> ErrorUsers { get; set; }

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

        void SetLookupContext(DevLogixLookupContext lookupContext);
    }
}
