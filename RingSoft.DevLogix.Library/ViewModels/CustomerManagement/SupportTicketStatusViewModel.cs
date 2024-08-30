using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class SupportTicketStatusViewModel : DbMaintenanceViewModel<SupportTicketStatus>
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
        protected override void PopulatePrimaryKeyControls(SupportTicketStatus newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override void LoadFromEntity(SupportTicketStatus entity)
        {

        }

        protected override SupportTicketStatus GetEntityData()
        {
            var supportTicketStatus = new SupportTicketStatus()
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
            };
            return supportTicketStatus;

        }

        protected override void ClearData()
        {
            Id = 0;
        }
    }
}
