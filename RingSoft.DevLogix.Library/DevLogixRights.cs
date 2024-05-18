using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.Library.ViewModels.ProjectManagement;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.Library
{
    public enum MenuCategories
    {
        UserManagement = 0,
        Tools = 1,
        Qa = 2,
        Projects = 3,
        Customers = 4,
    }

    public class DevLogixRights : ItemRights
    {
        public override void SetupRightsTree()
        {
            var category = new RightCategory("User Management", (int)MenuCategories.UserManagement);
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Users", AppGlobals.LookupContext.Users));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Groups", AppGlobals.LookupContext.Groups));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Departments", AppGlobals.LookupContext.Departments));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Time Clock Entries", AppGlobals.LookupContext.TimeClocks));
            category.Items.Add(new RightCategoryItem("Add/Edit User Trackers",
                AppGlobals.LookupContext.UserTracker));
            Categories.Add(category);

            category = new RightCategory("Quality Assurance", (int)MenuCategories.Qa);
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Error Statuses", AppGlobals.LookupContext.ErrorStatuses));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Error Priorities", AppGlobals.LookupContext.ErrorPriorities));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Errors", AppGlobals.LookupContext.Errors));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Products", AppGlobals.LookupContext.Products));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Product Versions", AppGlobals.LookupContext.ProductVersions));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Testing Templates", AppGlobals.LookupContext.TestingTemplates));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Testing Outlines", AppGlobals.LookupContext.TestingOutlines));

            Categories.Add(category);

            category = new RightCategory("Project Management", (int)MenuCategories.Projects);
            var categoryItem = new RightCategoryItem(item: "Add/Edit Projects", AppGlobals.LookupContext.Projects);
            category.Items.Add(categoryItem);

            categoryItem = new RightCategoryItem(item: "Add/Edit Project Tasks", AppGlobals.LookupContext.ProjectTasks);
            category.Items.Add(categoryItem);

            categoryItem = new RightCategoryItem(item: "Add/Edit Project Materials", AppGlobals.LookupContext.ProjectMaterials);
            category.Items.Add(categoryItem);

            AddSpecialRight((int)ProjectMaterialSpecialRights.AllowMaterialPost, "Allow Post Cost"
                , AppGlobals.LookupContext.ProjectMaterials);

            category.Items.Add(new RightCategoryItem(item: "Add/Edit Labor Parts", AppGlobals.LookupContext.LaborParts));

            category.Items.Add(new RightCategoryItem(item: "Add/Edit Material Parts", AppGlobals.LookupContext.MaterialParts));

            Categories.Add(category);

            category = new RightCategory("Customer Management", (int)MenuCategories.Customers);
            category.Items.Add(categoryItem = new RightCategoryItem(item: "Add/Edit Customers", AppGlobals.LookupContext.Customer));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Orders", AppGlobals.LookupContext.Order));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Time Zones", AppGlobals.LookupContext.TimeZone));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Territories", AppGlobals.LookupContext.Territory));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Customer Computers",
                AppGlobals.LookupContext.CustomerComputer));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Customer Statuses",
                AppGlobals.LookupContext.CustomerStatus));

            category.Items.Add(new RightCategoryItem(item: "Add/Edit Support Tickets", AppGlobals.LookupContext.SupportTicket));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Support Ticket Statuses",
                AppGlobals.LookupContext.SupportTicketStatus));


            Categories.Add(category);

            category = new RightCategory("Miscellaneous", (int)MenuCategories.Tools);
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Charts", AppGlobals.LookupContext.DevLogixCharts));
            category.Items.Add(new RightCategoryItem(item: "Add/Edit Advanced Finds", AppGlobals.LookupContext.AdvancedFinds));
            category.Items.Add(new RightCategoryItem(item: "View Record Locks", AppGlobals.LookupContext.RecordLocks));
            category.Items.Add(new RightCategoryItem(item: "View/Edit System Preferences", AppGlobals.LookupContext.SystemPreferences));
            Categories.Add(category);

        }
    }

    public class AppRights
    {
        public ItemRights UserRights { get; set; }

        public List<ItemRights> GroupRights { get; private set; }

        public AppRights()
        {
            UserRights = new DevLogixRights();

            GroupRights = new List<ItemRights>();
        }

        public bool HasRight(TableDefinitionBase tableDefinition, RightTypes rightType)
        {
            return UserRights.HasRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasRight(tableDefinition, rightType));
        }

        public bool HasSpecialRight(TableDefinitionBase tableDefinition, int rightType)
        {
            return UserRights.HasSpecialRight(tableDefinition, rightType) ||
                   GroupRights.Any(p => p.HasSpecialRight(tableDefinition, rightType));
        }
    }
}
