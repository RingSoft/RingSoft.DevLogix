using RingSoft.DbLookup;
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
            var error = table.FirstOrDefault();

            Assert.AreEqual("E-1", error.ErrorId);

            Globals.ViewModel.NewCommand.Execute(null);
            Globals.ViewModel.OnRecordSelected(error);
            
            Assert.AreEqual("E-1", Globals.ViewModel.KeyAutoFillValue.Text);

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
    }
}
