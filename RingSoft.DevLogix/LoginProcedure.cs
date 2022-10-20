using System.Windows;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.MasterData;

namespace RingSoft.DevLogix
{
    public class LoginProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        private ProcessingSplashWindow _splashWindow;
        private Organization _organization;

        public LoginProcedure(Organization organization)
        {
            _organization = organization;
        }

        protected override void ShowSplash()
        {
            _splashWindow = new ProcessingSplashWindow("Logging In");
            _splashWindow.ShowDialog();
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            var result = AppGlobals.LoginToOrganization(_organization);
            CloseSplash();
            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            if (!result.IsNullOrEmpty())
            {
                var caption = "File access failure";
                MessageBox.Show(result, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result.IsNullOrEmpty();
        }

        private void AppGlobals_AppSplashProgress(object sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
