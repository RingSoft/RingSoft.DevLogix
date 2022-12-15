using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public enum ValidationFailControls
    {
        Existing = 0,
        New = 1,
        Version = 2,
    }
    public interface IProductUpdateVersionsView
    {
        void CloseWindow();

        void ValidationFailed(ValidationFailControls validationFailControls, string message, string caption);
    }
    public class ProductUpdateVersionsViewModel : INotifyPropertyChanged
    {
        private AutoFillSetup _existingDepartmentSetup;

        public AutoFillSetup ExistingDepartmentSetup
        {
            get => _existingDepartmentSetup;
            set
            {
                if (_existingDepartmentSetup == value)
                {
                    return;
                }
                _existingDepartmentSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _existingDepartmentAutoFillValue;

        public AutoFillValue ExistingDepartmentAutoFillValue
        {
            get => _existingDepartmentAutoFillValue;
            set
            {
                if (_existingDepartmentAutoFillValue == value)
                    return;

                _existingDepartmentAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _newDepartmentSetup;

        public AutoFillSetup NewDepartmentSetup
        {
            get => _newDepartmentSetup;
            set
            {
                if (_newDepartmentSetup == value)
                {
                    return;
                }
                _newDepartmentSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _newDepartmentAutoFillValue;

        public AutoFillValue NewDepartmentAutoFillValue
        {
            get => _newDepartmentAutoFillValue;
            set
            {
                if (_newDepartmentAutoFillValue == value)
                    return;

                _newDepartmentAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _updateVersionSetup;

        public AutoFillSetup UpdateVersionSetup
        {
            get => _updateVersionSetup;
            set
            {
                if (_updateVersionSetup == value)
                {
                    return;
                }
                _updateVersionSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _updateVersionAutoFillValue;

        public AutoFillValue UpdateVersionAutoFillValue
        {
            get => _updateVersionAutoFillValue;
            set
            {
                if (_updateVersionAutoFillValue == value)
                    return;

                _updateVersionAutoFillValue = value;
                OnPropertyChanged();
            }
        }


        public ProductViewModel ProductViewModel { get; private set; }

        public IProductUpdateVersionsView View { get; private set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public bool DialogResult { get; private set; }

        public ProductUpdateVersionsViewModel()
        {
            ExistingDepartmentSetup = new AutoFillSetup(AppGlobals.LookupContext.DepartmentLookup.Clone());
            ExistingDepartmentSetup.AllowLookupAdd = false;

            NewDepartmentSetup = new AutoFillSetup(AppGlobals.LookupContext.DepartmentLookup.Clone());

            UpdateVersionSetup = new AutoFillSetup(AppGlobals.LookupContext.ProductVersionLookup.Clone());

            OkCommand = new RelayCommand(OnOK);

            CancelCommand = new RelayCommand(OnCancel);
        }

        public void Initialize(IProductUpdateVersionsView view, ProductViewModel productViewModel)
        {
            View = view;
            ProductViewModel = productViewModel;
        }

        private void OnCancel()
        {
            View.CloseWindow();
        }

        private void OnOK()
        {
            if (!Validate())
            {
                return;
            }

            var existingDepartment =
                AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(ExistingDepartmentAutoFillValue
                    .PrimaryKeyValue);

            var newDepartment =
                AppGlobals.LookupContext.Departments.GetEntityFromPrimaryKeyValue(NewDepartmentAutoFillValue
                    .PrimaryKeyValue);

            var version =
                AppGlobals.LookupContext.ProductVersions.GetEntityFromPrimaryKeyValue(UpdateVersionAutoFillValue
                    .PrimaryKeyValue);

            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<ProductVersionDepartment>();
            var productDepartment =
                query.FirstOrDefault(p => p.VersionId == version.Id && p.DepartmentId == existingDepartment.Id);

            var versionQuery = context.GetTable<ProductVersion>()
                .Include(p => p.ProductVersionDepartments);

            var newVersions = versionQuery.Where(p =>
                p.ProductId == ProductViewModel.Id &&
                p.ProductVersionDepartments.Any(
                    p => p.ReleaseDateTime <= productDepartment.ReleaseDateTime &&
                         p.DepartmentId == existingDepartment.Id &&
                         p.DepartmentId != newDepartment.Id) &&
                !p.ProductVersionDepartments.Any(
                    p => p.ReleaseDateTime <= productDepartment.ReleaseDateTime &&
                         p.DepartmentId == newDepartment.Id));
        }

        private bool Validate()
        {
            var caption = "Validation Failed";
            if (!ExistingDepartmentAutoFillValue.IsValid())
            {
                var message = "You must select a valid existing department.";
                View.ValidationFailed(ValidationFailControls.Existing, message, caption);
                return false;
            }

            if (!NewDepartmentAutoFillValue.IsValid())
            {
                var message = "You must select a valid new department.";
                View.ValidationFailed(ValidationFailControls.New, message, caption);
                return false;
            }

            if (!UpdateVersionAutoFillValue.IsValid())
            {
                var message = "You must select a valid version.";
                View.ValidationFailed(ValidationFailControls.Version, message, caption);
                return false;
            }

            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
