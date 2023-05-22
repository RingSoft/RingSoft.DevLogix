using RingSoft.App.Library;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Sqlite;
using RingSoft.DevLogix.Tests.UserManagement;

namespace RingSoft.DevLogix.Tests
{
    public class TestGlobals<TViewModel, TView> where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public TestDataRepository DataRepository { get; private set; } = new TestDataRepository();
        public TViewModel ViewModel { get; private set; }

        public TView View { get; private set; }

        public void Initialize()
        {
            AppGlobals.UnitTesting = true;
            AppGlobals.Initialize();
            AppGlobals.DataRepository = DataRepository;
            AppGlobals.LookupContext.Initialize(new DevLogixSqliteDbContext(), DbPlatforms.Sqlite);
            AppGlobals.MainViewModel = new MainViewModel();

            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel));
            ViewModel = viewModel;

            var view = (TView)Activator.CreateInstance(typeof(TView));
            View = view;

            ViewModel.OnViewLoaded(View);
        }
    }
}
