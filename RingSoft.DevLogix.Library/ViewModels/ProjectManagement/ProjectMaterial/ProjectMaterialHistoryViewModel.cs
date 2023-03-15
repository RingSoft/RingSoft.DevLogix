using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DevLogix.DataAccess.LookupModel.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class ProjectMaterialHistoryViewModel : DevLogixDbMaintenanceViewModel<ProjectMaterialHistory>
    {
        public override TableDefinition<ProjectMaterialHistory> TableDefinition =>
            AppGlobals.LookupContext.ProjectMaterialHistory;

        public override bool AllowEdit => false;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value)
                {
                    return;
                }

                _date = value;
                OnPropertyChanged();
            }
        }


        private AutoFillSetup _projectMaterialAutoFillSetup;

        public AutoFillSetup ProjectMaterialAutoFillSetup
        {
            get => _projectMaterialAutoFillSetup;
            set
            {
                if (_projectMaterialAutoFillSetup == value)
                {
                    return;
                }

                _projectMaterialAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectMaterialAutoFillValue;

        public AutoFillValue ProjectMaterialAutoFillValue
        {
            get => _projectMaterialAutoFillValue;
            set
            {
                if (_projectMaterialAutoFillValue == value)
                {
                    return;
                }

                _projectMaterialAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _userAutoFillSetup;

        public AutoFillSetup UserAutoFillSetup
        {
            get => _userAutoFillSetup;
            set
            {
                if (_userAutoFillSetup == value)
                {
                    return;
                }

                _userAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _userAutoFillValue;

        public AutoFillValue UserAutoFillValue
        {
            get => _userAutoFillValue;
            set
            {
                if (_userAutoFillValue == value)
                {
                    return;
                }

                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _quantity;

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                {
                    return;
                }

                _quantity = value;
                OnPropertyChanged();
            }
        }

        private decimal _cost;

        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost == value)
                {
                    return;
                }

                _cost = value;
                OnPropertyChanged();
            }
        }

        private decimal _extendedCost;

        public decimal ExtendedCost
        {
            get => _extendedCost;
            set
            {
                if (_extendedCost == value)
                {
                    return;
                }

                _extendedCost = value;
                OnPropertyChanged();
            }
        }


        public ProjectMaterialHistoryViewModel()
        {
            ProjectMaterialAutoFillSetup =
                new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ProjectMaterialId));

            UserAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.UserId));
        }

        protected override ProjectMaterialHistory PopulatePrimaryKeyControls(ProjectMaterialHistory newEntity,
            PrimaryKeyValue primaryKeyValue)
        {
            var table = AppGlobals.DataRepository.GetDataContext().GetTable<ProjectMaterialHistory>();
            var history = table
                .Include(p => p.User)
                .Include(p => p.ProjectMaterial)
                .FirstOrDefault(p => p.Id == newEntity.Id);

            Id = history.Id;

            return history;
        }

        protected override void LoadFromEntity(ProjectMaterialHistory entity)
        {
            Date = entity.Date.ToLocalTime();
            UserAutoFillValue = entity.User.GetAutoFillValue();
            ProjectMaterialAutoFillValue = entity.ProjectMaterial.GetAutoFillValue();
            Quantity = entity.Quantity;
            Cost = entity.Cost;
            ExtendedCost = Quantity * Cost;
        }

        protected override ProjectMaterialHistory GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
        }

        protected override bool SaveEntity(ProjectMaterialHistory entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }

        protected override void PrintOutput()
        {
            var printerSetup = CreatePrinterSetupArgs();
            printerSetup.LookupDefinition.InitialOrderByType = OrderByTypes.Ascending;

            var callBack = new HistoryPrintFilterCallBack();
            callBack.FilterDate = Date;
            var fieldFilters =
                printerSetup.LookupDefinition.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>();
            var filters = new List<FieldFilterDefinition>(fieldFilters);

            callBack.PrintOutput += (sender, model) =>
            {
                if (printerSetup.LookupDefinition is
                    LookupDefinition<ProjectMaterialHistoryLookup, ProjectMaterialHistory> historyLookup)
                {
                    historyLookup.FilterDefinition.ClearFixedFilters();
                    foreach (var fieldFilter in filters)
                    {
                        historyLookup.FilterDefinition.AddFixedFieldFilter(fieldFilter.FieldDefinition, fieldFilter.Condition,
                            fieldFilter.Value);
                    }
                    if (model.BeginningDate.HasValue)
                    {
                        var beginDate = model.BeginningDate.Value;
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.Date,
                            Conditions.GreaterThanEquals,
                            beginDate);
                    }

                    if (model.EndingDate.HasValue)
                    {
                        var endDate = model.EndingDate.Value;
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.Date,
                            Conditions.LessThanEquals,
                            endDate);
                    }
                }

                Processor.PrintOutput(printerSetup);
            };

            AppGlobals.MainViewModel.MainView.ShowHistoryPrintFilterWindow(callBack);
        }
    }
}
