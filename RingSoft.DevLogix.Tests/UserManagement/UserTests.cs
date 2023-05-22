using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.UserManagement;
using RingSoft.DevLogix.Sqlite;

namespace RingSoft.DevLogix.Tests.UserManagement
{
    [TestClass]
    public class UserTests
    {
        public static TestGlobals<UserMaintenanceViewModel, UserView> Globals { get; } =
            new TestGlobals<UserMaintenanceViewModel, UserView>();

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestUserDirty()
        {
            var user = new User()
            {
                Id = 1,
                Name = "Test",
            };

            var autoFillValue = user.GetAutoFillValue();
            Assert.IsTrue(autoFillValue.IsValid());
            Globals.ViewModel.KeyAutoFillValue = autoFillValue;
            Assert.IsTrue(Globals.ViewModel.RecordDirty);
        }
    }
}
