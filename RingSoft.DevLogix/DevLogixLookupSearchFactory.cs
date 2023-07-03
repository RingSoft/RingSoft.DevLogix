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
            if (hostId == DevLogixLookupContext.MemoryHostId)
            {
                return new MemoryLookupSearchForHost();
            }

            return base.CreateSearchForHost(hostId);
        }

        public override string FormatValue(int hostId, string value)
        {
            if (hostId == DevLogixLookupContext.TimeSpentHostId)
            {
                return AppGlobals.MakeTimeSpent(value.ToDecimal().ToDouble());
            }

            if (hostId == DevLogixLookupContext.SpeedHostId)
            {
                return AppGlobals.MakeSpeed(value.ToDecimal().ToDouble());
            }
            if (hostId == DevLogixLookupContext.MemoryHostId)
            {
                return AppGlobals.MakeSpace(value.ToDecimal().ToDouble());
            }

            return base.FormatValue(hostId, value);
        }
    }
}
