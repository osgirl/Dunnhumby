using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Helpers;

namespace Armin.Dunnhumby.Domain.Stores
{
    public interface ICampaignStore
    {
        Campaign Create(Campaign entity);
        bool Exists(int id);
        Campaign GetById(int id);
        void Update(Campaign entity);
        void Delete(Campaign entity);
        List<Campaign> List();
        PagedResult<Campaign> List(int page, int pageSize = 10);

        IQueryable<Campaign> Table { get; }
    }
}