using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IProjectMaterialPostView
    {
        void CloseWindow();
    }
    public class ProjectMaterialPostViewModel : INotifyPropertyChanged
    {
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
                    return;

                _userAutoFillValue = value;
                OnPropertyChanged();
            }
        }
        private AutoFillSetup _projectAutoFillSetup;

        public AutoFillSetup ProjectAutoFillSetup
        {
            get => _projectAutoFillSetup;
            set
            {
                if (_projectAutoFillSetup == value)
                {
                    return;
                }
                _projectAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _projectAutoFillValue;

        public AutoFillValue ProjectAutoFillValue
        {
            get => _projectAutoFillValue;
            set
            {
                if (_projectAutoFillValue == value)
                    return;

                _projectAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private ProjectMaterialPostManager _materialPostManager;

        public ProjectMaterialPostManager MaterialPostManager
        {
            get => _materialPostManager;
            set
            {
                if (_materialPostManager == value)
                    return;

                _materialPostManager = value;
                OnPropertyChanged();
            }
        }

        public IProjectMaterialPostView View { get; private set; }

        public bool DialogResult { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; } 

        public ProjectMaterialPostViewModel()
        {
            UserAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.UserLookup);
            ProjectAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.ProjectLookup);
            
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }
        public void Initialize(Project project, IProjectMaterialPostView view)
        {
            View = view;
            var context = AppGlobals.DataRepository.GetDataContext();
            project = context.GetTable<Project>().FirstOrDefault(p => p.Id == project.Id);
            UserAutoFillValue = AppGlobals.LoggedInUser.GetAutoFillValue();
            ProjectAutoFillValue = project.GetAutoFillValue();
            MaterialPostManager = new ProjectMaterialPostManager(this);
        }

        private void OnOk()
        {
            DialogResult = true;
            View.CloseWindow();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
