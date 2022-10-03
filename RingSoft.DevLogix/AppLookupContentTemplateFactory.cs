using System;
using System.Windows.Controls;
using System.Windows;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DevLogix
{
    public class AppLookupContentTemplateFactory : LookupControlContentTemplateFactory
    {
        private Application _application;

        public AppLookupContentTemplateFactory(Application application)
        {
            _application = application;
        }

        public override Image GetImageForAlertLevel(AlertLevels alertLevel)
        {

            Image result = null;
            switch (alertLevel)
            {
                case AlertLevels.Green:
                    result = _application.Resources["GreenAlertImage"] as Image;
                    return result;
                case AlertLevels.Yellow:
                    result = _application.Resources["YellowAlertImage"] as Image;
                    return result;
                case AlertLevels.Red:
                    result = _application.Resources["RedAlertImage"] as Image;
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alertLevel), alertLevel, null);
            }
            return base.GetImageForAlertLevel(alertLevel);

        }
    }
}
