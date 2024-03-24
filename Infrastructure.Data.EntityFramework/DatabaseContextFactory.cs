using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Infrastructure.Data.EntityFramework
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        //use for migrations
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ContentManagement;Integrated Security=True;Encrypt=False");
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
