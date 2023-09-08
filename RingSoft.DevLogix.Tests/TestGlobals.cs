using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels;
using RingSoft.DevLogix.Sqlite;

namespace RingSoft.DevLogix.Tests
{
    public enum TestDepartments
    {
        ProductDevelopment = 1,
        QualityAssurance = 2,
        Production = 3,
    }

    public enum TestUsers
    {
        DaveSmittyPD = 1,
        JohnDoeQA = 2,
    }
    public class TestGlobals<TViewModel, TView> : DbMaintenanceTestGlobals<TViewModel, TView>
        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public new DevLogixTestDataRepository DataRepository { get; } 
            
        public TestGlobals() : base(new DevLogixTestDataRepository(new TestDataRegistry()))
        {
            DataRepository = base.DataRepository as DevLogixTestDataRepository;
        }

        public override void Initialize()
        {
            AppGlobals.UnitTesting = true;
            AppGlobals.Initialize();
            AppGlobals.DataRepository = DataRepository;
            SystemGlobals.DataRepository = DataRepository;
            AppGlobals.LookupContext.Initialize(new DevLogixSqliteDbContext(), DbPlatforms.Sqlite);
            AppGlobals.MainViewModel = new MainViewModel();
            AppGlobals.LoggedInUser = new User();
            AppGlobals.Rights = new AppRights();

            base.Initialize();
        }

        public override void ClearData()
        {
            AppGlobals.LoggedInUser = new User();
            AppGlobals.Rights = new AppRights();

            base.ClearData();
            LoadDatabase();
        }


        private void LoadDatabase()
        {

        }
    }
}
