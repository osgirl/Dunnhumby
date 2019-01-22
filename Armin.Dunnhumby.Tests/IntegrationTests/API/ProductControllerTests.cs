using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Models;
using Newtonsoft.Json;
using Xunit;

namespace Armin.Dunnhumby.Tests.IntegrationTests.API
{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ProductControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Get_Search()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/products/list/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<ProductOutputModel[]>(responseString);
            Assert.True(searchResult.Length > 0);
        }

        [Fact]
        public async Task Create_Update_Delete()
        {
            var client = _factory.CreateClient();

            // POST Create

            MultipartFormDataContent createForm = new MultipartFormDataContent
            {
                {new StringContent("CreatedWithTest"), "name"},
                {new StringContent("25"), "price"}
            };

            var createResponse = await client.PostAsync("/api/v1/products", createForm);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var responseString = await createResponse.Content.ReadAsStringAsync();
            var createModel = JsonConvert.DeserializeObject<ProductOutputModel>(responseString);
            Assert.NotNull(createModel);
            Assert.Equal("CreatedWithTest", createModel.Name);
            Assert.Equal(25, createModel.Price);


            // PUT Update

            MultipartFormDataContent updateForm = new MultipartFormDataContent
            {
                {new StringContent("ChangedNameInTest"), "name"},
                {new StringContent("98"), "price"}
            };

            var updateResponse = await client.PutAsync("/api/v1/products/" + createModel.Id, updateForm);
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);


            // GET with id
            var getResponse = await client.GetAsync("/api/v1/products/" + createModel.Id);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getBody = await getResponse.Content.ReadAsStringAsync();
            var getModel = JsonConvert.DeserializeObject<ProductOutputModel>(getBody);
            Assert.Equal("ChangedNameInTest", getModel.Name);
            Assert.Equal(98, getModel.Price);


            // DELETE with id

            var deleteResponse = await client.DeleteAsync("/api/v1/products/" + getModel.Id);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // GET with id
            var getNoResponse = await client.GetAsync("/api/v1/products/" + createModel.Id);
            Assert.Equal(HttpStatusCode.NoContent, getNoResponse.StatusCode);
        }
    }
}