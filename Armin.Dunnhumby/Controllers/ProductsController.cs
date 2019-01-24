using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Models;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductStore _store;

        public ProductsController(IProductStore store)
        {
            _store = store;
        }

        // GET: Products
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] {"page", "size"})]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            var products = _store.List(page, size);
            List<ProductOutputModel> productsOutput = products.Data.Select(ProductOutputModel.FromEntity).ToList();
            PagedResult<ProductOutputModel> productsOutputPage = new PagedResult<ProductOutputModel>
            {
                Data = productsOutput,
                Page = products.Page,
                PageCount = products.PageCount,
                PageSize = products.PageCount,
                RecordCount = products.RecordCount
            };
            return View(productsOutputPage);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            Product product = FetchOrDefault(id);
            if (null == product) return NotFound();

            return View(ProductOutputModel.FromEntity(product));
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ProductInputModel inputModel)
        {
            if (!ModelState.IsValid) return View(inputModel);

            var product = inputModel.ToEntity();


            product = _store.Create(product);

            var outputModel = ProductOutputModel.FromEntity(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            Product product = FetchOrDefault(id);
            if (null == product) return NotFound();
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm] ProductInputModel inputModel)
        {
            if (inputModel == null) return View();
            if (!ModelState.IsValid) return View(inputModel.ToEntity());

            int productId = id;

            try
            {
                var product = _store.GetById(productId);
                if (null == product) return NotFound();

                product.Name = inputModel.Name;
                product.Description = inputModel.Description;
                product.Price = inputModel.Price;


                _store.Update(product);
                productId = product.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_store.Exists(productId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            Product product = FetchOrDefault(id);
            if (null == product) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Product product = FetchOrDefault(id);
            if (null == product) return NotFound();
            _store.Delete(product);

            return RedirectToAction(nameof(Index));
        }

        private Product FetchOrDefault(int? id)
        {
            if (id == null) return null;
            return _store.GetById((int) id);
        }
    }
}