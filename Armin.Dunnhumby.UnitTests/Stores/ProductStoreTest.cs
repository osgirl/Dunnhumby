using System;
using System.Linq;
using Armin.Dunnhumby.Domain.Data;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Stores;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Armin.Dunnhumby.UnitTests.Stores
{
    public class ProductStoreTest : BaseTestUnit
    {
        private readonly ApplicationDbContext _db;
        private readonly ProductStore _store;
        private readonly IMemoryCache _cache;

        public ProductStoreTest()
        {
            _db = BuildDataContext();
            _cache = BuildCache();
            _store = new ProductStore(_db, _cache);
            var pc = _db.Products.Count();
        }

        [Fact]
        public void CreateTest()
        {
            var prd = new Product
            {
                Name = "TestProductName-Long",
                Price = 50
            };
            var preCount = _db.Products.Count();
            var prdAfter = _store.Create(prd);
            var postCount = _db.Products.Count();

            Assert.Equal(prd.Name, prdAfter.Name);
            Assert.True(preCount < postCount);

            var prd1 = new Product
            {
                Name = "TestProductName-Short",
                Price = 60
            };
            var prd2 = new Product
            {
                Name = "ProductName-Short",
                Price = 70
            };
            _store.Create(prd1);
            _store.Create(prd2);
            var lastCount = _db.Products.Count();
            Assert.Equal(postCount + 2, lastCount);
        }

        [Fact]
        public void ListTest()
        {
            var task = _store.Search("TestProductName-Long");
            var result = task.Result;
            Assert.Single(result);

            task = _store.Search("TestProductName");
            result = task.Result;

            Assert.True(result.Any());
        }

        [Fact]
        public void UpdateTest()
        {
            var firstProduct = _db.Products.First();
            firstProduct.Price += 100;
            DateTime now = DateTime.Now;
            _store.Update(firstProduct);

            var changedProduct = _db.Products.FirstOrDefault(p => p.Id == firstProduct.Id);
            Assert.NotNull(changedProduct);
            Assert.Equal(firstProduct.Price, changedProduct.Price);
            Assert.True(changedProduct.LastUpdate >= now);
        }

        [Fact]
        public void DeleteTest()
        {
            var firstProduct = _db.Products.First();
            _store.Delete(firstProduct);
            var noFound = _db.Products.FirstOrDefault(p => p.Id == firstProduct.Id);
            Assert.Null(noFound);


            var found = _db.Products.FirstOrDefault(p => p.Id == firstProduct.Id);
            Assert.Null(found);
        }

        [Fact]
        public void CacheTest()
        {
            const string cacheKey = "_PROD_";
            CreateTest();
            _store.List();

            _cache.TryGetValue(cacheKey, out var productsList);

            Assert.NotNull(productsList);
        }
    }
}