using RingSoft.App.Controls;
using RingSoft.App.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix
{
    public class RecalcProcedure : AppProcedure
    {
        public event EventHandler StartRecalculate;

        private ProcessingSplashWindow _splashWindow;

        protected override void ShowSplash()
        {
            _splashWindow = new ProcessingSplashWindow("Recalculating");
            _splashWindow.ShowDialog();
        }

        protected override bool DoProcess()
        {
            StartRecalculate?.Invoke(this, EventArgs.Empty);
            _splashWindow.CloseSplash();

            return true;
        }

        public override ISplashWindow SplashWindow => _splashWindow;

    }
}
