using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Models;
using Newtonsoft.Json;
using Xunit;

namespace Armin.Dunnhumby.Tests.IntegrationTests.API
{
    public class ProductApiControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private const string BaseUrl = "/api/v1/products";

        public ProductApiControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FullTest()
        {
            var client = _factory.CreateClient();

            // POST Create
            var pim = new ProductInputModel()
            {
                Name = "CreatedWithTest",
                Price = 25
            };
            var content = new StringContent(JsonConvert.SerializeObject(pim),Encoding.Default, "text/json");

            var createResponse = await client.PostAsync(BaseUrl, content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var responseString = await createResponse.Content.ReadAsStringAsync();
            var createModel = JsonConvert.DeserializeObject<ProductOutputModel>(responseString);
            Assert.NotNull(createModel);
            Assert.Equal("CreatedWithTest", createModel.Name);
            Assert.Equal(25, createModel.Price);


            // List add Test
            var response = await client.GetAsync($"{BaseUrl}/list/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            responseString = await response.Content.ReadAsStringAsync();

            var searchResult = JsonConvert.DeserializeObject<PagedResult<ProductOutputModel>>(responseString);
            Assert.True(searchResult.RecordCount > 0);


            // PUT Update
            pim = new ProductInputModel()
            {
                Name = "ChangedNameInTest",
                Price = 98
            };
            var updateContent = new StringContent(JsonConvert.SerializeObject(pim), Encoding.Default, "text/json");

            var updateResponse = await client.PutAsync($"{BaseUrl}/{createModel.Id}", updateContent);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);


            // GET with id
            var getResponse = await client.GetAsync($"{BaseUrl}/{createModel.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getBody = await getResponse.Content.ReadAsStringAsync();
            var getModel = JsonConvert.DeserializeObject<ProductOutputModel>(getBody);
            Assert.Equal("ChangedNameInTest", getModel.Name);
            Assert.Equal(98, getModel.Price);


            // DELETE with id
            var deleteResponse = await client.DeleteAsync($"{BaseUrl}/{getModel.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // GET with id
            var getNoResponse = await client.GetAsync($"{BaseUrl}/{createModel.Id}");
            Assert.Equal(HttpStatusCode.NoContent, getNoResponse.StatusCode);
        }
    }
}