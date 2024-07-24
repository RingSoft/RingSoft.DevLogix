using RingSoft.DbLookup;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;

namespace RingSoft.DevLogix.Tests.UserManagement
{
    public class TestTimeClockView : ITimeClockView
    {
        public double PunchOutMinutes { get; set; } = 0;
        public void ResetViewForNewRecord()
        {
            
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            
        }

        public void SetTimeClockMode(TimeClockModes timeClockMode)
        {
            
        }

        public void SetElapsedTime()
        {
            
        }

        public void FocusNotes()
        {
            
        }

        public void SetDialogMode()
        {
            
        }

        public bool GetManualPunchOutDate(out DateTime? punchInDate, out DateTime? punchOutDate)
        {
            punchOutDate = GblMethods.NowDate();
            punchInDate = punchOutDate.Value.AddMinutes(-PunchOutMinutes);
            return true;
        }
    }
}
