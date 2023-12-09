using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorPriorityViewModel : DevLogixDbMaintenanceViewModel<ErrorPriority>
    {
        public override TableDefinition<ErrorPriority> TableDefinition => AppGlobals.LookupContext.ErrorPriorities;

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

        private int? _level;

        public int? Level
        {
            get => _level;
            set
            {
                if (_level == value)
                {
                    return;
                }
                _level = value;
                OnPropertyChanged();
            }
        }

        public UiCommand LevelUiCommand { get; } = new UiCommand();

        protected override void PopulatePrimaryKeyControls(ErrorPriority newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(ErrorPriority entity)
        {
            Level = entity.Level;
        }

        protected override ErrorPriority GetEntityData()
        {
            var errorPriority = new ErrorPriority
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                Level = Level ?? 0,
            };
            return errorPriority;

        }

        protected override bool ValidateEntity(ErrorPriority entity)
        {
            if (entity.Level == 0)
            {
                var message = "Level must have a value";
                var caption = "Validation Failure";
                LevelUiCommand.SetFocus();
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return false;
            }
            return base.ValidateEntity(entity);
        }

        protected override void ClearData()
        {
            Id = 0;
            KeyAutoFillValue = null;
            Level = null;
        }

        protected override bool SaveEntity(ErrorPriority entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, $"'{entity.Description}' Error Priority");
        }

        protected override bool DeleteEntity()
        {
            var errorPrioritySet = AppGlobals.DataRepository.GetDataContext().GetTable<ErrorPriority>();
            var errorPriority = errorPrioritySet.FirstOrDefault(p => p.Id == Id);
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.DeleteEntity(errorPriority, $"Deleting Error Priority '{errorPriority.Description}'");
        }
    }
}
