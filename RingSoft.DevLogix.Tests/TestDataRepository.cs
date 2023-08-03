using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.Testing;
using RingSoft.DevLogix.DataAccess.Model;
using RingSoft.DevLogix.DataAccess.Model.ProjectManagement;
using RingSoft.DevLogix.DataAccess.Model.UserManagement;
using RingSoft.DevLogix.Library;
using IDataRepository = RingSoft.DevLogix.Library.IDataRepository;

namespace RingSoft.DevLogix.Tests
{
    public class TestDataRegistry : DataRepositoryRegistry , DataAccess.IDbContext
    {
    }


    public class DevLogixTestDataRepository : TestDataRepository, IDataRepository
    {
        public new TestDataRegistry DataContext { get; }

        public DevLogixTestDataRepository(TestDataRegistry context) : base(context)
        {
            DataContext = context;
            DataContext.AddEntity(new DataRepositoryRegistryItem<User>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Department>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<TimeClock>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Project>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<ProjectTask>());
        }

        public DataAccess.IDbContext GetDataContext()
        {
            return DataContext;
        }
    }
}
    