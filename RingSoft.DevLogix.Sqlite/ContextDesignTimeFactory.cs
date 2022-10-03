using Microsoft.EntityFrameworkCore.Design;

namespace RingSoft.DevLogix.Sqlite
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<DevLogixSqliteDbContext>
    {
        DevLogixSqliteDbContext IDesignTimeDbContextFactory<DevLogixSqliteDbContext>.CreateDbContext(string[] args)
        {
            return new DevLogixSqliteDbContext(){IsDesignTime = true};
        }
    }
}
