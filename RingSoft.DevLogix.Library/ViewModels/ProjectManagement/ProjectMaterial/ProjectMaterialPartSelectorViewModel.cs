using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface IMaterialPartView
    {
        void CloseWindow();
    }

    public class ProjectMaterialPartSelectorViewModel : INotifyPropertyChanged
    {
        private string _labelText;

        public string LabelText
        {
            get => _labelText;
            set
            {
                if (_labelText == value)
                {
                    return;
                }
                _labelText = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddNewMaterialPartCommand { get; private set; }

        public RelayCommand AddNewMiscRowCommand { get; private set; }

        public RelayCommand AddNewOverheadRowCommand { get; private set; }

        public RelayCommand AddCommentRowCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public PrimaryKeyValue NewMaterialPartPkValue { get; private set; }

        public MaterialPartLineTypes NewLineType { get; private set; } = MaterialPartLineTypes.NewRow;

        public string KeyText { get; private set; }

        public IMaterialPartView View { get; private set; }

        public ProjectMaterialPartSelectorViewModel()
        {
            AddNewMaterialPartCommand = new RelayCommand(() =>
            {
                var lookupDefinition = AppGlobals.LookupContext.MaterialPartLookup.Clone();
                lookupDefinition.WindowClosed += (s, e) =>
                {
                    NewMaterialPartPkValue = e.LookupData.SelectedPrimaryKeyValue;
                    NewLineType = MaterialPartLineTypes.MaterialPart;
                    View.CloseWindow();
                };
                lookupDefinition.ShowAddOnTheFlyWindow(KeyText, null);
            });
            AddNewMiscRowCommand = new RelayCommand(() =>
            {
                NewLineType = MaterialPartLineTypes.Miscellaneous;
                View.CloseWindow();
            });

            AddNewOverheadRowCommand = new RelayCommand(() =>
            {
                NewLineType = MaterialPartLineTypes.Overhead;
                View.CloseWindow();
            });

            AddCommentRowCommand = new RelayCommand(() =>
            {
                NewLineType = MaterialPartLineTypes.Comment;
                View.CloseWindow();
            });

            CancelCommand = new RelayCommand(() =>
            {
                NewLineType = MaterialPartLineTypes.Comment;
                View.CloseWindow();
            });

        }
        public void Initialize(IMaterialPartView view, string keyText)
        {
            View = view;
            KeyText = keyText;
            LabelText = $"The Material Part of '{keyText}' was not found.  What do you want to do?";
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
