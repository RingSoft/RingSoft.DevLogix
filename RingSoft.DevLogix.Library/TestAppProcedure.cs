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

        public void SetProgress(string progressText)
        {
            
        }

        public void ShowError(string message, string title)
        {
            
        }

        public void ShowMessageBox(string message, string title, RsMessageBoxIcons icon)
        {
            
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
