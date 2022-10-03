using System.Windows;

namespace RingSoft.DevLogix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var appStart = new DevLogixAppStart(this);
            appStart.Start();

            base.OnStartup(e);
        }

    }
}
