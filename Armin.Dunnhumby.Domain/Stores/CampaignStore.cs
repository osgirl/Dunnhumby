using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Domain.Data;
using Armin.Dunnhumby.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Domain.Stores
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