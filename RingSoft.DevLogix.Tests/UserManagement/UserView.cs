using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Tests.UserManagement
{
    public class UserView : IUserView
    {
        public AppRights Rights { get; private set; } = new AppRights();

        public UserMaintenanceViewModel LocalViewModel { get; set; }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        public void ResetViewForNewRecord()
        {
            
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            return new List<DbAutoFillMap>();
        }


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
            Rights = new AppRights();
        }

        public void RefreshView()
        {
            
        }

        public void OnValGridFail(DataEntryGridManager dataEntryGridManager)
        {
            
        }

        public bool SetupRecalcFilter(LookupDefinitionBase lookupDefinition)
        {
            return true;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupDefinition)
        {
            return string.Empty;
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookupDefinition, AppProcedure procedure)
        {
            return LocalViewModel.StartRecalculateProcedure(lookupDefinition, procedure);
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
