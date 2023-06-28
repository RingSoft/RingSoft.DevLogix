using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DevLogix
{
    public class TimeSpentLookupSearchForHost : LookupSearchForHost<TimeControl>
    {
        public override string SearchText
        {
            get => Control.Minutes.ToString();
            set => Control.Minutes = value.ToDecimal().ToDouble();
        }

        public override void SelectAll()
        {
        }

        protected virtual double? DefaultWidth { get; set; } = 100;

        protected override TimeControl ConstructControl()
        {
            return new TimeControl();
        }

        protected override void Initialize(TimeControl control, LookupColumnDefinitionBase columnDefinition)
        {
            control.Width = DefaultWidth.Value;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.ControlDirty += (sender, args) => OnTextChanged();
        }

        protected override void Initialize(TimeControl control)
        {
            
        }
    }

    public class SpeedLookupSearchForHost : LookupSearchForHost<SpeedControl>
    {
        public override string SearchText
        {
            get => Control.Speed.ToString();
            set => Control.Speed = value.ToDecimal().ToDouble();
        }


        public override void SelectAll()
        {
        }

        protected virtual double? DefaultWidth { get; set; } = 100;

        protected override SpeedControl ConstructControl()
        {
            return new SpeedControl();
        }

        protected override void Initialize(SpeedControl control, LookupColumnDefinitionBase columnDefinition)
        {
            control.Width = DefaultWidth.Value;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.ControlDirty += (sender, args) => OnTextChanged();
        }

        protected override void Initialize(SpeedControl control)
        {

        }
    }
}
