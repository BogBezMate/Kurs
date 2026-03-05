using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Kurs.Data;

namespace Kurs.Data
{
    public class CarServiceContextFactory : IDesignTimeDbContextFactory<CarServiceContext>
    {
        public CarServiceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarServiceContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=CarServiceDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new CarServiceContext(optionsBuilder.Options);
        }
    }
}
