using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Armin.Dunnhumby.Tests.UnitTests.Stores
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
        }

        [Fact]
        public void CreateTest()
        {
            var prd = new Product
            {
                Name = "TestProductName-Long"
            };
            var preCount = _db.Products.Count();
            var prdAfter = _store.Create(prd);
            var postCount = _db.Products.Count();

            Assert.Equal(prd.Name, prdAfter.Name);
            Assert.Equal(preCount + 1, postCount);

            var prd1 = new Product
            {
                Name = "TestProductName-Short"
            };
            var prd2 = new Product
            {
                Name = "ProductName-Short"
            };
            _store.Create(prd1);
            _store.Create(prd2);
            var lastCount = _db.Products.Count();
            Assert.Equal(postCount + 2, lastCount);
        }

        [Fact]
        public void SearchTest()
        {
            var all = _db.Products.ToList();
            _db.RemoveRange(all);
            _db.SaveChanges();

            CreateTest();
            var task = _store.Search("TestProductName-Long");
            var result = task.Result;
            Assert.Single(result);

            task = _store.Search("TestProductName");
            result = task.Result;

            Assert.Equal(2, result.Count());
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
            CreateTest();
            var preCount = _db.Products.Count();
            var firstProduct = _db.Products.First();
            _store.Delete(firstProduct);
            var postCount = _db.Products.Count();

            Assert.Equal(preCount - 1, postCount);

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
