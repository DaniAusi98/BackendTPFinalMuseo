using Infrastructure.Repositories.Sql;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Factories
{
    public class MuseoDbContextFactory
        : IDesignTimeDbContextFactory<MuseoDbContext>
    {
        public MuseoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuseoDbContext>();

            var connectionString =
                "Server=localhost;Port=3306;Database=museobasedatos;User=root;Password=Daniel9807mysql;";

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));

            return new MuseoDbContext(optionsBuilder.Options);
        }
    }
}
