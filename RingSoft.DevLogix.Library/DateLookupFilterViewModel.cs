using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DevLogix.Library
{
    public enum DateFilterFocusControls
    {
        Date = 0,
    }

    public interface IDateFilterView : IGenericReportFilterView
    {
        void FocusDateControl(DateFilterFocusControls focusControl);
    }
    public class DateFilterInput : GenericReportLookupFilterInput
    {
        public DateFieldDefinition DateFieldToFilter { get; set; }
    }
    public class DateLookupFilterViewModel : GenericReportFilterViewModel
    {
        private string _dateLabel;

        public string DateLabel
        {
            get => _dateLabel;
            set
            {
                if (_dateLabel == value)
                {
                    return;
                }
                _dateLabel = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;

                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value)
                    return;

                _endDate = value;
                OnPropertyChanged();
            }
        }

        public new DateFilterInput Input { get; private set; }

        public new IDateFilterView View { get; private set; }

        public new void Initialize(IDateFilterView view, DateFilterInput input)
        {
            View = view;
            Input = input;
            DateLabel = input.DateFieldToFilter.Description;
            base.Initialize(view, Input);
        }

        protected override void SetupStartEndLabels(GenericReportLookupFilterInput input)
        {
            BeginCodeLabel = input.CodeNameToFilter;
        }

        protected override bool AdditionalValidate()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                if (StartDate.Value > EndDate.Value)
                {
                    var message = "The starting date cannot be greater than the ending date.";
                    var caption = "Validation Failure";
                    View.FocusDateControl(DateFilterFocusControls.Date);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }
            return base.AdditionalValidate();
        }

        protected override void SetupFilter()
        {
            if (IsCurrentOnly)
            {
                return;
            }
            if (StartDate.HasValue)
            {
                var beginDate = StartDate.Value.ToUniversalTime();
                Input.LookupDefinitionToFilter.FilterDefinition.AddFixedFilter(
                    Input.DateFieldToFilter, Conditions.GreaterThanEquals, beginDate);
            }

            if (EndDate.HasValue)
            {
                var endDate = EndDate.Value.ToUniversalTime();
                Input.LookupDefinitionToFilter.FilterDefinition.AddFixedFilter(
                    Input.DateFieldToFilter, Conditions.LessThanEquals, endDate);
            }
            base.SetupFilter();
        }
    }
}
