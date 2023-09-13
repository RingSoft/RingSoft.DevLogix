using RingSoft.DbLookup;
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
        }
    }
}
