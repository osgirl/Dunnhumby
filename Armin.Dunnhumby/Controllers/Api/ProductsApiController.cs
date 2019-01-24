using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Filters;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Models;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Armin.Dunnhumby.Web.Controllers.Api
{
    // https://www.c-sharpcorner.com/article/creating-crud-api-in-asp-net-core-2-0/

    [Route("api/v1/products")]
    [ApiController]
    [ValidateModelState]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductStore _store;

        public ProductsApiController(IProductStore store)
        {
            _store = store;
        }

        // GET api/v1/products/list
        [HttpGet("list", Name = "ListProduct")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] {"page", "size"})]
        public ActionResult<PagedResult<IEnumerable<ProductOutputModel>>> List([FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var products = _store.List(page, size);
            List<ProductOutputModel> productsOutput = products.Data.Select(ProductOutputModel.FromEntity).ToList();
            PagedResult<ProductOutputModel> productsOutputPage = new PagedResult<ProductOutputModel>()
            {
                Data = productsOutput,
                Page = products.Page,
                PageCount = products.PageCount,
                PageSize = products.PageCount,
                RecordCount = products.RecordCount
            };
            return Ok(productsOutputPage);
        }


        // GET api/v1/products/{id}
        /// <summary>
        /// Retreive full data and model of given product
        /// </summary>
        /// <param name="id">Product Id as it appear on data signiture</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ViewProduct")]
        public ActionResult<ProductOutputModel> Get(int id)
        {
            Product product = _store.GetById(id);
            if (null == product) return NoContent();

            return ProductOutputModel.FromEntity(product);
        }

        // POST api/v1/products
        [HttpPost(Name = "CreateProduct")]
        [ProducesResponseType(201)]
        public IActionResult Create([FromBody] ProductInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var product = inputModel.ToEntity();


            product = _store.Create(product);

            var outputModel = ProductOutputModel.FromEntity(product);

            var result = CreatedAtRoute("ViewProduct",
                new {id = outputModel.Id}, outputModel);
            return result;
        }

        // PUT api/v1/products
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public IActionResult Update(int id, [FromBody] ProductInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var product = _store.GetById(id);
            if (null == product) return NotFound();

            product.Name = inputModel.Name;
            product.Price = inputModel.Price;
            product.Description = inputModel.Description;


            _store.Update(product);

            return NoContent();
        }


        // DELETE api/v1/products/{id}
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(204)]
        public IActionResult Delete(int id)
        {
            var product = _store.GetById(id);
            if (null == product) return NotFound();

            _store.Delete(product);

            return NoContent();
        }
    }
}