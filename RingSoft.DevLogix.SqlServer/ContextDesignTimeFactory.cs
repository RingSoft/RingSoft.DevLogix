using Microsoft.EntityFrameworkCore.Design;

namespace RingSoft.DevLogix.SqlServer
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<DevLogixSqlServerDbContext>
    {
        DevLogixSqlServerDbContext IDesignTimeDbContextFactory<DevLogixSqlServerDbContext>.CreateDbContext(string[] args)
        {
            return new DevLogixSqlServerDbContext(){IsDesignTime = true};
        }
    }
}
