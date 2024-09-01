using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorPriorityViewModel : DbMaintenanceViewModel<ErrorPriority>
    {
        #region Properties

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

        #endregion

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
            Level = null;
        }
    }
}
