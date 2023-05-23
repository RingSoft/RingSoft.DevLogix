using Org.BouncyCastle.Asn1.Esf;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.QualityAssurance;
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

            Globals.ViewModel.NewCommand.Execute(null);
            var department = new Department()
            {
                Id = 1,
                Description = "Test"
            };
            autoFillValue = department.GetAutoFillValue();
            Assert.IsTrue(autoFillValue.IsValid());
            Globals.ViewModel.DepartmentAutoFillValue = autoFillValue;
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            var chart = new DevLogixChart()
            {
                Id = 1,
                Name = "Test",
            };
            autoFillValue = chart.GetAutoFillValue();
            Assert.IsTrue(autoFillValue.IsValid());
            Globals.ViewModel.DefaultChartAutoFillValue = autoFillValue;
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            autoFillValue = user.GetAutoFillValue();
            Assert.IsTrue(autoFillValue.IsValid());
            Globals.ViewModel.SupervisorAutoFillValue = autoFillValue;
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            var clockDate = new DateTime(2000, 1, 1);
            Globals.ViewModel.ClockDateTime = clockDate;
            Assert.IsFalse(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            var clockReason = "Test";
            Globals.ViewModel.ClockReason = clockReason;
            Assert.IsFalse(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.EmailAddress = "Test";
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.PhoneNumber = "Test";
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.HourlyRate = 40;
            Assert.IsTrue(Globals.ViewModel.RecordDirty);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.Notes = "Test";
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
                HourlyRate = 10,
            };

            var timeClock = new TimeClock()
            {
                Id = 1,
                MinutesSpent = 60,
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

            timeClock = new TimeClock()
            {
                Id = 2,
                MinutesSpent = 120,
                ProjectTask = new ProjectTask()
                {
                    Id = 2,
                    Name = "Test2",
                    Project = new Project()
                    {
                        Id = 2,
                        Name = "Test2",
                        IsBillable = false,
                    },
                },
                ProjectTaskId = 2,
            };
            user.TimeClocks.Add(timeClock);

            timeClock = new TimeClock()
            {
                Id = 3,
                MinutesSpent = 180,
                Error = new Error()
                {
                    Id = 1,
                },
                ErrorId = 1,
            };
            user.TimeClocks.Add(timeClock);

            timeClock = new TimeClock()
            {
                Id = 4,
                MinutesSpent = 240,
                TestingOutline = new TestingOutline(),
                TestingOutlineId = 1,
            };

            user.TimeClocks.Add(timeClock);

            Globals.DataRepository.DataContext.SaveNoCommitEntity(user, "Test");
            Globals.ViewModel.RecalcCommand.Execute(null);
            Assert.IsTrue(user.BillableProjectsMinutesSpent == 60);
            Assert.IsTrue(user.NonBillableProjectsMinutesSpent == 120);
            Assert.IsTrue(user.ErrorsMinutesSpent == 180);
            Assert.IsTrue(user.TestingOutlinesMinutesSpent == 240);
        }
    }
}
