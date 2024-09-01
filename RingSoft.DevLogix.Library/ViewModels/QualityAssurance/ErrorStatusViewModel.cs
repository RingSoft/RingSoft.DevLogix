using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;

namespace RingSoft.DevLogix.Library.ViewModels.QualityAssurance
{
    public class ErrorStatusViewModel : DbMaintenanceViewModel<ErrorStatus>
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

        #endregion

        protected override void PopulatePrimaryKeyControls(ErrorStatus newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(ErrorStatus entity)
        {
            
        }

        protected override ErrorStatus GetEntityData()
        {
            var errorStatus = new ErrorStatus
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
            };
            return errorStatus;
        }

        protected override void ClearData()
        {
            Id = 0;
        }
    }
}
