using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.Tests.UserManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RingSoft.DevLogix.Tests
{
    public class TestMainView : IMainView
    {
        private TimeClockMaintenanceViewModel _timeClockViewModel;
        public bool ChangeOrganization()
        {
            return true;
        }

        public bool LoginUser(int userId = 0)
        {
            return true;
        }

        public void CloseWindow()
        {
        }

        public void ShowDbMaintenanceWindow(TableDefinitionBase tableDefinition)
        {
        }

        public void ShowMaintenanceTab(TableDefinitionBase tableDefinition)
        {
            
        }

        public void ShowDbMaintenanceDialog(TableDefinitionBase tableDefinition)
        {
        }

        public void ShowAdvancedFindWindow()
        {
        }

        public void MakeMenu()
        {
        }

        private void GetNewTimeClockViewModel()
        {
            _timeClockViewModel = new TimeClockMaintenanceViewModel();
            _timeClockViewModel.Processor = new TestDbMaintenanceProcessor();
            var view = new TestTimeClockView();
            _timeClockViewModel.OnViewLoaded(view);
        }

        public void PunchIn(RingSoft.DevLogix.DataAccess.Model.Error error)
        {
            GetNewTimeClockViewModel();
            _timeClockViewModel.PunchIn(error);
        }

        public void PunchIn(ProjectTask projectTask)
        {
            GetNewTimeClockViewModel();
            _timeClockViewModel.PunchIn(projectTask);
        }

        public void PunchIn(TestingOutline testingOutline)
        {
            GetNewTimeClockViewModel();
            _timeClockViewModel.PunchIn(testingOutline);
        }

        public void PunchIn(Customer customer)
        {
            GetNewTimeClockViewModel();
            _timeClockViewModel.PunchIn(customer);
        }

        public void PunchIn(SupportTicket ticket)
        {
            GetNewTimeClockViewModel();
            _timeClockViewModel.PunchIn(ticket);
        }

        public void ShowChart(DevLogixChart chart)
        {
            
        }

        public object GetOwnerWindow()
        {
            return null;
        }

        public void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack)
        {
            
        }

        public void SetElapsedTime()
        {
            
        }

        public void ShowTimeClock(bool show = true)
        {
            
        }

        public void LaunchTimeClock(TimeClock activeTimeCard)
        {
            
        }

        public UserClockReasonViewModel GetUserClockReason()
        {
            var result = new UserClockReasonViewModel();
            result.ClockOutReason = ClockOutReasons.GoneHome;
            return result;
        }

        public bool UpgradeVersion()
        {
            return true;
        }

        public void ShowAbout()
        {
            
        }

        public void RepairDates()
        {
            
        }

        public bool CloseAllTabs()
        {
            return true;
        }
    }
}
