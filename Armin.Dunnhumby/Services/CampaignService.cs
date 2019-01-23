using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Stores;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Armin.Dunnhumby.Web.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignStore _store;

        public CampaignService(ICampaignStore store)
        {
            _store = store;
        }


        public Campaign Create(Campaign entity)
        {
            return _store.Create(entity);
        }

        public bool Exists(int id)
        {
            return _store.Exists(id);
        }

        public Campaign GetById(int id)
        {
            return _store.GetById(id);
        }

        public void Update(Campaign entity)
        {
            _store.Update(entity);
        }

        public void Delete(Campaign entity)
        {
            _store.Delete(entity);
        }

        public List<Campaign> List()
        {
            return _store.List();
        }

        public PagedResult<Campaign> List(int page, int pageSize = 10)
        {
            return _store.List(page, pageSize);
        }

        public IQueryable<Campaign> Table => _store.Table;

        public PagedResult<Campaign> ActiveList(int page, int pageSize = 10)
        {
            DateTime now = DateTime.Now;
            return _store.Table.Where(c => c.Start <= now && c.End >= now).ToPageOf(page, pageSize);
        }
    }
}