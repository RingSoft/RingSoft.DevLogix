using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public interface ITaskLaborPartView
    {
        void CloseWindow();
    }
    public class TaskLaborPartLtSelectorViewModel : INotifyPropertyChanged
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

        public RelayCommand AddNewLaborPartCommand { get; private set; }

        public RelayCommand AddNewMiscRowCommand { get; private set; }

        public RelayCommand AddCommentRowCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public PrimaryKeyValue NewLaborPartPkValue { get; private set; }

        public LaborPartLineTypes NewLineType { get; private set; } = LaborPartLineTypes.NewRow;

        public string KeyText { get; private set; }

        public ITaskLaborPartView View { get; private set; }

        public void Initialize(ITaskLaborPartView view, string keyText)
        {
            View = view;
            KeyText = keyText;
            LabelText = $"The Labor Part of '{keyText}' was not found.  What do you want to do?";
        }
        public TaskLaborPartLtSelectorViewModel()
        {
            AddNewLaborPartCommand = new RelayCommand(() =>
            {
                var selectedPrimaryKey = SystemGlobals.TableRegistry.ShowNewAddOnTheFly(
                    AppGlobals.LookupContext.LaborParts
                    , null
                    , KeyText
                    , null
                    , false);

                {

                }
                //var lookupDefinition = AppGlobals.LookupContext.LaborPartLookup.Clone();
                //lookupDefinition.WindowClosed += (s, e) =>
                //{
                //    NewLaborPartPkValue = e.LookupData.SelectedPrimaryKeyValue;
                //    NewLineType = LaborPartLineTypes.LaborPart;
                //    View.CloseWindow();
                //};
                //lookupDefinition.ShowAddOnTheFlyWindow(KeyText, null);
            });

            AddNewMiscRowCommand = new RelayCommand(() =>
            {
                NewLineType = LaborPartLineTypes.Miscellaneous;
                View.CloseWindow();
            });

            AddCommentRowCommand = new RelayCommand(() =>
            {
                NewLineType = LaborPartLineTypes.Comment;
                View.CloseWindow();
            });

            CancelCommand = new RelayCommand(() =>
            {
                NewLineType = LaborPartLineTypes.NewRow;
                View.CloseWindow();
            });
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
