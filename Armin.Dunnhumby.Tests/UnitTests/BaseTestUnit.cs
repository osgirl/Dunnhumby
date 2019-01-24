using Armin.Dunnhumby.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Armin.Dunnhumby.Tests.UnitTests
{
    public class BaseTestUnit
    {
        public ApplicationDbContext BuildDataContext(bool withData = true)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder = builder.UseInMemoryDatabase("TestDb");

            var db = new ApplicationDbContext(builder.Options);
            db.Database.EnsureCreated();



            return db;
        }

        public MemoryCache BuildCache()
        {
            return new MemoryCache(new MemoryCacheOptions());
        }
    }
}
