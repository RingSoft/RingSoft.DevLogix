using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;

namespace RingSoft.DevLogix.Library.ViewModels.ProjectManagement
{
    public class LaborPartViewModel : DbMaintenanceViewModel<LaborPart>
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

        private double _minutesCost;

        public double MinutesCost
        {
            get => _minutesCost;
            set
            {
                if (_minutesCost == value)
                    return;

                _minutesCost = value;
                OnPropertyChanged();
            }
        }

        private string? _comment;

        public string? Comment
        {
            get => _comment;
            set
            {
                if (_comment == value)
                    return;

                _comment = value;
                OnPropertyChanged();
            }
        }

        #endregion

        protected override void PopulatePrimaryKeyControls(LaborPart newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(LaborPart entity)
        {
            MinutesCost = entity.MinutesCost;
            Comment = entity.Comment;
        }

        protected override LaborPart GetEntityData()
        {
            return new LaborPart
            {
                Id = Id,
                Name = KeyAutoFillValue.Text,
                MinutesCost = MinutesCost,
                Comment = Comment,
            };
        }

        protected override void ClearData()
        {
            Id = 0;
            MinutesCost = 0;
            Comment = null;
        }
    }
}
