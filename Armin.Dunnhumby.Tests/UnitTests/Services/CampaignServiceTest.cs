using System;
using System.Linq;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Services;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Armin.Dunnhumby.Tests.UnitTests.Services
{
    public class CampaignServiceTest : BaseTestUnit
    {
        private readonly ApplicationDbContext _db;
        private readonly CampaignStore _store;
        private readonly CampaignService _service;

        public CampaignServiceTest()
        {
            _db = BuildDataContext();
            _store = new CampaignStore(_db);
            _service = new CampaignService(_store);
        }

        [Fact]
        public void CreateTest()
        {
            ClearData();

            var prd = new Product
            {
                Name = "TestProduct",
                Price = 10
            };
            var product = _db.Products.Add(prd);
            _db.SaveChanges();
            prd = product.Entity;

            var camp1 = new Campaign
            {
                Name = "Cyber Monday",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                ProductId = prd.Id
            };
            var camp2 = new Campaign
            {
                Name = "Expired Campaign",
                Start = DateTime.Now.AddDays(-2),
                End = DateTime.Now.AddDays(-1),
                ProductId = prd.Id
            };

            var preCount = _db.Campaigns.Count();
            var campaign1 = _store.Create(camp1);
            var campaign2 = _store.Create(camp2);
            var postCount = _db.Campaigns.Count();

            Assert.Equal(camp1.Name, campaign1.Name);
            Assert.Equal(preCount + 2, postCount);
        }

        [Fact]
        public void ListTest()
        {
            CreateTest();
            var result = _service.List();
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public void ActiveTest()
        {
            CreateTest();
            var resultAll = _service.List(1);
            var resultActive = _service.ActiveList(1);
            Assert.NotEqual(resultActive.Data.Count, resultAll.Data.Count);
        }

        [Fact]
        public void UpdateTest()
        {
            CreateTest();

            var firstCampaign = _db.Campaigns.First(c => c.Name != "Updated");
            firstCampaign.Name = "Updated";
            DateTime now = DateTime.Now;
            _service.Update(firstCampaign);

            var changedProduct = _db.Campaigns.FirstOrDefault(p => p.Id == firstCampaign.Id);
            Assert.NotNull(changedProduct);
            Assert.Equal(firstCampaign.Name, changedProduct.Name);
            Assert.True(changedProduct.LastUpdate >= now);
        }

        [Fact]
        public void DeleteTest()
        {
            CreateTest();

            var preCount = _db.Campaigns.Count();
            var firstCampaign = _db.Campaigns.First();
            _service.Delete(firstCampaign);
            var postCount = _db.Campaigns.Count();

            Assert.Equal(preCount - 1, postCount);

            var found = _db.Products.FirstOrDefault(p => p.Id == firstCampaign.Id);
            Assert.Null(found);
        }

        private void ClearData()
        {
            var allCampaigns = _db.Campaigns.ToList();
            _db.RemoveRange(allCampaigns);
            var all = _db.Products.ToList();
            _db.RemoveRange(all);

            _db.SaveChanges();
        }
    }
}