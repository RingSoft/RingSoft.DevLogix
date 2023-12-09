using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model.CustomerManagement;
using System.Linq;

namespace RingSoft.DevLogix.Library.ViewModels.CustomerManagement
{
    public class SupportTicketStatusViewModel : DevLogixDbMaintenanceViewModel<SupportTicketStatus>
    {
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
            KeyAutoFillValue = null;
        }

        protected override bool SaveEntity(SupportTicketStatus entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            return context.SaveEntity(entity, $"Saving Support Ticket Status '{entity.Description}'");
        }

        protected override bool DeleteEntity()
        {
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<SupportTicketStatus>();
            var entity = query.FirstOrDefault(p => p.Id == Id);
            var context = AppGlobals.DataRepository.GetDataContext();
            return entity != null && context.DeleteEntity(entity, $"Deleting Support Ticket Status '{entity.Description}'");
        }
    }
}
