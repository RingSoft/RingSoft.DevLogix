﻿using System.Windows;
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
            set => Control.Minutes = value.ToDecimal();
        }

        public override void SelectAll()
        {
        }

        public override void SetValue(string value)
        {
            Control.Minutes = value.ToDecimal();
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
            set => Control.Speed = value.ToDecimal();
        }


        public override void SelectAll()
        {
        }

        public override void SetValue(string value)
        {
            Control.Speed = value.ToDecimal();
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

    public class MemoryLookupSearchForHost : LookupSearchForHost<MemoryControl>
    {
        public override string SearchText
        {
            get => Control.Memory.ToString();
            set => Control.Memory = value.ToDecimal();
        }


        public override void SelectAll()
        {
        }

        public override void SetValue(string value)
        {
            Control.Memory = value.ToDecimal();
        }

        protected virtual double? DefaultWidth { get; set; } = 100;

        protected override MemoryControl ConstructControl()
        {
            return new MemoryControl();
        }

        protected override void Initialize(MemoryControl control, LookupColumnDefinitionBase columnDefinition)
        {
            control.Width = DefaultWidth.Value;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.ControlDirty += (sender, args) => OnTextChanged();
        }

        protected override void Initialize(MemoryControl control)
        {

        }
    }

}
