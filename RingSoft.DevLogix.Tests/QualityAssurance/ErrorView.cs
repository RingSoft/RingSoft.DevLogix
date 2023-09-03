using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Tests.QualityAssurance
{
    public class ErrorView : TestDbMaintenanceView, IErrorView
    {
        public void SetFocusAfterText(string text, bool descrioption, bool setFocus)
        {
            
        }

        public void CopyToClipboard(string text)
        {
            
        }

        public bool ProcessRecalcLookupFilter(LookupDefinitionBase lookup)
        {
            throw new NotImplementedException();
        }

        public string StartRecalcProcedure(LookupDefinitionBase lookup)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecalcProcedure(int currentError, int totalErrors, string currentErrorText)
        {
            
        }
    }
}
