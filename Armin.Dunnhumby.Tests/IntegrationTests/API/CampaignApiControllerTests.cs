using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Xunit;

namespace Armin.Dunnhumby.Tests.IntegrationTests.API
{
    public class CampaignApiControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private const string BaseUrl = "/api/v1/campaigns";

        public CampaignApiControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        private async Task<PagedResult<ProductOutputModel>> ListCampaigns()
        {
            var response = await _client.GetAsync(BaseUrl + "/list/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var listResult = JsonConvert.DeserializeObject<PagedResult<ProductOutputModel>>(responseString);
            return listResult;
        }

        [Fact]
        public async Task ListTest()
        {
            // List Campaigns
            var listResult = await ListCampaigns();
            Assert.True(listResult.RecordCount > 0);
        }


        [Fact]
        public async Task FullTest()
        {


            // POST Create a product for later test purposes
            var pim = new ProductInputModel()
            {
                Name = "CreatedWithTest",
                Price = 25
            };
            var content = new StringContent(JsonConvert.SerializeObject(pim), Encoding.Default, "application/json");

            var createResponse = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var responseString = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<ProductOutputModel>(responseString);


            // Add a campaign
            var cim = new CampaignInputModel()
            {
                Name = "CreatedCampaignWithTest",
                ProductId = createdProduct.Id,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5)
            };
            content = new StringContent(JsonConvert.SerializeObject(cim), Encoding.Default, "application/json");
            createResponse = await _client.PostAsync(BaseUrl, content);
            responseString = await createResponse.Content.ReadAsStringAsync();
            var createdCampaign = JsonConvert.DeserializeObject<CampaignOutputModel>(responseString);
            Assert.NotNull(createdCampaign);
            Assert.Equal(cim.Name, createdCampaign.Name);



            // List Campaigns
            var listResult = await ListCampaigns();
            Assert.True(listResult.RecordCount > 0);



            // PUT Update
            cim = new CampaignInputModel()
            {
                Name = "ChangedNameInTest",
                ProductId = createdProduct.Id,
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(5)
            };
            var updateContent = new StringContent(JsonConvert.SerializeObject(cim), Encoding.Default, "application/json");

            var updateResponse = await _client.PutAsync(BaseUrl + $"/{createdCampaign.Id}", updateContent);
            var updateBody = await updateResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);



            // GET with id
            var getResponse = await _client.GetAsync($"{BaseUrl}/{createdCampaign.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getBody = await getResponse.Content.ReadAsStringAsync();
            var getModel = JsonConvert.DeserializeObject<CampaignOutputModel>(getBody);
            Assert.Equal("ChangedNameInTest", getModel.Name);



            // DELETE with id
            var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{getModel.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);


            // GET with id
            var getNoResponse = await _client.GetAsync($"{BaseUrl}/{getModel.Id}");
            Assert.Equal(HttpStatusCode.NoContent, getNoResponse.StatusCode);
        }
    }
}