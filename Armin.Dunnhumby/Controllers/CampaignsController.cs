using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Models;
using Armin.Dunnhumby.Web.Services;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Web.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly ICampaignService _service;
        private readonly IProductStore _productStore;

        public CampaignsController(ICampaignService service, IProductStore productStore)
        {
            _service = service;
            _productStore = productStore;
        }

        // GET: Campaigns
        [HttpGet("/Campaigns/{active?}")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] {"active", "page", "size"})]
        public IActionResult Index([FromRoute] ActiveStates active = ActiveStates.All, [FromQuery] int page = 1,
            [FromQuery] int size = 5)
        {
            PagedResult<Campaign> campaigns = active == ActiveStates.Active
                ? _service.ActiveList(page, size)
                : _service.List(page, size);
            var productsOutput = campaigns.Data.Select(CampaignOutputModel.FromEntity).ToList();
            PagedResult<CampaignOutputModel> campaignsOutputPage = new PagedResult<CampaignOutputModel>
            {
                Data = productsOutput,
                Page = campaigns.Page,
                PageCount = campaigns.PageCount,
                PageSize = campaigns.PageCount,
                RecordCount = campaigns.RecordCount
            };
            ViewData["active"] = active;
            return View(campaignsOutputPage);
        }

        // GET: Campaigns/Details/5
        public IActionResult Details(int? id)
        {
            Campaign campaign = FetchOrDefault(id);
            if (null == campaign) return NotFound();

            return View(CampaignOutputModel.FromEntity(campaign));
        }

        // GET: Campaigns/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_productStore.List(), "Id", "Name");
            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CampaignInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = new SelectList(_productStore.List(), "Id", "Name");
                return View(inputModel);
            }

            var campaign = inputModel.ToEntity();
            if (!_productStore.Exists(campaign.ProductId))
            {
                ModelState.AddModelError("Product", $"Product '{campaign.ProductId}' does not exists.");
                return BadRequest(ModelState);
            }

            campaign = _service.Create(campaign);

            var outputModel = CampaignOutputModel.FromEntity(campaign);
            return RedirectToAction(nameof(Index));
        }

        // GET: Campaigns/Edit/5
        public IActionResult Edit(int? id)
        {
            Campaign campaign = FetchOrDefault(id);
            if (null == campaign) return NotFound();

            ViewData["ProductId"] = new SelectList(_productStore.List(), "Id", "Name", campaign.ProductId);
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm] CampaignInputModel inputModel)
        {
            if (inputModel == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = new SelectList(_productStore.List(), "Id", "Name", inputModel.ProductId);
                return View(inputModel.ToEntity());
            }

            int campaignId = id;

            if (!_productStore.Exists(inputModel.ProductId))
            {
                ModelState.AddModelError("Product", $"Product '{inputModel.ProductId}' does not exists.");
                return BadRequest(ModelState);
            }

            try
            {
                var campaign = _service.GetById(campaignId);
                if (null == campaign) return NotFound();

                campaign.Name = inputModel.Name;
                campaign.Description = inputModel.Description;
                campaign.End = inputModel.End;
                campaign.Start = inputModel.Start;

                campaign.ProductId = inputModel.ProductId;

                _service.Update(campaign);
                campaignId = campaign.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.Exists(campaignId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Campaigns/Delete/5
        public IActionResult Delete(int? id)
        {
            Campaign campaign = FetchOrDefault(id);
            if (null == campaign) return NotFound();

            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Campaign campaign = FetchOrDefault(id);
            if (null == campaign) return NotFound();
            _service.Delete(campaign);

            return RedirectToAction(nameof(Index));
        }

        private Campaign FetchOrDefault(int? id)
        {
            if (id == null) return null;
            return _service.GetById((int) id);
        }
    }
}