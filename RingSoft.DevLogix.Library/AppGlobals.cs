using System;
using System.Threading;
using RingSoft.App.Library;

namespace RingSoft.DevLogix.Library
{
    public class AppProgressArgs
    {
        public string ProgressText { get; }

        public AppProgressArgs(string progressText)
        {
            ProgressText = progressText;
        }
    }
    public class AppGlobals
    {
        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "DevLogix";
            RingSoftAppGlobals.AppCopyright = "©2022 by Peter Ringering";
            RingSoftAppGlobals.AppVersion = "0.80.00";
        }

        public static void Initialize()
        {
            Thread.Sleep(3000);
        }

    }
}
