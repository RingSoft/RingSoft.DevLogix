using System.Windows;
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

    }
}
