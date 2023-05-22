using Org.BouncyCastle.Asn1.Esf;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
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
            Globals.ClearData();
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

        [TestMethod]
        public void TestUserRecalculate()
        {
            Globals.ClearData();
            var user = new User()
            {
                Id = 1,
                Name = "Test",
            };

            var timeClock = new TimeClock()
            {
                Id = 1,
                MinutesSpent = 10,
                ProjectTask = new ProjectTask()
                {
                    Id = 1,
                    Name = "Test",
                    Project = new Project()
                    {
                        Id = 1,
                        Name = "Test",
                        IsBillable = true,
                    },
                },
                ProjectTaskId = 1,
            };

            user.TimeClocks.Add(timeClock);

            Globals.DataRepository.DataContext.SaveNoCommitEntity(user, "Test");
            Globals.ViewModel.RecalcCommand.Execute(null);
            Assert.IsTrue(user.BillableProjectsMinutesSpent == 10);
        }
    }
}
