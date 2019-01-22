using System.Collections.Generic;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;

namespace Armin.Dunnhumby.Web.Stores
{
    public interface ICampaignStore
    {
        Campaign Create(Campaign entity);
        Campaign GetById(int id);
        void Update(Campaign entity);
        void Delete(Campaign entity);
        List<Campaign> List();
        PagedResult<Campaign> List(int page, int pageSize = 10);
    }
}