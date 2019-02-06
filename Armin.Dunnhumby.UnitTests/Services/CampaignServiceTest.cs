using System;
using System.Linq;
using Armin.Dunnhumby.Domain.Data;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Services;
using Armin.Dunnhumby.Domain.Stores;
using Xunit;

namespace Armin.Dunnhumby.UnitTests.Services
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
            var result = _service.List();
            Assert.True(result.Any());
        }


        [Fact]
        public void ActiveTest()
        {
            var resultAll = _service.List(1, 500);
            var resultActive = _service.ActiveList(1, 500);
            Assert.NotEqual(resultActive.Data.Count, resultAll.Data.Count);
        }

        [Fact]
        public void UpdateTest()
        {
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
            var firstCampaign = _db.Campaigns.First();
            _service.Delete(firstCampaign);
            Assert.False(_service.Exists(firstCampaign.Id));
        }
    }
}