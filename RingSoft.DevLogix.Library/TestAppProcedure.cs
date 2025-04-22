using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DevLogix.Library
{
    public class TestAppProcedure : AppProcedure, ISplashWindow
    {
        protected override void ShowSplash()
        {
            
        }

        protected override bool DoProcess()
        {
            return true;
        }

        public override ISplashWindow SplashWindow => this;

        public void InitAndShow(AppProcedure2 procedure)
        {
            throw new System.NotImplementedException();
        }

        public void SetProgress(string progressText)
        {
            
        }

        public void ShowError(string message, string title)
        {
            
        }

        public void ShowMessageBox(string message, string title, RsMessageBoxIcons icon)
        {
            
        }

        public bool ShowYesNoMessageBox(string message, string caption)
        {
            throw new System.NotImplementedException();
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string message, string caption)
        {
            throw new System.NotImplementedException();
        }

        public void CloseSplash()
        {
            
        }

        public void SetWindowCursor(WindowCursorTypes cursorType)
        {
            
        }

        public bool IsDisposed => true;
        public bool Disposing => true;
    }
}
