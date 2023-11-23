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
        TechnicalSupport = 3,
        Administration = 4,
        Production = 5,
    }

    public enum TestUsers
    {
        DaveSmittyPD = 1,
        JohnDoeQA = 2,
        AnnaSmallsaTS = 3,
        PeterBlankardAdmin = 3,
    }

    public enum TestErrorStatuses
    {
        PendingCorrection = 1,
        PendingUnitTest = 2,
        PendingQaTest = 3,
        PendingQaFail = 4,
        PendingRelease = 5,
    }

    public enum TestErrorPriorities
    {
        Design = 1,
        Cosmetic = 2,
        Procedural = 3,
        DataCorruption = 4,
        AppCrash = 5,
    }

    public enum TestProducts
    {
        DevLogix = 1,
    }

    public enum TestVersions
    {
        First = 1,
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
            var context = SystemGlobals.DataRepository.GetDataContext();
            var pendCorrStatus = new ErrorStatus
            {
                Id = (int)TestErrorStatuses.PendingCorrection,
                Description = "Pending Correction",
            };
            context.SaveEntity(pendCorrStatus, "Saving Error Status");

            var pendUtStatus = new ErrorStatus
            {
                Id = (int)TestErrorStatuses.PendingUnitTest,
                Description = "Pending Unit Test",
            };
            context.SaveEntity(pendUtStatus, "Saving Error Status");

            var pendQaTestStatus = new ErrorStatus
            {
                Id = (int)TestErrorStatuses.PendingQaTest,
                Description = "Pending QA Test",
            };
            context.SaveEntity(pendQaTestStatus, "Saving Error Status");

            var pendQaFailStatus = new ErrorStatus
            {
                Id = (int)TestErrorStatuses.PendingQaFail,
                Description = "Pending QA Fail",
            };
            context.SaveEntity(pendQaFailStatus, "Saving Error Status");

            var pendReleaseStatus = new ErrorStatus
            {
                Id = (int)TestErrorStatuses.PendingRelease,
                Description = "Pending Release",
            };
            context.SaveEntity(pendReleaseStatus, "Saving Error Status");

            var errorPriority = new ErrorPriority
            {
                Id = (int)TestErrorPriorities.Design,
                Description = "Design",
                Level = 10,
            };

            context.SaveEntity(errorPriority, "Saving Error Priority");

            errorPriority = new ErrorPriority
            {
                Id = (int)TestErrorPriorities.Cosmetic,
                Description = "Cosmetic",
                Level = 20,
            };

            context.SaveEntity(errorPriority, "Saving Error Priority");

            errorPriority = new ErrorPriority
            {
                Id = (int)TestErrorPriorities.Procedural,
                Description = "Procedural",
                Level = 30,
            };

            context.SaveEntity(errorPriority, "Saving Error Priority");

            errorPriority = new ErrorPriority
            {
                Id = (int)TestErrorPriorities.DataCorruption,
                Description = "Data Corruption",
                Level = 40,
            };

            context.SaveEntity(errorPriority, "Saving Error Priority");

            errorPriority = new ErrorPriority
            {
                Id = (int)TestErrorPriorities.AppCrash,
                Description = "Application Crash",
                Level = 50,
            };

            context.SaveEntity(errorPriority, "Saving Error Priority");

            var pdDept = new Department()
            {
                Id = (int)TestDepartments.ProductDevelopment,
                Description = "Product Development",
                ErrorFixStatusId = pendUtStatus.Id,
                FixText = "Fixed",
                ErrorFailStatusId = pendCorrStatus.Id,
                FailText = "UT Fail",
                ErrorPassStatusId = pendQaTestStatus.Id,
                PassText = "UT Pass",
                ReleaseLevel = 10,
            };

            context.SaveEntity(pdDept, "Saving Department");

            var qaDept = new Department()
            {
                Id = (int)TestDepartments.QualityAssurance,
                Description = "Quality Assurance",
                ErrorFailStatusId = pendQaFailStatus.Id,
                FailText = "QA Fail",
                ErrorPassStatusId = pendReleaseStatus.Id,
                PassText = "QA Pass",
                ReleaseLevel = 20,
            };

            context.SaveEntity(qaDept, "Saving Department");

            var supportDept = new Department()
            {
                Id = (int)TestDepartments.TechnicalSupport,
                Description = "Technical Support",
                ReleaseLevel = 30,
            };

            context.SaveEntity(supportDept, "Saving Department");

            var adminDept = new Department()
            {
                Id = (int)TestDepartments.Administration,
                Description = "Administration",
                ReleaseLevel = 30,
            };

            context.SaveEntity(adminDept, "Saving Department");

            var productionDept = new Department()
            {
                Id = (int)TestDepartments.Production,
                Description = "Production",
                ReleaseLevel = 30,
            };

            context.SaveEntity(productionDept, "Saving Department");

            var user = new User
            {
                Id = (int)TestUsers.DaveSmittyPD,
                Name = "Dave Smitty (PD)",
                DepartmentId = pdDept.Id,
            };

            context.SaveEntity(user, "Saving User");

            user = new User
            {
                Id = (int)TestUsers.JohnDoeQA,
                Name = "John Doe (QA)",
                DepartmentId = qaDept.Id,
            };

            context.SaveEntity(user, "Saving User");

            user = new User
            {
                Id = (int)TestUsers.AnnaSmallsaTS,
                Name = "Anna Smallsa (Support)",
                DepartmentId = supportDept.Id,
            };

            context.SaveEntity(user, "Saving User");

            user = new User
            {
                Id = (int)TestUsers.PeterBlankardAdmin,
                Name = "Peter Blankard (Admin)",
                DepartmentId = adminDept.Id,
            };

            context.SaveEntity(user, "Saving User");

            var product = new Product
            {
                Id = (int)TestProducts.DevLogix,
                Description = "DevLogix",
                CreateDepartmentId = (int)TestDepartments.ProductDevelopment,
            };

            context.SaveEntity(product, "Saving Product");

            var version = new ProductVersion
            {
                Id = (int)TestVersions.First,
                Description = "00.95.00",
                ProductId = product.Id,
                DepartmentId = product.CreateDepartmentId,
                VersionDate = DateTime.Now,
            };

            context.SaveEntity(version, "Saving Product Version");

            var versionDept = new ProductVersionDepartment
            {
                VersionId = product.Id,
                DepartmentId = product.CreateDepartmentId,
                ReleaseDateTime = DateTime.Now,
            };

            context.SaveEntity(versionDept, "Saving Product Version Department");
        }

        public void LoginToUser(TestUsers userType)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<User>();
            var user = table
                .FirstOrDefault(p => p.Id == (int)userType);

            if (user != null)
            {
                AppGlobals.LookupContext.Users.FillOutEntity(user);
                AppGlobals.LoggedInUser = user;
            }
        }

        public ErrorStatus GetErrorStatus(TestErrorStatuses errorStatusType)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ErrorStatus>();
            var result = table
                .FirstOrDefault(p => p.Id == (int)errorStatusType);
            return result;
        }

        public ErrorPriority GetErrorPriority(TestErrorPriorities errorPriorityType)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ErrorPriority>();
            var result = table
                .FirstOrDefault(p => p.Id == (int)errorPriorityType);
            return result;
        }


        public Product GetProduct(TestProducts productType)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Product>();
            var result = table
                .FirstOrDefault(p => p.Id == (int)productType);
            return result;
        }

        public ProductVersion GetProductVersion(TestVersions versionType)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<ProductVersion>();
            var result = table
                .FirstOrDefault(p => p.Id == (int)versionType);
            return result;
        }
    }
}
