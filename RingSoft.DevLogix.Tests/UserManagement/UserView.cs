using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Tests.UserManagement
{
    public class UserView : TestDbMaintenanceView, IUserView
    {
        public AppRights Rights { get; private set; } = new AppRights(new DevLogixRights());

        public TestAppProcedure RecalcProcedure { get; } = new TestAppProcedure();

        public UserMaintenanceViewModel LocalViewModel { get; set; }


        public string GetRights()
        {
            return Rights.UserRights.GetRightsString();
        }

        public void LoadRights(string rightsString)
        {
            Rights.UserRights.LoadRights(rightsString);
        }

        public void ResetRights()
        {
            Rights = new AppRights(new DevLogixRights());
        }

        public void RefreshView()
        {
            
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            return true;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupDefinition)
        {
            return LocalViewModel.StartRecalculateProcedure(lookupDefinition, RecalcProcedure);
        }

        public void UpdateRecalcProcedure(int currentProject, int totalProjects, string currentProjectText)
        {
            
        }

        public void SetUserReadOnlyMode(bool value)
        {
            
        }

        public void SetExistRecordFocus(UserGrids userGrid, int rowId)
        {
            
        }

        public string GetPassword()
        {
            return "12345";
        }

        public void SetPassword(string password)
        {
            
        }

        public void SetMasterUserMode(bool value = true)
        {
            
        }
    }
}
