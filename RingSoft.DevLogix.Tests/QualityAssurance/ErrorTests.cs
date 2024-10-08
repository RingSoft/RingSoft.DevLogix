﻿using RingSoft.DbLookup;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.Library;
using RingSoft.DevLogix.Library.ViewModels.QualityAssurance;

namespace RingSoft.DevLogix.Tests.QualityAssurance
{
    [TestClass]
    public class ErrorTests
    {
        public static TestGlobals<ErrorViewModel, ErrorView> Globals { get; } =
            new TestGlobals<ErrorViewModel, ErrorView>();

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestErrorSave()
        {
            Globals.ClearData();

            Globals.LoginToUser(TestUsers.DaveSmittyPD);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.StatusAutoFillValue = Globals.GetErrorStatus(TestErrorStatuses.PendingCorrection)
                .GetAutoFillValue();

            var product = Globals.GetProduct(TestProducts.DevLogix);
            Globals.ViewModel.ProductAutoFillValue = product.GetAutoFillValue();
            Globals.ViewModel.PriorityAutoFillValue = Globals.GetErrorPriority(
                TestErrorPriorities.Procedural)
                .GetAutoFillValue();

            Globals.ViewModel.Description = "Test";
            //Globals.ViewModel.WriteOffCommand.Execute(null);

            Assert.IsFalse(Globals.ViewModel.WriteOffCommand.IsEnabled);
            Globals.ViewModel.SaveCommand.Execute(null);

            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Error>();
            var error = table.LastOrDefault();

            Assert.AreEqual("E-2", error.ErrorId);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.OnRecordSelected(error);
            
            Assert.AreEqual("E-2", Globals.ViewModel.KeyAutoFillValue.Text);

            Globals.ViewModel.WriteOffCommand.Execute(null);
            Globals.ViewModel.SaveCommand.Execute(null);
            Globals.ViewModel.OnRecordSelected(error);
            Assert.AreEqual(1, Globals.ViewModel.DeveloperManager.Rows.Count);

            var errorStatus = Globals.ViewModel.StatusAutoFillValue.GetEntity<ErrorStatus>();
            Assert.AreEqual(AppGlobals.LoggedInUser.Department.ErrorFixStatusId, errorStatus.Id);

            Globals.ViewModel.PassCommand.Execute(null);
            errorStatus = Globals.ViewModel.StatusAutoFillValue.GetEntity<ErrorStatus>();
            Assert.AreEqual(AppGlobals.LoggedInUser.Department.ErrorPassStatusId, errorStatus.Id);

            Globals.LoginToUser(TestUsers.JohnDoeQA);
            Globals.ViewModel.FailCommand.Execute(null);
            errorStatus = Globals.ViewModel.StatusAutoFillValue.GetEntity<ErrorStatus>();
            Assert.AreEqual(AppGlobals.LoggedInUser.Department.ErrorFailStatusId, errorStatus.Id);
        }

        [TestMethod]
        public void TestErrorPunchIn()
        {
            Globals.ClearData();

            Globals.LoginToUser(TestUsers.DaveSmittyPD);

            Globals.ViewModel.NewCommand.Execute(null);
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Error>();

            var e1Error = table
                .FirstOrDefault(p => p.Id == (int)TestErrors.E1);
            Globals.ViewModel.OnRecordSelected(e1Error);

            Globals.ViewModel.PunchInCommand.Execute(null);
            Assert.AreNotEqual(AppGlobals.MainViewModel.ActiveTimeClockId, 0);

            var activeTimeClock = AppGlobals.MainViewModel.TimeClockMaintenanceViewModel;
            var tcUser = activeTimeClock.UserAutoFillValue.GetEntity<User>();
            Assert.AreEqual(tcUser.Id, (int)TestUsers.DaveSmittyPD);

            Assert.AreEqual(activeTimeClock.ErrorAutoFillValue.GetEntity<Error>().Id
            , (int)TestErrors.E1);

            Globals.PunchOut(activeTimeClock, 20);
            var userTable = context.GetTable<User>();
            var user = userTable.FirstOrDefault(p => p.Id == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(20, user.ErrorsMinutesSpent);
            var cost = Math.Round(user.HourlyRate / 3, 2);

            e1Error = table
                .FirstOrDefault(p => p.Id == (int)TestErrors.E1);
            e1Error.UtFillOutEntity();
            var errorUser = e1Error.Users.FirstOrDefault(p => p.UserId == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(20, errorUser.MinutesSpent);
            Assert.AreEqual(cost, errorUser.Cost);

            var userRow = Globals.ViewModel
                .ErrorUserGridManager
                .Rows.OfType<ErrorUserRow>()
                .FirstOrDefault(p => p.UserId == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(20, userRow.MinutesSpent);
            Assert.AreEqual(cost, userRow.Cost);

            Globals.ViewModel.PunchInCommand.Execute(null);

            activeTimeClock = AppGlobals.MainViewModel.TimeClockMaintenanceViewModel;
            tcUser = activeTimeClock.UserAutoFillValue.GetEntity<User>();
            Assert.AreEqual(tcUser.Id, (int)TestUsers.DaveSmittyPD);

            Assert.AreEqual(activeTimeClock.ErrorAutoFillValue.GetEntity<Error>().Id
                , (int)TestErrors.E1);

            Globals.PunchOut(activeTimeClock, 20);
            userTable = context.GetTable<User>();
            user = userTable.FirstOrDefault(p => p.Id == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(40, user.ErrorsMinutesSpent);
            cost = 53.33;

            e1Error = table
                .FirstOrDefault(p => p.Id == (int)TestErrors.E1);
            e1Error.UtFillOutEntity();
            errorUser = e1Error.Users.FirstOrDefault(p => p.UserId == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(40, errorUser.MinutesSpent);
            Assert.AreEqual(cost, errorUser.Cost);

            userRow = Globals.ViewModel
                .ErrorUserGridManager
                .Rows.OfType<ErrorUserRow>()
                .FirstOrDefault(p => p.UserId == (int)TestUsers.DaveSmittyPD);
            Assert.AreEqual(40, userRow.MinutesSpent);
            Assert.AreEqual(cost, userRow.Cost);
        }
    }
}
