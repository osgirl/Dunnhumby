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

    [Route("api/v1/campaigns")]
    [ApiController]
    [ValidateModelState]
    public class CampaignApiController : ControllerBase
    {
        private readonly ICampaignStore _store;

        public CampaignApiController(ICampaignStore store)
        {
            _store = store;
        }

        // GET api/v1/campaigns/list
        [HttpGet("list", Name = "ListCampaigns")]
        public ActionResult<PagedResult<IEnumerable<CampaignOutputModel>>> Search([FromQuery] int page = 1, [FromQuery] int size = 10)
        {

            var campaigns = _store.List(page, size);
            List<CampaignOutputModel> campaignsOutput = campaigns.Data.Select(CampaignOutputModel.FromEntity).ToList();
            PagedResult<CampaignOutputModel> campaignsOutputPage = new PagedResult<CampaignOutputModel>()
            {
                Data = campaignsOutput,
                Page = campaigns.Page,
                PageCount = campaigns.PageCount,
                PageSize = campaigns.PageCount,
                RecordCount = campaigns.RecordCount
            };
            return Ok(campaignsOutputPage);
        }


        // GET api/v1/campaigns/{id}
        /// <summary>
        /// Retreive full data and model of given campaign
        /// </summary>
        /// <param name="id">Campaign Id as it appear on data signiture</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ViewCampaign")]
        public ActionResult<CampaignOutputModel> Get(int id)
        {
            Campaign campaign = _store.GetById(id);
            if (null == campaign) return NoContent();

            return CampaignOutputModel.FromEntity(campaign);
        }

        // POST api/v1/campaigns
        [HttpPost(Name = "CreateCampaign")]
        [ProducesResponseType(201)]
        public IActionResult Create([FromForm] CampaignInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var campaign = inputModel.ToEntity();


            campaign = _store.Create(campaign);

            var outputModel = CampaignOutputModel.FromEntity(campaign);
            return CreatedAtRoute("ViewCampaign",
                new { id = outputModel.Id }, outputModel);
        }

        // PUT api/v1/campaigns
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public IActionResult Update(int id, [FromForm] CampaignInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            var campaign = _store.GetById(id);
            if (null == campaign) return NotFound();

            campaign.Name = inputModel.Name;
            campaign.ProductId = inputModel.ProductId;


            _store.Update(campaign);

            return NoContent();
        }


        // DELETE api/v1/campaigns/{id}
        [HttpDelete("{id:int}", Name = "DeleteCampaign")]
        [ProducesResponseType(204)]
        public IActionResult Delete(int id)
        {
            var campaign = _store.GetById(id);
            if (null == campaign) return NotFound();

            _store.Delete(campaign);

            return NoContent();
        }
    }
}