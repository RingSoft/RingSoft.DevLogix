using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;

namespace RingSoft.DevLogix.Library.ViewModels.UserManagement
{
    public class UserTrackerViewModel  : DevLogixDbMaintenanceViewModel<UserTracker>
    {
        public override TableDefinition<UserTracker> TableDefinition => AppGlobals.LookupContext.UserTracker;

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

        protected override UserTracker PopulatePrimaryKeyControls(UserTracker newEntity, PrimaryKeyValue primaryKeyValue)
        {
            return new UserTracker();
        }

        protected override void LoadFromEntity(UserTracker entity)
        {
            
        }

        protected override UserTracker GetEntityData()
        {
            return new UserTracker();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(UserTracker entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
