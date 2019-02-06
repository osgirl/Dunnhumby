using System;
using Armin.Dunnhumby.Domain.Data;
using Armin.Dunnhumby.Domain.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Armin.Dunnhumby.UnitTests
{
    public class BaseTestUnit
    {
        public ApplicationDbContext BuildDataContext(bool withData = true)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder = builder.UseInMemoryDatabase("TestDbx");

            ProductsSeed.SeedData = false;
            var db = new ApplicationDbContext(builder.Options);
            
            db.Database.EnsureCreated();
            try
            {
                if (withData)
                {
                    // Seed the database with test data.
                    Utilities.PrepareDbForTests(db);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }


            return db;
        }

        public MemoryCache BuildCache()
        {
            return new MemoryCache(new MemoryCacheOptions());
        }
    }
}
