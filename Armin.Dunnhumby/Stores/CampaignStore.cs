using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Web.Stores
{
    public class CampaignStore : Store<Campaign>, ICampaignStore
    {
        public CampaignStore(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        public override void Delete(Campaign campaign)
        {
            base.Delete(campaign);
        }

        public override Campaign GetById(int id)
        {
            return Table.FirstOrDefault(c => c.Id == id);
        }

        public override IQueryable<Campaign> Table => base.Table.Include("Product");
    }
}