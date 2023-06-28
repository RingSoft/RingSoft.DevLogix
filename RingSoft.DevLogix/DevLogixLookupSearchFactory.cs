using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DevLogix.DataAccess;
using RingSoft.DevLogix.Library;

namespace RingSoft.DevLogix
{
    public class DevLogixLookupSearchFactory : LookupSearchForHostFactory
    {
        protected override LookupSearchForHost CreateSearchForHost(int? hostId)
        {
            if (hostId == DevLogixLookupContext.TimeSpentHostId)
            {
                return new TimeSpentLookupSearchForHost();
            }
            if (hostId == DevLogixLookupContext.SpeedHostId)
            {
                return new SpeedLookupSearchForHost();
            }

            return base.CreateSearchForHost(hostId);
        }

        public override string FormatValue(int hostId, string value)
        {
            if (hostId == DevLogixLookupContext.TimeSpentHostId)
            {
                return AppGlobals.MakeTimeSpent(value.ToDecimal().ToDouble());
            }
            return base.FormatValue(hostId, value);
        }
    }
}
